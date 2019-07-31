using ESFA.DC.ILR.Desktop.Interface;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Context;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.CollectionDates;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Tests
{
    public class DesktopContextReturnPeriodUpdateServiceTests
    {
        [Theory]
        [InlineData(2018, 7, 1, 1)]
        [InlineData(2019, 7, 1, 1)]
        [InlineData(2019, 8, 1, 1)]
        [InlineData(2019, 9, 1, 1)]
        [InlineData(2019, 10, 1, 2)]
        [InlineData(2019, 11, 1, 3)]
        [InlineData(2019, 12, 1, 4)]
        [InlineData(2020, 1, 1, 5)]
        [InlineData(2020, 2, 1, 6)]
        [InlineData(2020, 3, 1, 7)]
        [InlineData(2020, 4, 1, 8)]
        [InlineData(2020, 5, 1, 9)]
        [InlineData(2020, 6, 1, 10)]
        [InlineData(2020, 7, 1, 11)]
        [InlineData(2020, 8, 1, 12)]
        [InlineData(2020, 9, 1, 13)]
        [InlineData(2020, 10, 1, 14)]
        [InlineData(2020, 11, 1, 14)]
        [InlineData(2021, 11, 1, 14)]
        public void UpdateCollectionPeriod(int year, int month, int day, int assertion)
        {
            var returnPeriods = ReturnPeriods();

            var desktopContextMock = new Mock<IDesktopContext>();
            desktopContextMock.Setup(dm => dm.KeyValuePairs)
                .Returns(new Dictionary<string, object>
                {
                    { "ReturnPeriod", 0 }
                });

            IReferenceDataContext referenceDataContext = new ReferenceDataJobContextMessageContext(desktopContextMock.Object);

            NewService().UpdateCollectionPeriod(referenceDataContext, new DateTime(year, month, day), returnPeriods);

            referenceDataContext.ReturnPeriod.Should().Be(assertion);
        }

        private IReadOnlyCollection<ReturnPeriod> ReturnPeriods()
        {
            return new List<ReturnPeriod>
            {
                new ReturnPeriod { Name = "R01", Period = 1, Start = new DateTime(2019, 08, 23), End = new DateTime(2019, 09, 06, 23, 59, 59) },
                new ReturnPeriod { Name = "R02", Period = 2, Start = new DateTime(2019, 09, 07), End = new DateTime(2019, 10, 04, 23, 59, 59) },
                new ReturnPeriod { Name = "R03", Period = 3, Start = new DateTime(2019, 10, 05), End = new DateTime(2019, 11, 06, 23, 59, 59) },
                new ReturnPeriod { Name = "R04", Period = 4, Start = new DateTime(2019, 11, 07), End = new DateTime(2019, 12, 06, 23, 59, 59) },
                new ReturnPeriod { Name = "R05", Period = 5, Start = new DateTime(2019, 12, 07), End = new DateTime(2020, 01, 07, 23, 59, 59) },
                new ReturnPeriod { Name = "R06", Period = 6, Start = new DateTime(2020, 01, 08), End = new DateTime(2020, 02, 06, 23, 59, 59) },
                new ReturnPeriod { Name = "R07", Period = 7, Start = new DateTime(2020, 02, 07), End = new DateTime(2020, 03, 06, 23, 59, 59) },
                new ReturnPeriod { Name = "R08", Period = 8, Start = new DateTime(2020, 03, 07), End = new DateTime(2020, 04, 04, 23, 59, 59) },
                new ReturnPeriod { Name = "R09", Period = 9, Start = new DateTime(2020, 04, 05), End = new DateTime(2020, 05, 07, 23, 59, 59) },
                new ReturnPeriod { Name = "R10", Period = 10, Start = new DateTime(2020, 05, 08), End = new DateTime(2020, 06, 06, 23, 59, 59) },
                new ReturnPeriod { Name = "R11", Period = 11, Start = new DateTime(2020, 06, 07), End = new DateTime(2020, 07, 04, 23, 59, 59) },
                new ReturnPeriod { Name = "R12", Period = 12, Start = new DateTime(2020, 07, 05), End = new DateTime(2020, 08, 06, 23, 59, 59) },
                new ReturnPeriod { Name = "R13", Period = 13, Start = new DateTime(2020, 08, 07), End = new DateTime(2020, 09, 13, 23, 59, 59) },
                new ReturnPeriod { Name = "R14", Period = 14, Start = new DateTime(2020, 09, 14), End = new DateTime(2020, 10, 17, 23, 59, 59) },
            };
        }

        private DesktopContextReturnPeriodUpdateService NewService()
        {
            return new DesktopContextReturnPeriodUpdateService();
        }
    }
}
