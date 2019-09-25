using CsvHelper.Configuration;
using ESFA.DC.ILR.ReferenceDataService.Model.CsvModels;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Mappers
{
    public sealed class ValidationMessagesMapper : ClassMap<ValidationMessagesModel>
    {
        public ValidationMessagesMapper()
        {
            Map(m => m.RuleName).Index(0).Name("Rule Name");
            Map(m => m.ErrorMessage).Index(1).Name("Error Message");
            Map(m => m.Severity).Index(2).Name("Severity");
            Map(m => m.EnabledSLD).Index(3).Name("Enabled SLD");
            Map(m => m.EnabledDesktop).Index(4).Name("Enabled Desktop");
        }
    }
}
