using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.EAS1920.EF;
using ESFA.DC.EAS1920.EF.Interface;
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
            var paymentPeriod1 = new EasPaymentValue(1.0m, null);
            var paymentPeriod2 = new EasPaymentValue(2.0m, null);
            var paymentPeriod3 = new EasPaymentValue(3.0m, null);

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

            var easValuesDictionary = new Dictionary<string, Dictionary<string, Dictionary<int, EasPaymentValue>>>
            {
                {
                    "FundLine1", new Dictionary<string, Dictionary<int, EasPaymentValue>>
                    {
                        {
                            "PaymentName1", new Dictionary<int, EasPaymentValue>
                            {
                                { 1, paymentPeriod1 },
                                { 2, paymentPeriod2 }
                            }
                        },
                        {
                            "PaymentName11", new Dictionary<int, EasPaymentValue>
                            {
                                { 1, paymentPeriod1 },
                                { 2, paymentPeriod2 },
                                { 3, paymentPeriod3 }
                            }
                        }
                    }
                },
                {
                    "FundLine2", new Dictionary<string, Dictionary<int, EasPaymentValue>>
                    {
                        {
                            "PaymentName2", new Dictionary<int, EasPaymentValue>
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

            var easValuesDictionary = new Dictionary<string, Dictionary<string, Dictionary<int, EasPaymentValue>>>();

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
            var paymentPeriod1 = new EasPaymentValue(1.0m, null);
            var paymentPeriod11 = new EasPaymentValue(11.0m, null);
            var paymentPeriod2 = new EasPaymentValue(2.0m, null);

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

            var easMock = new Mock<IEasdbContext>();

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
                                PaymentName = "PaymentName1"
                            }
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
                            }
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
                            }
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

            easMock.Setup(e => e.FundingLines).Returns(fundingLinesMock.Object);
            easMock.Setup(e => e.EasSubmissions).Returns(easSubmissionsMock.Object);

            var easFundLines = await NewService(easMock.Object).RetrieveAsync(12345678, CancellationToken.None);

            easFundLines.Should().BeEquivalentTo(expectedOutput);
        }

        private EasRepositoryService NewService(IEasdbContext easContext = null)
        {
            return new EasRepositoryService(easContext);
        }
    }
}
