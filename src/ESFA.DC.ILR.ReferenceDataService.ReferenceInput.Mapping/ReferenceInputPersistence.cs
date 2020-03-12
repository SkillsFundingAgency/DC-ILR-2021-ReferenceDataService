using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Mapping.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model.Containers.Interface;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping
{
    public class ReferenceInputPersistence : IReferenceInputPersistence
    {
        private readonly ILogger _logger;

        public ReferenceInputPersistence(ILogger logger)
        {
            _logger = logger;
        }

        public void PersistEfModelByType<T>(SqlConnection connection, SqlTransaction sqlTransaction, IEnumerable<T> source)
        {
            // Need to recurse through collections to get child types and also save.

            var tableName = GetTableNameFromType<T>();

            _logger.LogInfo($"  Starting Bulk insert of {source.Count()} items of type {typeof(T).Name}");
            var bulkInsert = new BulkInsert();
            bulkInsert.InsertWithIds(tableName, source, connection, sqlTransaction);
            _logger.LogInfo($"  Finished Bulk insert of {source.Count()} items of type {typeof(T).Name}");

            Type objType = typeof(T);
            var properties = objType.GetProperties().ToList();
            var listsOfChildEntitys = properties.Where(p =>
                    p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
                .ToList();

            foreach (var childEntityList in listsOfChildEntitys)
            {
                var childListType = childEntityList.PropertyType.GenericTypeArguments.First();

                var listType = typeof(List<>);
                var constructedListType = listType.MakeGenericType(childListType);

                var subItems = Activator.CreateInstance(constructedListType) as IList;

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

                // Create a generic version from a runtime type to call with this child set of items
                this.GetType()
                    .GetMethod("PersistEfModelByTypeAsync")
                    .MakeGenericMethod(childListType)
                    .Invoke(this, new object[] { connection, sqlTransaction, subItems });
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

        public void PersistEFModels(IInputReferenceDataContext inputReferenceDataContext,
            IEFReferenceInputDataRoot efModels)
        {
            using (var connection = new SqlConnection(inputReferenceDataContext.ConnectionString))
            {
                connection.Open();
                var trans = connection.BeginTransaction();
                try
                {
                    var bulkInsert = new BulkInsert();

                    BulkInsertLarsVersion(efModels, bulkInsert, connection, trans);
                    BulkInsertLarsStandards(efModels, bulkInsert, connection, trans);
                    BulkInsertLarsLearningDelivery(efModels, bulkInsert, connection, trans);
                    BulkInsertLarsFrameworkDesktop(efModels, bulkInsert, connection, trans);
                    BulkInsertLarsFrameworkAims(efModels, bulkInsert, connection, trans);

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }

            }
        }

        private static void BulkInsertLarsVersion(IEFReferenceInputDataRoot efModels,
            BulkInsert bulkInsert, SqlConnection connection, SqlTransaction trans)
        {
            bulkInsert.InsertWithIds("ReferenceInput.LARS_LARSVersion", new List<LARS_LARSVersion> {efModels.Lars_LarsVersion}, connection,
                trans);
        }

        private static void BulkInsertLarsStandards(IEFReferenceInputDataRoot efModels,
            BulkInsert bulkInsert, SqlConnection connection, SqlTransaction trans)
        {
            bulkInsert.InsertWithIds("ReferenceInput.LARS_LARSStandard", efModels.Lars_LarsStandards, connection, trans);

            DoSubTableBulkInsert<LARS_LARSStandard, LARS_LARSStandardApprenticeshipFunding>("ReferenceInput.LARS_LARSStandardApprenticeshipFunding",
                efModels.Lars_LarsStandards, s => s.LARS_LARSStandardApprenticeshipFundings,
                bulkInsert, connection, trans);
            DoSubTableBulkInsert<LARS_LARSStandard, LARS_LARSStandardCommonComponent>("ReferenceInput.LARS_LARSStandardCommonComponent",
                efModels.Lars_LarsStandards, s => s.LARS_LARSStandardCommonComponents,
                bulkInsert, connection, trans);
            DoSubTableBulkInsert<LARS_LARSStandard, LARS_LARSStandardFunding>("ReferenceInput.LARS_LARSStandardFunding",
                efModels.Lars_LarsStandards, s => s.LARS_LARSStandardFundings,
                bulkInsert, connection, trans);
            DoSubTableBulkInsert<LARS_LARSStandard, LARS_LARSStandardValidity>("ReferenceInput.LARS_LARSStandardValidity",
                efModels.Lars_LarsStandards, s => s.LARS_LARSStandardValidities,
                bulkInsert, connection, trans);
        }

        private static void BulkInsertLarsLearningDelivery(IEFReferenceInputDataRoot efModels,
            BulkInsert bulkInsert, SqlConnection connection, SqlTransaction trans)
        {
            bulkInsert.InsertWithIds("ReferenceInput.LARS_LARSLearningDelivery",
                efModels.Lars_LarsLearningDeliveries, connection, trans);


            DoSubTableBulkInsert<LARS_LARSLearningDelivery, LARS_LARSFunding>("ReferenceInput.LARS_LARSFunding",
                efModels.Lars_LarsLearningDeliveries, s => s.LARS_LARSFundings,
                bulkInsert, connection, trans);
            DoSubTableBulkInsert<LARS_LARSLearningDelivery, LARS_LARSAnnualValue>("ReferenceInput.LARS_LARSAnnualValue",
                efModels.Lars_LarsLearningDeliveries, s => s.LARS_LARSAnnualValues,
                bulkInsert, connection, trans);
            DoSubTableBulkInsert<LARS_LARSLearningDelivery, LARS_LARSLearningDeliveryCategory>("ReferenceInput.LARS_LARSLearningDeliveryCategory",
                efModels.Lars_LarsLearningDeliveries, s => s.LARS_LARSLearningDeliveryCategories,
                bulkInsert, connection, trans);
            DoSubTableBulkInsert<LARS_LARSLearningDelivery, LARS_LARSValidity>("ReferenceInput.LARS_LARSValidity",
                efModels.Lars_LarsLearningDeliveries, s => s.LARS_LARSValidities,
                bulkInsert, connection, trans);
        }

        private static void BulkInsertLarsFrameworkDesktop(IEFReferenceInputDataRoot efModels,
            BulkInsert bulkInsert, SqlConnection connection, SqlTransaction trans)
        {
            bulkInsert.InsertWithIds("ReferenceInput.LARS_LARSFrameworkDesktop",
                efModels.Lars_LarsFrameworkDesktops, connection, trans);

            DoSubTableBulkInsert<LARS_LARSFrameworkDesktop, LARS_LARSFrameworkApprenticeshipFunding>("ReferenceInput.LARS_LARSFrameworkApprenticeshipFunding",
                efModels.Lars_LarsFrameworkDesktops, s => s.LARS_LARSFrameworkApprenticeshipFundings,
                bulkInsert, connection, trans);
            DoSubTableBulkInsert<LARS_LARSFrameworkDesktop, LARS_LARSFrameworkCommonComponent>("ReferenceInput.LARS_LARSFrameworkCommonComponent",
                efModels.Lars_LarsFrameworkDesktops, s => s.LARS_LARSFrameworkCommonComponents,
                bulkInsert, connection, trans);
        }

        private static void BulkInsertLarsFrameworkAims(IEFReferenceInputDataRoot efModels,
            BulkInsert bulkInsert, SqlConnection connection, SqlTransaction trans)
        {
            bulkInsert.InsertWithIds("ReferenceInput.LARS_LARSFrameworkAim",
                efModels.Lars_LarsFrameworkAims, connection, trans);
        }

        private static void DoSubTableBulkInsert<T, T1>(string tableName, IEnumerable<T> topLevelCollection, Func<T, ICollection<T1>> func,
            BulkInsert bulkInsert, SqlConnection connection, SqlTransaction trans)
        {
            var subCollection = topLevelCollection.SelectMany(func);
            bulkInsert.InsertWithIds(tableName, subCollection, connection, trans);
        }
    }
}
