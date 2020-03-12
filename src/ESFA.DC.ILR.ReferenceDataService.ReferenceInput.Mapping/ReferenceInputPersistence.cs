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

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping
{
    public class ReferenceInputPersistence : IReferenceInputPersistence
    {
        public async Task PersistEfModelByTypeAsync<T>(SqlConnection connection, SqlTransaction sqlTransaction,
            CancellationToken cancellationToken, IEnumerable<T> source)
        {
            // Need to recurse through collections to get child types and also save.

            var tableName = GetTableNameFromType<T>();


            var bulkInsert = new BulkInsert();
            await bulkInsert.InsertWithIdsAsync(tableName, source, connection, sqlTransaction, cancellationToken);

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
                    .Invoke(this, new object[] { connection, sqlTransaction, cancellationToken, subItems });
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

        public async Task PersistEFModelsAsync(IInputReferenceDataContext inputReferenceDataContext,
            IEFReferenceInputDataRoot efModels, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(inputReferenceDataContext.ConnectionString))
            {
                connection.Open();
                var trans = connection.BeginTransaction();
                try
                {
                    var bulkInsert = new BulkInsert();

                    await BulkInsertLarsVersion(efModels, cancellationToken, bulkInsert, connection, trans);
                    await BulkInsertLarsStandards(efModels, cancellationToken, bulkInsert, connection, trans);
                    await BulkInsertLarsLearningDelivery(efModels, cancellationToken, bulkInsert, connection, trans);
                    await BulkInsertLarsFrameworkDesktop(efModels, cancellationToken, bulkInsert, connection, trans);
                    await BulkInsertLarsFrameworkAims(efModels, cancellationToken, bulkInsert, connection, trans);

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }

            }
        }

        private static async Task BulkInsertLarsVersion(IEFReferenceInputDataRoot efModels,
            CancellationToken cancellationToken, BulkInsert bulkInsert, SqlConnection connection, SqlTransaction trans)
        {
            await bulkInsert.InsertWithIdsAsync("ReferenceInput.LARS_LARSVersion", new List<LARS_LARSVersion> {efModels.Lars_LarsVersion}, connection,
                trans, cancellationToken);
        }

        private static async Task BulkInsertLarsStandards(IEFReferenceInputDataRoot efModels,
            CancellationToken cancellationToken, BulkInsert bulkInsert, SqlConnection connection, SqlTransaction trans)
        {
            await bulkInsert.InsertWithIdsAsync("ReferenceInput.LARS_LARSStandard", efModels.Lars_LarsStandards, connection,
                trans, cancellationToken);

            await DoSubTableBulkInsert<LARS_LARSStandard, LARS_LARSStandardApprenticeshipFunding>("ReferenceInput.LARS_LARSStandardApprenticeshipFunding",
                efModels.Lars_LarsStandards, s => s.LARS_LARSStandardApprenticeshipFundings,
                cancellationToken, bulkInsert, connection, trans);
            await DoSubTableBulkInsert<LARS_LARSStandard, LARS_LARSStandardCommonComponent>("ReferenceInput.LARS_LARSStandardCommonComponent",
                efModels.Lars_LarsStandards, s => s.LARS_LARSStandardCommonComponents,
                cancellationToken, bulkInsert, connection, trans);
            await DoSubTableBulkInsert<LARS_LARSStandard, LARS_LARSStandardFunding>("ReferenceInput.LARS_LARSStandardFunding",
                efModels.Lars_LarsStandards, s => s.LARS_LARSStandardFundings,
                cancellationToken, bulkInsert, connection, trans);
            await DoSubTableBulkInsert<LARS_LARSStandard, LARS_LARSStandardValidity>("ReferenceInput.LARS_LARSStandardValidity",
                efModels.Lars_LarsStandards, s => s.LARS_LARSStandardValidities,
                cancellationToken, bulkInsert, connection, trans);
        }

        private static async Task BulkInsertLarsLearningDelivery(IEFReferenceInputDataRoot efModels,
            CancellationToken cancellationToken, BulkInsert bulkInsert, SqlConnection connection, SqlTransaction trans)
        {
            await bulkInsert.InsertWithIdsAsync("ReferenceInput.LARS_LARSLearningDelivery",
                efModels.Lars_LarsLearningDeliveries, connection, trans, cancellationToken);


            await DoSubTableBulkInsert<LARS_LARSLearningDelivery, LARS_LARSFunding>("ReferenceInput.LARS_LARSFunding",
                efModels.Lars_LarsLearningDeliveries, s => s.LARS_LARSFundings,
                cancellationToken, bulkInsert, connection, trans);
            await DoSubTableBulkInsert<LARS_LARSLearningDelivery, LARS_LARSAnnualValue>("ReferenceInput.LARS_LARSAnnualValue",
                efModels.Lars_LarsLearningDeliveries, s => s.LARS_LARSAnnualValues,
                cancellationToken, bulkInsert, connection, trans);
            await DoSubTableBulkInsert<LARS_LARSLearningDelivery, LARS_LARSLearningDeliveryCategory>("ReferenceInput.LARS_LARSLearningDeliveryCategory",
                efModels.Lars_LarsLearningDeliveries, s => s.LARS_LARSLearningDeliveryCategories,
                cancellationToken, bulkInsert, connection, trans);
            await DoSubTableBulkInsert<LARS_LARSLearningDelivery, LARS_LARSValidity>("ReferenceInput.LARS_LARSValidity",
                efModels.Lars_LarsLearningDeliveries, s => s.LARS_LARSValidities,
                cancellationToken, bulkInsert, connection, trans);
        }

        private static async Task BulkInsertLarsFrameworkDesktop(IEFReferenceInputDataRoot efModels,
            CancellationToken cancellationToken, BulkInsert bulkInsert, SqlConnection connection, SqlTransaction trans)
        {
            await bulkInsert.InsertWithIdsAsync("ReferenceInput.LARS_LARSFrameworkDesktop",
                efModels.Lars_LarsFrameworkDesktops, connection, trans, cancellationToken);

            await DoSubTableBulkInsert<LARS_LARSFrameworkDesktop, LARS_LARSFrameworkApprenticeshipFunding>("ReferenceInput.LARS_LARSFrameworkApprenticeshipFunding",
                efModels.Lars_LarsFrameworkDesktops, s => s.LARS_LARSFrameworkApprenticeshipFundings,
                cancellationToken, bulkInsert, connection, trans);
            await DoSubTableBulkInsert<LARS_LARSFrameworkDesktop, LARS_LARSFrameworkCommonComponent>("ReferenceInput.LARS_LARSFrameworkCommonComponent",
                efModels.Lars_LarsFrameworkDesktops, s => s.LARS_LARSFrameworkCommonComponents,
                cancellationToken, bulkInsert, connection, trans);
        }

        private static async Task BulkInsertLarsFrameworkAims(IEFReferenceInputDataRoot efModels,
            CancellationToken cancellationToken, BulkInsert bulkInsert, SqlConnection connection, SqlTransaction trans)
        {
            await bulkInsert.InsertWithIdsAsync("ReferenceInput.LARS_LARSFrameworkAim",
                efModels.Lars_LarsFrameworkAims, connection, trans, cancellationToken);
        }

        private static async Task DoSubTableBulkInsert<T, T1>(string tableName, IEnumerable<T> topLevelCollection, Func<T, ICollection<T1>> func,
            CancellationToken cancellationToken, BulkInsert bulkInsert, SqlConnection connection, SqlTransaction trans)
        {
            var subCollection = topLevelCollection.SelectMany(func);
            await bulkInsert.InsertWithIdsAsync(tableName, subCollection, connection, trans, cancellationToken);
        }
    }
}
