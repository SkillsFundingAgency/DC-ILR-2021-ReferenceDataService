using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping.Interface;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ESFA.DC.Logging.Interfaces;
using Microsoft.EntityFrameworkCore.Internal;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping
{
    public class ReferenceInputPersistence : IReferenceInputPersistence
    {
        private readonly ILogger _logger;

        public ReferenceInputPersistence(ILogger logger)
        {
            _logger = logger;
        }

        public void PersistEfModelByType<T>(SqlConnection connection, SqlTransaction sqlTransaction, int bulkCopyTimeout, IEnumerable<T> source)
        {
            // Need to recurse through collections to get child types and also save.
            var tableName = GetTableNameFromType<T>();

            _logger.LogInfo($"  Starting Bulk insert of {source.Count()} items of type {typeof(T).Name}");
            var bulkInsert = new BulkInsert();
            bulkInsert.InsertWithIds(tableName, source, connection, sqlTransaction, bulkCopyTimeout);
            _logger.LogInfo($"  Finished Bulk insert of {source.Count()} items of type {typeof(T).Name}");

            Type objType = typeof(T);
            var properties = objType.GetProperties().ToList();


            // Deal with any children in collections
            var listsOfCollectionChildEntitys = properties.Where(p =>
                    p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
                .ToList();

            foreach (var childEntityList in listsOfCollectionChildEntitys)
            {
                var childListType = childEntityList.PropertyType.GenericTypeArguments.First();

                var listType = typeof(List<>);
                var constructedListType = listType.MakeGenericType(childListType);

                var subItems = Activator.CreateInstance(constructedListType) as IList;

                if (subItems != null)
                {
                    foreach (T item in source)
                    {
                        object propValue = childEntityList.GetValue(item, null);
                        var elems = propValue as IEnumerable;
                        if (elems != null)
                        {
                            foreach (var elem in elems)
                            {
                                subItems.Add(elem);
                            }
                        }
                    }

                    if (subItems.Any())
                    {
                        // Create a generic version from a runtime type to call with this child set of items
                        this.GetType()
                            .GetMethod("PersistEfModelByType")
                            .MakeGenericMethod(childListType)
                            .Invoke(this, new object[] {connection, sqlTransaction, bulkCopyTimeout, subItems});
                    }
                }
            }
        }

        public void PersistEfModelByTypeWithoutCollections<T>(SqlConnection connection, SqlTransaction sqlTransaction, int bulkCopyTimeout, IEnumerable<T> source)
        {
            // Need to recurse through collections to get child types and also save.

            var tableName = GetTableNameFromType<T>();

            _logger.LogInfo($"  Starting Bulk insert of {source.Count()} items of type {typeof(T).Name}");
            var bulkInsert = new BulkInsert();
            bulkInsert.InsertWithIds(tableName, source, connection, sqlTransaction, bulkCopyTimeout);
            _logger.LogInfo($"  Finished Bulk insert of {source.Count()} items of type {typeof(T).Name}");

            Type objType = typeof(T);
            var properties = objType.GetProperties().ToList();

            // Deal with any children not in a collection
            var instanceOfChildEntities = properties.Where(p => p.PropertyType.IsClass && p.PropertyType.Assembly.FullName.Contains("ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model")).ToList();

            foreach (var instanceOfChildEntity in instanceOfChildEntities)
            {
                var childListType = instanceOfChildEntity.PropertyType;

                var listType = typeof(List<>);
                var constructedListType = listType.MakeGenericType(childListType);

                var subItems = Activator.CreateInstance(constructedListType) as IList;

                foreach (T item in source)
                {
                    object childValue = instanceOfChildEntity.GetValue(item, null);

                    if (childValue != null)
                    {
                        subItems.Add(childValue);
                    }
                }

                // Create a generic version from a runtime type to call with this child set of items
                this.GetType()
                    .GetMethod("PersistEfModelByType")
                    .MakeGenericMethod(childListType)
                    .Invoke(this, new object[] { connection, sqlTransaction, bulkCopyTimeout, subItems });
            }
        }

        private string GetTableNameFromType<T>()
        {
            using (var context = new ReferenceInputDataContext())
            {
                var entityTypes = context.Model.GetEntityTypes();
                var entityTypeOfT = entityTypes.First(t => t.ClrType == typeof(T));

                // The entity type information has the table name as an annotation!
                var tableNameAnnotation = entityTypeOfT.GetAnnotation("Relational:TableName");
                var tableNameSchema = entityTypeOfT.GetAnnotation("Relational:Schema");
                var tableName = $"{tableNameSchema.Value.ToString()}.{tableNameAnnotation.Value.ToString()}";

                return tableName;
            }
        }
    }
}
