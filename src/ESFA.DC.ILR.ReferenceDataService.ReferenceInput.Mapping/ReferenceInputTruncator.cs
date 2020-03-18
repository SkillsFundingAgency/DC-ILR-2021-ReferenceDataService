using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Permissions;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping
{
    public class ReferenceInputTruncator : IReferenceInputTruncator
    {
        private const string ClearMetaData = @"
TRUNCATE TABLE [ReferenceInput].[MetaData_LookupSubCategory]
DELETE FROM [ReferenceInput].[MetaData_Lookup];
DBCC CHECKIDENT ('[ReferenceInput].[MetaData_Lookup]', RESEED, 1);

-- ReferenceDataVersion
DELETE FROM [ReferenceInput].[MetaData_ReferenceDataVersion];
DBCC CHECKIDENT ('[ReferenceInput].[MetaData_ReferenceDataVersion]', RESEED, 1);

-- ReferenceDataVersion - child tables
DELETE FROM [ReferenceInput].[MetaData_CampusIdentifierVersion];
DBCC CHECKIDENT ('[ReferenceInput].[MetaData_CampusIdentifierVersion]', RESEED, 1);
DELETE FROM [ReferenceInput].[MetaData_CoFVersion];
DBCC CHECKIDENT ('[ReferenceInput].[MetaData_CoFVersion]', RESEED, 1);
DELETE FROM [ReferenceInput].[MetaData_DevolvedPostcodesVersion];
DBCC CHECKIDENT ('[ReferenceInput].[MetaData_DevolvedPostcodesVersion]', RESEED, 1);
DELETE FROM [ReferenceInput].[MetaData_EasUploadDateTime];
DBCC CHECKIDENT ('[ReferenceInput].[MetaData_EasUploadDateTime]', RESEED, 1);
DELETE FROM [ReferenceInput].[MetaData_EmployersVersion];
DBCC CHECKIDENT ('[ReferenceInput].[MetaData_EmployersVersion]', RESEED, 1);
DELETE FROM [ReferenceInput].[MetaData_HmppPostcodesVersion];
DBCC CHECKIDENT ('[ReferenceInput].[MetaData_HmppPostcodesVersion]', RESEED, 1);
DELETE FROM [ReferenceInput].[MetaData_LarsVersion];
DBCC CHECKIDENT ('[ReferenceInput].[MetaData_LarsVersion]', RESEED, 1);
DELETE FROM [ReferenceInput].[MetaData_OrganisationsVersion];
DBCC CHECKIDENT ('[ReferenceInput].[MetaData_OrganisationsVersion]', RESEED, 1);
DELETE FROM [ReferenceInput].[MetaData_PostcodeFactorsVersion];
DBCC CHECKIDENT ('[ReferenceInput].[MetaData_PostcodeFactorsVersion]', RESEED, 1);
DELETE FROM [ReferenceInput].[MetaData_PostcodesVersion];
DBCC CHECKIDENT ('[ReferenceInput].[MetaData_PostcodesVersion]', RESEED, 1);

TRUNCATE TABLE [ReferenceInput].[MetaData_CensusDate]
TRUNCATE TABLE [ReferenceInput].[MetaData_ReturnPeriod]
TRUNCATE TABLE [ReferenceInput].[MetaData_MetaData]
TRUNCATE TABLE [ReferenceInput].[MetaData_ValidationError]
TRUNCATE TABLE [ReferenceInput].[MetaData_ValidationRule]";

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

        private const string ClearPostcodes = @"
TRUNCATE TABLE [ReferenceInput].[Postcodes_ONSData];
TRUNCATE TABLE [ReferenceInput].[Postcodes_DasDisadvantage];
TRUNCATE TABLE [ReferenceInput].[Postcodes_EfaDisadvantage];
TRUNCATE TABLE [ReferenceInput].[Postcodes_SfaAreaCost];
TRUNCATE TABLE [ReferenceInput].[Postcodes_SfaDisadvantage];

TRUNCATE TABLE [ReferenceInput].[PostcodesDevolution_McaGlaSofLookup];
TRUNCATE TABLE [ReferenceInput].[PostcodesDevolution_Postcode];

DELETE FROM [ReferenceInput].[Postcodes_Postcode];
DBCC CHECKIDENT ('[ReferenceInput].[Postcodes_Postcode]', RESEED, 1);";

        public void TruncateReferenceData(IInputReferenceDataContext inputReferenceDataContext)
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

                        command.CommandText = ClearMetaData;
                        command.ExecuteNonQuery();

                        command.CommandText = ClearLarsStandardsSql;
                        command.ExecuteNonQuery();

                        command.CommandText = ClearLarsLearningDelivery;
                        command.ExecuteNonQuery();

                        command.CommandText = ClearLarsFrameworkDesktops;
                        command.ExecuteNonQuery();

                        command.CommandText = ClearLarsFrameworkAims;
                        command.ExecuteNonQuery();

                        command.CommandText = ClearPostcodes;
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
