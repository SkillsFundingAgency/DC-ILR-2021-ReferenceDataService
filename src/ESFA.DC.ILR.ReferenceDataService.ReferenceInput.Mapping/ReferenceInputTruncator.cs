﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping
{
    public class ReferenceInputTruncator : IReferenceInputTruncator
    {

        private const string ClearLarsVersion = @"
TRUNCATE TABLE [ReferenceInput].[LARS_LARSVersion];";

        private const string ClearLarsStandardsSql = @"
TRUNCATE TABLE [ReferenceInput].[LARS_LARSStandardApprenticeshipFunding];
TRUNCATE TABLE [ReferenceInput].[LARS_LARSStandardCommonComponent];
TRUNCATE TABLE [ReferenceInput].[LARS_LARSStandardFunding];
TRUNCATE TABLE [ReferenceInput].[LARS_LARSStandardValidity];

DELETE FROM [ReferenceInput].[LARS_LARSStandard];
DBCC CHECKIDENT ('[ReferenceInput].[LARS_LARSStandard]', RESEED, 1);";

        private const string ClearLarsLearningDelivery = @"
TRUNCATE TABLE [ReferenceInput].[LARS_LARSFunding];
TRUNCATE TABLE [ReferenceInput].[LARS_LARSAnnualValue];
TRUNCATE TABLE [ReferenceInput].[LARS_LARSLearningDeliveryCategory];
TRUNCATE TABLE [ReferenceInput].[LARS_LARSValidity];

DELETE FROM [ReferenceInput].[LARS_LARSLearningDelivery];
DBCC CHECKIDENT ('[ReferenceInput].[LARS_LARSLearningDelivery]', RESEED, 1);";

        private const string ClearLarsFrameworkDesktops = @"
TRUNCATE TABLE [ReferenceInput].[LARS_LARSFrameworkApprenticeshipFunding];
TRUNCATE TABLE [ReferenceInput].[LARS_LARSFrameworkCommonComponent];

DELETE FROM [ReferenceInput].[LARS_LARSFrameworkDesktop];
DBCC CHECKIDENT ('[ReferenceInput].[LARS_LARSFrameworkDesktop]', RESEED, 1);";

        private const string ClearLarsFrameworkAims = @"
TRUNCATE TABLE [ReferenceInput].[LARS_LARSFrameworkAim];";

        public async Task TruncateReferenceDataAsync(IInputReferenceDataContext inputReferenceDataContext,
            CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(inputReferenceDataContext.ConnectionString))
            {
                connection.Open();
                using (var trans = connection.BeginTransaction())
                {
                    try
                    {
                        var command = trans.Connection.CreateCommand();
                        //connection.CreateCommand();
                        command.CommandType = CommandType.Text;
                        command.Transaction = trans;

                        command.CommandText = ClearLarsVersion;
                        command.ExecuteNonQuery();

                        command.CommandText = ClearLarsStandardsSql;
                        command.ExecuteNonQuery();

                        command.CommandText = ClearLarsLearningDelivery;
                        command.ExecuteNonQuery();

                        command.CommandText = ClearLarsFrameworkDesktops;
                        command.ExecuteNonQuery();

                        command.CommandText = ClearLarsFrameworkAims;
                        command.ExecuteNonQuery();

                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
