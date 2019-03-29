using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model;
using ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;
using static ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ValidationError;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Repository
{
    public class IlrReferenceDataRepositoryServiceTests
    {
        [Fact]
        public async Task RetrieveAsync()
        {
            var validationErrors = new List<ValidationError>
            {
                new ValidationError { RuleName = "Rule1", Severity = SeverityLevel.Error, Message = "Message1" },
                new ValidationError { RuleName = "Rule2", Severity = SeverityLevel.Error, Message = "Message2" },
                new ValidationError { RuleName = "Rule3", Severity = SeverityLevel.Error, Message = "Message3" },
                new ValidationError { RuleName = "Rule4", Severity = SeverityLevel.Error, Message = "Message4" },
                new ValidationError { RuleName = "Rule5", Severity = SeverityLevel.Warning, Message = "Message5" },
                new ValidationError { RuleName = "Rule6", Severity = SeverityLevel.Warning, Message = "Message6" },
                new ValidationError { RuleName = "Rule7", Severity = SeverityLevel.Error, Message = "Message7" },
                new ValidationError { RuleName = "Rule8", Severity = SeverityLevel.Error, Message = "Message8" },
                new ValidationError { RuleName = "Rule9", Severity = SeverityLevel.Error, Message = "Message9" },
                new ValidationError { RuleName = "Rule10", Severity = SeverityLevel.Error, Message = "Message10" },
            };

            var validationErrorsMock = new Mock<IIlrReferenceDataContext>();

            IEnumerable<Rule> errorsList = new List<Rule>
            {
                new Rule { Rulename = "Rule1", Severity = "E", Message = "Message1" },
                new Rule { Rulename = "Rule2", Severity = "E", Message = "Message2" },
                new Rule { Rulename = "Rule3", Severity = "E", Message = "Message3" },
                new Rule { Rulename = "Rule4", Severity = "E", Message = "Message4" },
                new Rule { Rulename = "Rule5", Severity = "W", Message = "Message5" },
                new Rule { Rulename = "Rule6", Severity = "W", Message = "Message6" },
                new Rule { Rulename = "Rule7", Severity = "E", Message = "Message7" },
                new Rule { Rulename = "Rule8", Severity = "E", Message = "Message8" },
                new Rule { Rulename = "Rule9", Severity = "E", Message = "Message9" },
                new Rule { Rulename = "Rule10", Severity = "E", Message = "Message10" },
            };

            var errorsListDbMock = errorsList.AsQueryable().BuildMockDbSet();

            validationErrorsMock.Setup(v => v.Rules).Returns(errorsListDbMock.Object);

            var serviceResult = await NewService(validationErrorsMock.Object).RetrieveValidationErrorsAsync(CancellationToken.None);

            serviceResult.Count().Should().Be(10);
            serviceResult.ToList().Should().BeEquivalentTo(validationErrors);
        }

        private IlrReferenceDataRepositoryService NewService(IIlrReferenceDataContext ilrReferenceDataContext = null)
        {
            return new IlrReferenceDataRepositoryService(ilrReferenceDataContext);
        }
    }
}
