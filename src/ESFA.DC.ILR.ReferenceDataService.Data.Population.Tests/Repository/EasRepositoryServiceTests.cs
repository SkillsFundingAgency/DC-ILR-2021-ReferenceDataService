using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.EAS1920.EF;
using ESFA.DC.EAS1920.EF.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ILR.ReferenceDataService.Model.EAS;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;
using EasSubmissionValue = ESFA.DC.ILR.ReferenceDataService.Model.EAS.EasSubmissionValue;
using EasSubmissionValueEF = ESFA.DC.EAS1920.EF.EasSubmissionValue;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Repository
{
    public class EasRepositoryServiceTests
    {
        [Fact]
        public void MapEasValues()
        {
            var paymentPeriod1 = new List<EasPaymentValue> { new EasPaymentValue(1.0m, null) };
            var paymentPeriod2 = new List<EasPaymentValue> { new EasPaymentValue(2.0m, null) };
            var paymentPeriod3 = new List<EasPaymentValue> { new EasPaymentValue(3.0m, null) };

            var easFundingLines = new List<EasFundingLine>
            {
                new EasFundingLine
                {
                    FundLine = "FundLine1",
                    EasSubmissionValues = new List<EasSubmissionValue>
                    {
                        new EasSubmissionValue
                        {
                            AdjustmentTypeName = "AdjustmentTypeName1",
                            PaymentName = "PaymentName1",
                        },
                        new EasSubmissionValue
                        {
                            AdjustmentTypeName = "AdjustmentTypeName11",
                            PaymentName = "PaymentName11",
                        }
                    }
                },
                new EasFundingLine
                {
                    FundLine = "FundLine2",
                    EasSubmissionValues = new List<EasSubmissionValue>
                    {
                        new EasSubmissionValue
                        {
                            AdjustmentTypeName = "AdjustmentTypeName2",
                            PaymentName = "PaymentName2",
                        },
                        new EasSubmissionValue
                        {
                            AdjustmentTypeName = "AdjustmentTypeName22",
                            PaymentName = "PaymentName22",
                        }
                    }
                }
            };

            var easValuesDictionary = new Dictionary<string, Dictionary<string, Dictionary<int, List<EasPaymentValue>>>>
            {
                {
                    "FundLine1", new Dictionary<string, Dictionary<int, List<EasPaymentValue>>>
                    {
                        {
                            "PaymentName1", new Dictionary<int, List<EasPaymentValue>>
                            {
                                { 1, paymentPeriod1 },
                                { 2, paymentPeriod2 }
                            }
                        },
                        {
                            "PaymentName11", new Dictionary<int, List<EasPaymentValue>>
                            {
                                { 1, paymentPeriod1 },
                                { 2, paymentPeriod2 },
                                { 3, paymentPeriod3 }
                            }
                        }
                    }
                },
                {
                    "FundLine2", new Dictionary<string, Dictionary<int, List<EasPaymentValue>>>
                    {
                        {
                            "PaymentName2", new Dictionary<int, List<EasPaymentValue>>
                            {
                                { 1, paymentPeriod1 },
                                { 2, paymentPeriod2 }
                            }
                        }
                    }
                }
            };

            var expectedOutput = new List<EasFundingLine>
            {
                new EasFundingLine
                {
                    FundLine = "FundLine1",
                    EasSubmissionValues = new List<EasSubmissionValue>
                    {
                        new EasSubmissionValue
                        {
                            AdjustmentTypeName = "AdjustmentTypeName1",
                            PaymentName = "PaymentName1",
                            Period1 = paymentPeriod1,
                            Period2 = paymentPeriod2
                        },
                        new EasSubmissionValue
                        {
                            AdjustmentTypeName = "AdjustmentTypeName11",
                            PaymentName = "PaymentName11",
                            Period1 = paymentPeriod1,
                            Period2 = paymentPeriod2,
                            Period3 = paymentPeriod3,
                        }
                    }
                },
                new EasFundingLine
                {
                    FundLine = "FundLine2",
                    EasSubmissionValues = new List<EasSubmissionValue>
                    {
                        new EasSubmissionValue
                        {
                            AdjustmentTypeName = "AdjustmentTypeName2",
                            PaymentName = "PaymentName2",
                            Period1 = paymentPeriod1,
                            Period2 = paymentPeriod2,
                        },
                        new EasSubmissionValue
                        {
                            AdjustmentTypeName = "AdjustmentTypeName22",
                            PaymentName = "PaymentName22"
                        }
                    }
                }
            };

            NewService().MapEasValues(easFundingLines, easValuesDictionary).Should().BeEquivalentTo(expectedOutput);
        }

        [Fact]
        public void MapEasValues_NoEasValues()
        {
            var easFundingLines = new List<EasFundingLine>
            {
                new EasFundingLine
                {
                    FundLine = "FundLine1",
                    EasSubmissionValues = new List<EasSubmissionValue>
                    {
                        new EasSubmissionValue
                        {
                            AdjustmentTypeName = "AdjustmentTypeName1",
                            PaymentName = "PaymentName1",
                        },
                        new EasSubmissionValue
                        {
                            AdjustmentTypeName = "AdjustmentTypeName11",
                            PaymentName = "PaymentName11",
                        }
                    }
                },
                new EasFundingLine
                {
                    FundLine = "FundLine2",
                    EasSubmissionValues = new List<EasSubmissionValue>
                    {
                        new EasSubmissionValue
                        {
                            AdjustmentTypeName = "AdjustmentTypeName2",
                            PaymentName = "PaymentName2",
                        },
                        new EasSubmissionValue
                        {
                            AdjustmentTypeName = "AdjustmentTypeName22",
                            PaymentName = "PaymentName22",
                        }
                    }
                }
            };

            var easValuesDictionary = new Dictionary<string, Dictionary<string, Dictionary<int, List<EasPaymentValue>>>>();

            var expectedOutput = new List<EasFundingLine>
            {
                new EasFundingLine
                {
                    FundLine = "FundLine1",
                    EasSubmissionValues = new List<EasSubmissionValue>
                    {
                        new EasSubmissionValue
                        {
                            AdjustmentTypeName = "AdjustmentTypeName1",
                            PaymentName = "PaymentName1"
                        },
                        new EasSubmissionValue
                        {
                            AdjustmentTypeName = "AdjustmentTypeName11",
                            PaymentName = "PaymentName11"
                        }
                    }
                },
                new EasFundingLine
                {
                    FundLine = "FundLine2",
                    EasSubmissionValues = new List<EasSubmissionValue>
                    {
                        new EasSubmissionValue
                        {
                            AdjustmentTypeName = "AdjustmentTypeName2",
                            PaymentName = "PaymentName2"
                        },
                        new EasSubmissionValue
                        {
                            AdjustmentTypeName = "AdjustmentTypeName22",
                            PaymentName = "PaymentName22"
                        }
                    }
                }
            };

            NewService().MapEasValues(easFundingLines, easValuesDictionary).Should().BeEquivalentTo(expectedOutput);
        }

        [Fact]
        public async Task RetrieveAsync()
        {
            var paymentPeriod1 = new List<EasPaymentValue> { new EasPaymentValue(1.0m, 1) };
            var paymentPeriod11 = new List<EasPaymentValue> { new EasPaymentValue(11.0m, 11) };
            var paymentPeriod2 = new List<EasPaymentValue> { new EasPaymentValue(2.0m, 2), new EasPaymentValue(2.0m, 22) };

            var expectedOutput = new List<EasFundingLine>
            {
                new EasFundingLine
                {
                    FundLine = "FundLine1",
                    EasSubmissionValues = new List<EasSubmissionValue>
                    {
                        new EasSubmissionValue
                        {
                            AdjustmentTypeName = "AdjustmentTypeName1",
                            PaymentName = "PaymentName1",
                            Period1 = paymentPeriod1,
                        },
                        new EasSubmissionValue
                        {
                            AdjustmentTypeName = "AdjustmentTypeName11",
                            PaymentName = "PaymentName11",
                            Period11 = paymentPeriod11,
                        }
                    }
                },
                new EasFundingLine
                {
                    FundLine = "FundLine2",
                    EasSubmissionValues = new List<EasSubmissionValue>
                    {
                        new EasSubmissionValue
                        {
                            AdjustmentTypeName = "AdjustmentTypeName2",
                            PaymentName = "PaymentName2",
                            Period2 = paymentPeriod2,
                        },
                        new EasSubmissionValue
                        {
                            AdjustmentTypeName = "AdjustmentTypeName22",
                            PaymentName = "PaymentName22"
                        }
                    }
                }
            };

            var fundingLinesMock = new List<FundingLine>
            {
                new FundingLine
                {
                    Id = 1,
                    Name = "FundLine1",
                    PaymentTypes = new List<PaymentType>
                    {
                        new PaymentType
                        {
                            PaymentId = 1,
                            PaymentName = "PaymentName1",
                            AdjustmentType = new AdjustmentType
                            {
                                Id = 1,
                                Name = "AdjustmentTypeName1"
                            }
                        },
                        new PaymentType
                        {
                            PaymentId = 11,
                            PaymentName = "PaymentName11",
                            AdjustmentType = new AdjustmentType
                            {
                                Id = 11,
                                Name = "AdjustmentTypeName11"
                            }
                        }
                    },
                },
                new FundingLine
                {
                    Id = 2,
                    Name = "FundLine2",
                    PaymentTypes = new List<PaymentType>
                    {
                        new PaymentType
                        {
                            PaymentId = 2,
                            PaymentName = "PaymentName2",
                            AdjustmentType = new AdjustmentType
                            {
                                Id = 2,
                                Name = "AdjustmentTypeName2"
                            }
                        },
                        new PaymentType
                        {
                            PaymentId = 22,
                            PaymentName = "PaymentName22",
                            AdjustmentType = new AdjustmentType
                            {
                                Id = 22,
                                Name = "AdjustmentTypeName22"
                            }
                        }
                    },
                }
            }.AsQueryable().BuildMockDbSet();

            var easSubmissionsMock = new List<EasSubmission>
            {
                new EasSubmission
                {
                    Ukprn = "12345678",
                    SubmissionId = default(Guid),
                    EasSubmissionValues = new List<EasSubmissionValueEF>
                    {
                        new EasSubmissionValueEF
                        {
                            CollectionPeriod = 1,
                            PaymentId = 1,
                            PaymentValue = 1.0m,
                            Payment = new PaymentType
                            {
                                AdjustmentTypeId = 1,
                                AdjustmentType = new AdjustmentType
                                {
                                    Id = 1,
                                    Name = "AdjustmentTypeName1"
                                },
                                FundingLine = new FundingLine
                                {
                                    Name = "FundLine1",
                                    Id = 1
                                },
                                PaymentName = "PaymentName1",
                            },
                            DevolvedAreaSoF = 1
                        },
                        new EasSubmissionValueEF
                        {
                            CollectionPeriod = 11,
                            PaymentId = 11,
                            PaymentValue = 11.0m,
                            Payment = new PaymentType
                            {
                                AdjustmentTypeId = 11,
                                AdjustmentType = new AdjustmentType
                                {
                                    Id = 11,
                                    Name = "AdjustmentTypeName11"
                                },
                                FundingLine = new FundingLine
                                {
                                    Name = "FundLine1",
                                    Id = 11
                                },
                                PaymentName = "PaymentName11"
                            },
                            DevolvedAreaSoF = 11
                        },
                        new EasSubmissionValueEF
                        {
                            CollectionPeriod = 2,
                            PaymentId = 2,
                            PaymentValue = 2.0m,
                            Payment = new PaymentType
                            {
                                AdjustmentTypeId = 2,
                                AdjustmentType = new AdjustmentType
                                {
                                    Id = 2,
                                    Name = "AdjustmentTypeName2"
                                },
                                FundingLine = new FundingLine
                                {
                                    Name = "FundLine2",
                                    Id = 2
                                },
                                PaymentName = "PaymentName2"
                            },
                            DevolvedAreaSoF = 2
                        },
                        new EasSubmissionValueEF
                        {
                            CollectionPeriod = 2,
                            PaymentId = 2,
                            PaymentValue = 2.0m,
                            Payment = new PaymentType
                            {
                                AdjustmentTypeId = 2,
                                AdjustmentType = new AdjustmentType
                                {
                                    Id = 2,
                                    Name = "AdjustmentTypeName2"
                                },
                                FundingLine = new FundingLine
                                {
                                    Name = "FundLine2",
                                    Id = 2
                                },
                                PaymentName = "PaymentName2"
                            },
                            DevolvedAreaSoF = 22
                        }
                    }
                },
                new EasSubmission
                {
                    Ukprn = "999999",
                    SubmissionId = default(Guid),
                    EasSubmissionValues = new List<EasSubmissionValueEF>
                    {
                        new EasSubmissionValueEF
                        {
                            CollectionPeriod = 1,
                            PaymentId = 1,
                            PaymentValue = 1.0m,
                            Payment = new PaymentType
                            {
                                AdjustmentTypeId = 1,
                                AdjustmentType = new AdjustmentType
                                {
                                    Id = 1,
                                    Name = "AdjustmentTypeName1"
                                },
                                FundingLine = new FundingLine
                                {
                                    Name = "FundLine1",
                                    Id = 1
                                },
                                PaymentName = "PaymentName1"
                            }
                        },
                    }
                }
            }.AsQueryable().BuildMockDbSet();

            var easMock = new Mock<IEasdbContext>();

            easMock.Setup(e => e.FundingLines).Returns(fundingLinesMock.Object);
            easMock.Setup(e => e.EasSubmissions).Returns(easSubmissionsMock.Object);

            var easContextFactoryMock = new Mock<IDbContextFactory<IEasdbContext>>();
            easContextFactoryMock.Setup(c => c.Create()).Returns(easMock.Object);

            var easFundLines = await NewService(easContextFactoryMock.Object).RetrieveAsync(12345678, CancellationToken.None);

            var resultFundline1Period1 = easFundLines.ToArray()[0].EasSubmissionValues.First().Period1;
            var resultFundline1Period1Sofs = easFundLines.ToArray()[0].EasSubmissionValues.First().Period1;
            var expectedFundline1Period1 = expectedOutput.ToArray()[0].EasSubmissionValues.First().Period1;
            var expectedFundline1Period1Sofs = expectedOutput.ToArray()[0].EasSubmissionValues.First().Period1;

            var resultFundline1Period11 = easFundLines.ToArray()[0].EasSubmissionValues.ToArray()[1].Period11;
            var resultFundline1Period11Sofs = easFundLines.ToArray()[0].EasSubmissionValues.ToArray()[1].Period11;
            var expectedFundline1Period11 = expectedOutput.ToArray()[0].EasSubmissionValues.ToArray()[1].Period11;
            var expectedFundline1Period11Sofs = expectedOutput.ToArray()[0].EasSubmissionValues.ToArray()[1].Period11;

            var resultFundline2Period2 = easFundLines.ToArray()[1].EasSubmissionValues.First().Period2;
            var resultFundline2Period2Sofs = easFundLines.ToArray()[1].EasSubmissionValues.First().Period2;
            var expectedFundline2Period2 = expectedOutput.ToArray()[1].EasSubmissionValues.First().Period2;
            var expectedFundline2Period2Sofs = expectedOutput.ToArray()[1].EasSubmissionValues.First().Period2;

            easFundLines.ToArray()[0].FundLine.Should().Be("FundLine1");
            easFundLines.ToArray()[1].FundLine.Should().Be("FundLine2");
            easFundLines.ToArray()[0].EasSubmissionValues.Should().HaveCount(2);
            easFundLines.ToArray()[1].EasSubmissionValues.Should().HaveCount(2);
            resultFundline1Period1.Should().BeEquivalentTo(expectedFundline1Period1);
            expectedFundline1Period1Sofs.Should().BeEquivalentTo(expectedFundline1Period1Sofs);
            resultFundline1Period11.Should().BeEquivalentTo(expectedFundline1Period11);
            expectedFundline1Period11Sofs.Should().BeEquivalentTo(expectedFundline1Period11Sofs);
            resultFundline2Period2.Should().BeEquivalentTo(expectedFundline2Period2);
            expectedFundline2Period2Sofs.Should().BeEquivalentTo(expectedFundline2Period2Sofs);
        }

        private EasRepositoryService NewService(IDbContextFactory<IEasdbContext> easContextFactory = null)
        {
            return new EasRepositoryService(easContextFactory);
        }
    }
}
