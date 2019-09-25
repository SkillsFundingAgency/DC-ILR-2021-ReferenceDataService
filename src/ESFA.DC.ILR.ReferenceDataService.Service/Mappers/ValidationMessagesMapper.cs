using CsvHelper.Configuration;
using ESFA.DC.ILR.ReferenceDataService.Model.CsvModels;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Mappers
{
    public sealed class ValidationMessagesMapper : ClassMap<ValidationMessagesModel>
    {
        public ValidationMessagesMapper()
        {
            Map(m => m.RuleName).Name("Rule Name");
            Map(m => m.ErrorMessage).Name("Error Message");
            Map(m => m.Severity).Name("Severity");
            Map(m => m.EnabledSLD).Name("Enabled SLD");
            Map(m => m.EnabledDesktop).Name("Enabled Desktop");
        }
    }
}
