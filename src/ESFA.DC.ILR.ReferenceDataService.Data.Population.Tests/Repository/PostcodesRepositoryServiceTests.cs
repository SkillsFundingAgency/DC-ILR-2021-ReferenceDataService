using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ReferenceData.Postcodes.Model;
using ESFA.DC.Serialization.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Repository
{
    public class PostcodesRepositoryServiceTests
    {
        [Fact]
        public async Task RetrieveAsync()
        {
            var cancellationToken = CancellationToken.None;
            IReadOnlyCollection<string> postcodes = new List<string> { "PostCode1", "PostCode2", "PostCode3", "PostCode4" };
            var json = @"[""PostCode1"",""PostCode2"",""PostCode3"",""PostCode4""]";

            var masterPostcodeTaskResult = new TaskCompletionSource<IEnumerable<MasterPostcode>>();
            var sfaAreaCostTaskResult = new TaskCompletionSource<IEnumerable<SfaPostcodeAreaCost>>();
            var sfaDisadvantageTaskResult = new TaskCompletionSource<IEnumerable<SfaPostcodeDisadvantage>>();
            var efaDisadvantageTaskResult = new TaskCompletionSource<IEnumerable<EfaPostcodeDisadvantage>>();
            var dasDisadvantageTaskResult = new TaskCompletionSource<IEnumerable<DasPostcodeDisadvantage>>();
            var careerLearningPilotsTaskResult = new TaskCompletionSource<IEnumerable<CareerLearningPilotPostcode>>();
            var onsDataTaskResult = new TaskCompletionSource<IEnumerable<OnsPostcode>>();
            var mcaglaSOFTaskResult = new TaskCompletionSource<IEnumerable<McaglaSof>>();

            var masterPostcodes = new List<MasterPostcode>
            {
                new MasterPostcode
                {
                    Postcode = "PostCode1",
                },
                new MasterPostcode
                {
                    Postcode = "PostCode2",
                },
                new MasterPostcode
                {
                    Postcode = "PostCode3",
                }
            };

            var sfaAreaCost = new List<SfaPostcodeAreaCost>
            {
                new SfaPostcodeAreaCost
                {
                    Postcode = "PostCode1",
                    AreaCostFactor = 1.2m,
                    EffectiveFrom = new DateTime(2018, 8, 1)
                },
                new SfaPostcodeAreaCost
                {
                    Postcode = "PostCode2",
                    AreaCostFactor = 1.2m,
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    EffectiveTo = new DateTime(2018, 9, 1)
                },
                new SfaPostcodeAreaCost
                {
                    Postcode = "PostCode2",
                    AreaCostFactor = 1.5m,
                    EffectiveFrom = new DateTime(2018, 9, 2)
                }
            };

            var sfaDisadvantage = new List<SfaPostcodeDisadvantage>
            {
                new SfaPostcodeDisadvantage
                {
                    Postcode = "PostCode1",
                    Uplift = 1.2m,
                    EffectiveFrom = new DateTime(2018, 8, 1)
                },
                new SfaPostcodeDisadvantage
                {
                    Postcode = "PostCode2",
                    Uplift = 1.2m,
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    EffectiveTo = new DateTime(2018, 9, 1)
                },
                new SfaPostcodeDisadvantage
                {
                    Postcode = "PostCode2",
                    Uplift = 1.5m,
                    EffectiveFrom = new DateTime(2018, 9, 2)
                }
            };

            var efaDisadvantage = new List<EfaPostcodeDisadvantage>
            {
                new EfaPostcodeDisadvantage
                {
                    Postcode = "PostCode1",
                    Uplift = 1.2m,
                    EffectiveFrom = new DateTime(2018, 8, 1)
                },
                new EfaPostcodeDisadvantage
                {
                    Postcode = "PostCode2",
                    Uplift = 1.2m,
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    EffectiveTo = new DateTime(2018, 9, 1)
                },
                new EfaPostcodeDisadvantage
                {
                    Postcode = "PostCode2",
                    Uplift = 1.5m,
                    EffectiveFrom = new DateTime(2018, 9, 2)
                }
            };

            var dasDisadvantage = new List<DasPostcodeDisadvantage>
            {
                new DasPostcodeDisadvantage
                {
                    Postcode = "PostCode1",
                    Uplift = 1.2m,
                    EffectiveFrom = new DateTime(2018, 8, 1)
                },
                new DasPostcodeDisadvantage
                {
                    Postcode = "PostCode2",
                    Uplift = 1.2m,
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    EffectiveTo = new DateTime(2018, 9, 1)
                },
                new DasPostcodeDisadvantage
                {
                    Postcode = "PostCode2",
                    Uplift = 1.5m,
                    EffectiveFrom = new DateTime(2018, 9, 2)
                }
            };

            var careerLearningPilotsPostcode = new List<CareerLearningPilotPostcode>
            {
                new CareerLearningPilotPostcode
                {
                    Postcode = "PostCode1",
                    AreaCode = "Code1",
                    EffectiveFrom = new DateTime(2018, 8, 1)
                },
                new CareerLearningPilotPostcode
                {
                    Postcode = "PostCode2",
                    AreaCode = "Code1",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    EffectiveTo = new DateTime(2018, 9, 1)
                },
                new CareerLearningPilotPostcode
                {
                    Postcode = "PostCode2",
                    AreaCode = "Code2",
                    EffectiveFrom = new DateTime(2018, 9, 2)
                }
            };

            var onsPostcodes = new List<OnsPostcode>
            {
                new OnsPostcode
                {
                    Postcode = "PostCode1",
                    Lep1 = "Lep1",
                    LocalAuthority = "LocalAuthority",
                    EffectiveFrom = new DateTime(2018, 8, 1)
                },
                new OnsPostcode
                {
                    Postcode = "PostCode2",
                    Lep1 = "Lep1",
                    LocalAuthority = "LocalAuthority",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    EffectiveTo = new DateTime(2018, 9, 1)
                },
                new OnsPostcode
                {
                    Postcode = "PostCode2",
                    Lep1 = "Lep11",
                    LocalAuthority = "LocalAuthority",
                    EffectiveFrom = new DateTime(2018, 9, 2)
                }
            };

            var mcaglaSofPostCodes = new List<McaglaSof> //<McaglaSOFpostCode>
            {
                new McaglaSof
                {
                    Postcode = "PostCode1",
                    SofCode = "SofCode1",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                },
                new McaglaSof
                {
                    Postcode = "PostCode2",
                    SofCode = "SofCode1",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    EffectiveTo = new DateTime(2018, 9, 1)
                },
                new McaglaSof
                 {
                     Postcode = "PostCode2",
                     SofCode = "SofCode2",
                     EffectiveFrom = new DateTime(2018, 8, 1),
                 },
            };

            masterPostcodeTaskResult.SetResult(masterPostcodes);
            sfaAreaCostTaskResult.SetResult(sfaAreaCost);
            sfaDisadvantageTaskResult.SetResult(sfaDisadvantage);
            efaDisadvantageTaskResult.SetResult(efaDisadvantage);
            dasDisadvantageTaskResult.SetResult(dasDisadvantage);
            careerLearningPilotsTaskResult.SetResult(careerLearningPilotsPostcode);
            onsDataTaskResult.SetResult(onsPostcodes);
            mcaglaSOFTaskResult.SetResult(mcaglaSofPostCodes);

            var jsonSerializationMock = new Mock<IJsonSerializationService>();

            jsonSerializationMock.Setup(sm => sm.Serialize(postcodes)).Returns(json);

            var service = NewServiceMock(jsonSerializationService: jsonSerializationMock.Object);

            service.Setup(s => s.RetrieveAsync<MasterPostcode>(json, It.IsAny<string>(), cancellationToken)).Returns(masterPostcodeTaskResult.Task);
            service.Setup(s => s.RetrieveAsync<SfaPostcodeAreaCost>(json, It.IsAny<string>(), cancellationToken)).Returns(sfaAreaCostTaskResult.Task);
            service.Setup(s => s.RetrieveAsync<SfaPostcodeDisadvantage>(json, It.IsAny<string>(), cancellationToken)).Returns(sfaDisadvantageTaskResult.Task);
            service.Setup(s => s.RetrieveAsync<EfaPostcodeDisadvantage>(json, It.IsAny<string>(), cancellationToken)).Returns(efaDisadvantageTaskResult.Task);
            service.Setup(s => s.RetrieveAsync<DasPostcodeDisadvantage>(json, It.IsAny<string>(), cancellationToken)).Returns(dasDisadvantageTaskResult.Task);
            service.Setup(s => s.RetrieveAsync<CareerLearningPilotPostcode>(json, It.IsAny<string>(), cancellationToken)).Returns(careerLearningPilotsTaskResult.Task);
            service.Setup(s => s.RetrieveAsync<OnsPostcode>(json, It.IsAny<string>(), cancellationToken)).Returns(onsDataTaskResult.Task);
            service.Setup(s => s.RetrieveAsync<McaglaSof>(json, It.IsAny<string>(), cancellationToken)).Returns(mcaglaSOFTaskResult.Task);

            var serviceResult = await service.Object.RetrieveAsync(postcodes, cancellationToken);

            var x = 3;

            IReadOnlyCollection<Postcode> expectedResult = new List<Postcode>
            {
                new Postcode
                {
                    PostCode = "PostCode1",
                    SfaAreaCosts = new List<SfaAreaCost>
                    {
                        new SfaAreaCost
                        {
                            AreaCostFactor = 1.2m,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    },
                    SfaDisadvantages = new List<SfaDisadvantage>
                    {
                        new SfaDisadvantage
                        {
                            Uplift = 1.2m,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    },
                    EfaDisadvantages = new List<EfaDisadvantage>
                    {
                        new EfaDisadvantage
                        {
                            Uplift = 1.2m,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    },
                    DasDisadvantages = new List<DasDisadvantage>
                    {
                        new DasDisadvantage
                        {
                            Uplift = 1.2m,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    },
                    CareerLearningPilots = new List<CareerLearningPilot>
                    {
                        new CareerLearningPilot
                        {
                            AreaCode = "Code1",
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    },
                    ONSData = new List<ONSData>
                    {
                        new ONSData
                        {
                            Lep1 = "Lep1",
                            LocalAuthority = "LocalAuthority",
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    },
                    McaglaSOFs = new List<McaglaSOF>
                    {
                        new McaglaSOF
                        {
                            SofCode = "SofCode1",
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    }
                },
                new Postcode
                {
                    PostCode = "PostCode2",
                    SfaAreaCosts = new List<SfaAreaCost>
                    {
                        new SfaAreaCost
                        {
                            AreaCostFactor = 1.2m,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = new DateTime(2018, 9, 1)
                        },
                        new SfaAreaCost
                        {
                            AreaCostFactor = 1.5m,
                            EffectiveFrom = new DateTime(2018, 9, 2)
                        }
                    },
                    SfaDisadvantages = new List<SfaDisadvantage>
                    {
                        new SfaDisadvantage
                        {
                            Uplift = 1.2m,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = new DateTime(2018, 9, 1)
                        },
                        new SfaDisadvantage
                        {
                            Uplift = 1.5m,
                            EffectiveFrom = new DateTime(2018, 9, 2)
                        }
                    },
                    EfaDisadvantages = new List<EfaDisadvantage>
                    {
                        new EfaDisadvantage
                        {
                            Uplift = 1.2m,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = new DateTime(2018, 9, 1)
                        },
                        new EfaDisadvantage
                        {
                            Uplift = 1.5m,
                            EffectiveFrom = new DateTime(2018, 9, 2)
                        }
                    },
                    DasDisadvantages = new List<DasDisadvantage>
                    {
                        new DasDisadvantage
                        {
                            Uplift = 1.2m,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = new DateTime(2018, 9, 1)
                        },
                        new DasDisadvantage
                        {
                            Uplift = 1.5m,
                            EffectiveFrom = new DateTime(2018, 9, 2)
                        }
                    },
                    CareerLearningPilots = new List<CareerLearningPilot>
                    {
                        new CareerLearningPilot
                        {
                            AreaCode = "Code1",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = new DateTime(2018, 9, 1)
                        },
                        new CareerLearningPilot
                        {
                            AreaCode = "Code2",
                            EffectiveFrom = new DateTime(2018, 9, 2)
                        }
                    },
                    ONSData = new List<ONSData>
                    {
                        new ONSData
                        {
                            Lep1 = "Lep1",
                            LocalAuthority = "LocalAuthority",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = new DateTime(2018, 9, 1)
                        },
                        new ONSData
                        {
                            Lep1 = "Lep11",
                            LocalAuthority = "LocalAuthority",
                            EffectiveFrom = new DateTime(2018, 9, 2)
                        }
                    },
                    McaglaSOFs = new List<McaglaSOF>
                    {
                        new McaglaSOF
                        {
                            SofCode = "SofCode1",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = new DateTime(2018, 9, 1)
                        },
                        new McaglaSOF
                        {
                            SofCode = "SofCode2",
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    }
                },
                new Postcode
                {
                    PostCode = "PostCode3"
                }
            };

            serviceResult.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task RetrieveMasterPostcodes()
        {
            var cancellationToken = CancellationToken.None;
            var json = @"[""Postcode1"",""Postcode2"",""Postcode3""]";

            var taskResult = new TaskCompletionSource<IEnumerable<MasterPostcode>>();

            var masterPostCodes = new List<MasterPostcode>
            {
                new MasterPostcode
                {
                    Postcode = "PostCode1",
                },
                new MasterPostcode
                {
                    Postcode = "PostCode2",
                },
                new MasterPostcode
                {
                    Postcode = "PostCode3",
                }
            };

            taskResult.SetResult(masterPostCodes);

            var masterPostCodeList = new List<string>
            {
                "PostCode1",
                "PostCode2",
                "PostCode3"
            };

            var service = NewServiceMock();

            service.Setup(s => s.RetrieveAsync<MasterPostcode>(json, It.IsAny<string>(), cancellationToken)).Returns(taskResult.Task);

            var result = await service.Object.RetrieveMasterPostcodes(json, cancellationToken);

            result.Should().BeEquivalentTo(masterPostCodeList);
        }

        [Fact]
        public async Task RetrieveSfaAreaCosts()
        {
            var cancellationToken = CancellationToken.None;
            var json = @"[""Postcode1"",""Postcode2"",""Postcode3""]";

            var taskResult = new TaskCompletionSource<IEnumerable<SfaPostcodeAreaCost>>();

            var sfaAreaCost = new List<SfaPostcodeAreaCost>
            {
                new SfaPostcodeAreaCost
                {
                    Postcode = "PostCode1",
                    AreaCostFactor = 1.2m,
                    EffectiveFrom = new DateTime(2018, 8, 1)
                },
                new SfaPostcodeAreaCost
                {
                    Postcode = "PostCode2",
                    AreaCostFactor = 1.2m,
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    EffectiveTo = new DateTime(2018, 9, 1)
                },
                new SfaPostcodeAreaCost
                {
                    Postcode = "PostCode2",
                    AreaCostFactor = 1.5m,
                    EffectiveFrom = new DateTime(2018, 9, 2)
                }
            };

            taskResult.SetResult(sfaAreaCost);

            var sfaAreaCostDictionary = new Dictionary<string, List<SfaAreaCost>>
            {
                {
                    "PostCode1", new List<SfaAreaCost>
                    {
                        new SfaAreaCost
                        {
                            AreaCostFactor = 1.2m,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    }
                },
                {
                    "PostCode2", new List<SfaAreaCost>
                    {
                        new SfaAreaCost
                        {
                            AreaCostFactor = 1.2m,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = new DateTime(2018, 9, 1)
                        },
                        new SfaAreaCost
                        {
                            AreaCostFactor = 1.5m,
                            EffectiveFrom = new DateTime(2018, 9, 2)
                        }
                    }
                }
            };

            var service = NewServiceMock();

            service.Setup(s => s.RetrieveAsync<SfaPostcodeAreaCost>(json, It.IsAny<string>(), cancellationToken)).Returns(taskResult.Task);

            var result = await service.Object.RetrieveSfaAreaCosts(json, cancellationToken);

            result.Should().BeEquivalentTo(sfaAreaCostDictionary);
        }

        [Fact]
        public async Task RetrieveSfaPostcodeDisadvantages()
        {
            var cancellationToken = CancellationToken.None;
            var json = @"[""Postcode1"",""Postcode2"",""Postcode3""]";

            var taskResult = new TaskCompletionSource<IEnumerable<SfaPostcodeDisadvantage>>();

            var sfaDisadvantage = new List<SfaPostcodeDisadvantage>
            {
                new SfaPostcodeDisadvantage
                {
                    Postcode = "PostCode1",
                    Uplift = 1.2m,
                    EffectiveFrom = new DateTime(2018, 8, 1)
                },
                new SfaPostcodeDisadvantage
                {
                    Postcode = "PostCode2",
                    Uplift = 1.2m,
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    EffectiveTo = new DateTime(2018, 9, 1)
                },
                new SfaPostcodeDisadvantage
                {
                    Postcode = "PostCode2",
                    Uplift = 1.5m,
                    EffectiveFrom = new DateTime(2018, 9, 2)
                }
            };

            taskResult.SetResult(sfaDisadvantage);

            var sfaDisadvantageDictionary = new Dictionary<string, List<SfaDisadvantage>>
            {
                {
                    "PostCode1", new List<SfaDisadvantage>
                    {
                        new SfaDisadvantage
                        {
                            Uplift = 1.2m,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    }
                },
                {
                    "PostCode2", new List<SfaDisadvantage>
                    {
                        new SfaDisadvantage
                        {
                            Uplift = 1.2m,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = new DateTime(2018, 9, 1)
                        },
                        new SfaDisadvantage
                        {
                            Uplift = 1.5m,
                            EffectiveFrom = new DateTime(2018, 9, 2)
                        }
                    }
                }
            };

            var service = NewServiceMock();

            service.Setup(s => s.RetrieveAsync<SfaPostcodeDisadvantage>(json, It.IsAny<string>(), cancellationToken)).Returns(taskResult.Task);

            var result = await service.Object.RetrieveSfaPostcodeDisadvantages(json, cancellationToken);

            result.Should().BeEquivalentTo(sfaDisadvantageDictionary);
        }

        [Fact]
        public async Task RetrieveEfaPostcodeDisadvantages()
        {
            var cancellationToken = CancellationToken.None;
            var json = @"[""Postcode1"",""Postcode2"",""Postcode3""]";

            var taskResult = new TaskCompletionSource<IEnumerable<EfaPostcodeDisadvantage>>();

            var efaPostcodeDisadvantage = new List<EfaPostcodeDisadvantage>
            {
                new EfaPostcodeDisadvantage
                {
                    Postcode = "PostCode1",
                    Uplift = 1.2m,
                    EffectiveFrom = new DateTime(2018, 8, 1)
                },
                new EfaPostcodeDisadvantage
                {
                    Postcode = "PostCode2",
                    Uplift = 1.2m,
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    EffectiveTo = new DateTime(2018, 9, 1)
                },
                new EfaPostcodeDisadvantage
                {
                    Postcode = "PostCode2",
                    Uplift = 1.5m,
                    EffectiveFrom = new DateTime(2018, 9, 2)
                }
            };

            taskResult.SetResult(efaPostcodeDisadvantage);

            var efaDisadvantageDictionary = new Dictionary<string, List<EfaDisadvantage>>
            {
                {
                    "PostCode1", new List<EfaDisadvantage>
                    {
                        new EfaDisadvantage
                        {
                            Uplift = 1.2m,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    }
                },
                {
                    "PostCode2", new List<EfaDisadvantage>
                    {
                        new EfaDisadvantage
                        {
                            Uplift = 1.2m,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = new DateTime(2018, 9, 1)
                        },
                        new EfaDisadvantage
                        {
                            Uplift = 1.5m,
                            EffectiveFrom = new DateTime(2018, 9, 2)
                        }
                    }
                }
            };

            var service = NewServiceMock();

            service.Setup(s => s.RetrieveAsync<EfaPostcodeDisadvantage>(json, It.IsAny<string>(), cancellationToken)).Returns(taskResult.Task);

            var result = await service.Object.RetrieveEfaPostcodeDisadvantages(json, cancellationToken);

            result.Should().BeEquivalentTo(efaDisadvantageDictionary);
        }

        [Fact]
        public async Task RetrieveDasPostcodeDisadvantages()
        {
            var cancellationToken = CancellationToken.None;
            var json = @"[""Postcode1"",""Postcode2"",""Postcode3""]";

            var taskResult = new TaskCompletionSource<IEnumerable<DasPostcodeDisadvantage>>();

            var dasPostcodeDisadvantage = new List<DasPostcodeDisadvantage>
            {
                new DasPostcodeDisadvantage
                {
                    Postcode = "PostCode1",
                    Uplift = 1.2m,
                    EffectiveFrom = new DateTime(2018, 8, 1)
                },
                new DasPostcodeDisadvantage
                {
                    Postcode = "PostCode2",
                    Uplift = 1.2m,
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    EffectiveTo = new DateTime(2018, 9, 1)
                },
                new DasPostcodeDisadvantage
                {
                    Postcode = "PostCode2",
                    Uplift = 1.5m,
                    EffectiveFrom = new DateTime(2018, 9, 2)
                }
            };

            taskResult.SetResult(dasPostcodeDisadvantage);

            var dasDisadvantageDictionary = new Dictionary<string, List<DasDisadvantage>>
            {
                {
                    "PostCode1", new List<DasDisadvantage>
                    {
                        new DasDisadvantage
                        {
                            Uplift = 1.2m,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    }
                },
                {
                    "PostCode2", new List<DasDisadvantage>
                    {
                        new DasDisadvantage
                        {
                            Uplift = 1.2m,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = new DateTime(2018, 9, 1)
                        },
                        new DasDisadvantage
                        {
                            Uplift = 1.5m,
                            EffectiveFrom = new DateTime(2018, 9, 2)
                        }
                    }
                }
            };

            var service = NewServiceMock();

            service.Setup(s => s.RetrieveAsync<DasPostcodeDisadvantage>(json, It.IsAny<string>(), cancellationToken)).Returns(taskResult.Task);

            var result = await service.Object.RetrieveDasPostcodeDisadvantages(json, cancellationToken);

            result.Should().BeEquivalentTo(dasDisadvantageDictionary);
        }

        [Fact]
        public async Task RetrieveCareerLearningPilots()
        {
            var cancellationToken = CancellationToken.None;
            var json = @"[""Postcode1"",""Postcode2"",""Postcode3""]";

            var taskResult = new TaskCompletionSource<IEnumerable<CareerLearningPilotPostcode>>();

            var careerLearningPilotsPostcode = new List<CareerLearningPilotPostcode>
            {
                new CareerLearningPilotPostcode
                {
                    Postcode = "PostCode1",
                    AreaCode = "Code1",
                    EffectiveFrom = new DateTime(2018, 8, 1)
                },
                new CareerLearningPilotPostcode
                {
                    Postcode = "PostCode2",
                    AreaCode = "Code1",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    EffectiveTo = new DateTime(2018, 9, 1)
                },
                new CareerLearningPilotPostcode
                {
                    Postcode = "PostCode2",
                    AreaCode = "Code2",
                    EffectiveFrom = new DateTime(2018, 9, 2)
                }
            };

            taskResult.SetResult(careerLearningPilotsPostcode);

            var careerLearningPilotsDictionary = new Dictionary<string, List<CareerLearningPilot>>
            {
                {
                    "PostCode1", new List<CareerLearningPilot>
                    {
                        new CareerLearningPilot
                        {
                            AreaCode = "Code1",
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    }
                },
                {
                    "PostCode2", new List<CareerLearningPilot>
                    {
                        new CareerLearningPilot
                        {
                            AreaCode = "Code1",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = new DateTime(2018, 9, 1)
                        },
                        new CareerLearningPilot
                        {
                            AreaCode = "Code2",
                            EffectiveFrom = new DateTime(2018, 9, 2)
                        }
                    }
                }
            };

            var service = NewServiceMock();

            service.Setup(s => s.RetrieveAsync<CareerLearningPilotPostcode>(json, It.IsAny<string>(), cancellationToken)).Returns(taskResult.Task);

            var result = await service.Object.RetrieveCareerLearningPilots(json, cancellationToken);

            result.Should().BeEquivalentTo(careerLearningPilotsDictionary);
        }

        [Fact]
        public async Task RetrieveOnsData()
        {
            var cancellationToken = CancellationToken.None;
            var json = @"[""Postcode1"",""Postcode2"",""Postcode3""]";

            var taskResult = new TaskCompletionSource<IEnumerable<OnsPostcode>>();

            var onsPostcodes = new List<OnsPostcode>
            {
                new OnsPostcode
                {
                    Postcode = "PostCode1",
                    Lep1 = "Lep1",
                    LocalAuthority = "LocalAuthority",
                    EffectiveFrom = new DateTime(2018, 8, 1)
                },
                new OnsPostcode
                {
                    Postcode = "PostCode2",
                    Lep1 = "Lep1",
                    LocalAuthority = "LocalAuthority",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    EffectiveTo = new DateTime(2018, 9, 1)
                },
                new OnsPostcode
                {
                    Postcode = "PostCode2",
                    Lep1 = "Lep11",
                    LocalAuthority = "LocalAuthority",
                    EffectiveFrom = new DateTime(2018, 9, 2)
                }
            };

            taskResult.SetResult(onsPostcodes);

            var onsDictionary = new Dictionary<string, List<ONSData>>
            {
                {
                    "PostCode1", new List<ONSData>
                    {
                        new ONSData
                        {
                            Lep1 = "Lep1",
                            LocalAuthority = "LocalAuthority",
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    }
                },
                {
                    "PostCode2", new List<ONSData>
                    {
                        new ONSData
                        {
                            Lep1 = "Lep1",
                            LocalAuthority = "LocalAuthority",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = new DateTime(2018, 9, 1)
                        },
                        new ONSData
                        {
                            Lep1 = "Lep11",
                            LocalAuthority = "LocalAuthority",
                            EffectiveFrom = new DateTime(2018, 9, 2)
                        }
                    }
                }
            };

            var service = NewServiceMock();

            service.Setup(s => s.RetrieveAsync<OnsPostcode>(json, It.IsAny<string>(), cancellationToken)).Returns(taskResult.Task);

            var result = await service.Object.RetrieveOnsData(json, cancellationToken);

            result.Should().BeEquivalentTo(onsDictionary);
        }

        [Fact]
        public async Task RetrieveMcaglaSOFData()
        {
            var cancellationToken = CancellationToken.None;
            var json = @"[""Postcode1"",""Postcode2"",""Postcode3""]";

            var taskResult = new TaskCompletionSource<IEnumerable<McaglaSof>>();

            var mcaglaPostcoeSOF = new List<McaglaSof>
            {
               new McaglaSof
                {
                    Postcode = "PostCode1",
                    SofCode = "SofCode1",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                },
               new McaglaSof
                {
                    Postcode = "PostCode2",
                    SofCode = "SofCode1",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    EffectiveTo = new DateTime(2018, 9, 1)
                },
               new McaglaSof
                 {
                     Postcode = "PostCode2",
                     SofCode = "SofCode2",
                     EffectiveFrom = new DateTime(2018, 8, 1),
                 }
            };

            taskResult.SetResult(mcaglaPostcoeSOF);

            var expectedResult = new Dictionary<string, List<McaglaSOF>>
            {
                 {
                    "PostCode1", new List<McaglaSOF>
                    {
                        new McaglaSOF
                        {
                             SofCode = "SofCode1",
                             EffectiveFrom = new DateTime(2018, 8, 1),
                        }
                    }
                 },
                 {
                    "PostCode2", new List<McaglaSOF>
                    {
                        new McaglaSOF
                        {
                            SofCode = "SofCode1",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = new DateTime(2018, 9, 1)
                        },
                        new McaglaSOF
                        {
                            SofCode = "SofCode2",
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    }
                 }
            };

            var service = NewServiceMock();
            service.Setup(s => s.RetrieveAsync<McaglaSof>(json, It.IsAny<string>(), cancellationToken)).Returns(taskResult.Task);
            var result = await service.Object.RetrieveMcaglaSOFData(json, cancellationToken);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void SfaPostcodeDisadvantagesToEntity()
        {
            var sfaPostcodeDisadvantage = new SfaPostcodeDisadvantage
            {
                Uplift = 1.0m,
                EffectiveFrom = new DateTime(2018, 8, 1),
                EffectiveTo = new DateTime(2018, 8, 31)
            };

            var sfaDisdvantage = new SfaDisadvantage
            {
                Uplift = 1.0m,
                EffectiveFrom = new DateTime(2018, 8, 1),
                EffectiveTo = new DateTime(2018, 8, 31)
            };

            NewService().SfaPostcodeDisadvantagesToEntity(sfaPostcodeDisadvantage).Should().BeEquivalentTo(sfaDisdvantage);
        }

        [Fact]
        public void EfaPostcodeDisadvantagesToEntity()
        {
            var efaPostcodeDisadvantage = new EfaPostcodeDisadvantage
            {
                Uplift = 1.0m,
                EffectiveFrom = new DateTime(2018, 8, 1),
                EffectiveTo = new DateTime(2018, 8, 31)
            };

            var efaDisdvantage = new EfaDisadvantage
            {
                Uplift = 1.0m,
                EffectiveFrom = new DateTime(2018, 8, 1),
                EffectiveTo = new DateTime(2018, 8, 31)
            };

            NewService().EfaPostcodeDisadvantagesToEntity(efaPostcodeDisadvantage).Should().BeEquivalentTo(efaDisdvantage);
        }

        [Fact]
        public void DasPostcodeDisadvantagesToEntity()
        {
            var dasPostcodeDisadvantage = new DasPostcodeDisadvantage
            {
                Uplift = 1.0m,
                EffectiveFrom = new DateTime(2018, 8, 1),
                EffectiveTo = new DateTime(2018, 8, 31)
            };

            var dasDisdvantage = new DasDisadvantage
            {
                Uplift = 1.0m,
                EffectiveFrom = new DateTime(2018, 8, 1),
                EffectiveTo = new DateTime(2018, 8, 31)
            };

            NewService().DasPostcodeDisadvantagesToEntity(dasPostcodeDisadvantage).Should().BeEquivalentTo(dasDisdvantage);
        }

        [Fact]
        public void SfaPostcodeAreaCostsToEntity()
        {
            var sfaPostcodeAreaCost = new SfaPostcodeAreaCost
            {
                AreaCostFactor = 1.0m,
                EffectiveFrom = new DateTime(2018, 8, 1),
                EffectiveTo = new DateTime(2018, 8, 31)
            };

            var sfaAreaCost = new SfaAreaCost
            {
                AreaCostFactor = 1.0m,
                EffectiveFrom = new DateTime(2018, 8, 1),
                EffectiveTo = new DateTime(2018, 8, 31)
            };

            NewService().SfaAreaCostsToEntity(sfaPostcodeAreaCost).Should().BeEquivalentTo(sfaAreaCost);
        }

        [Fact]
        public void CareerLearningPilotsToEntity()
        {
            var careerLearningPilotPostcode = new CareerLearningPilotPostcode
            {
                AreaCode = "Area1",
                EffectiveFrom = new DateTime(2018, 8, 1),
                EffectiveTo = new DateTime(2018, 8, 31)
            };

            var careerLearningPilot = new CareerLearningPilot
            {
                AreaCode = "Area1",
                EffectiveFrom = new DateTime(2018, 8, 1),
                EffectiveTo = new DateTime(2018, 8, 31)
            };

            NewService().CareerLearningPilotsToEntity(careerLearningPilotPostcode).Should().BeEquivalentTo(careerLearningPilot);
        }

        [Fact]
        public void ONSDataToEntity()
        {
            var onsPostcode = new OnsPostcode
            {
                LocalAuthority = "Authority",
                Lep1 = "Lep1",
                Lep2 = "Lep2",
                Nuts = "Nuts",
                Termination = "202008",
                EffectiveFrom = new DateTime(2018, 8, 1)
            };

            var onsData = new ONSData
            {
                LocalAuthority = "Authority",
                Lep1 = "Lep1",
                Lep2 = "Lep2",
                Nuts = "Nuts",
                Termination = new DateTime(2020, 8, 31),
                EffectiveFrom = new DateTime(2018, 8, 1)
            };

            NewService().ONSDataToEntity(onsPostcode).Should().BeEquivalentTo(onsData);
        }

        [Fact]
        public void McaglaSofToEntity()
        {
            var mcaglaSof = new McaglaSof
            {
                SofCode = "SofCode1",
                EffectiveFrom = new DateTime(2018, 8, 1)
            };

            var expectedMcaglasSOF = new McaglaSOF
            {
                SofCode = "SofCode1",
                EffectiveFrom = new DateTime(2018, 8, 1)
            };

            NewService().McaglaSofToEntity(mcaglaSof).Should().BeEquivalentTo(expectedMcaglasSOF);
        }

        [Fact]
        public void McaglaSofToEntity_Should_BeFalse()
        {
            var mcaglaSof = new McaglaSof
            {
                SofCode = "SofCode2",
                EffectiveFrom = new DateTime(2018, 8, 1)
            };

            var expectedMcaglasSOF = new McaglaSOF
            {
                SofCode = "SofCode1",
                EffectiveFrom = new DateTime(2018, 8, 1)
            };

            NewService().McaglaSofToEntity(mcaglaSof).Should().NotBeSameAs(expectedMcaglasSOF);
        }

        private PostcodesRepositoryService NewService(IReferenceDataOptions referenceDataOptions = null, IJsonSerializationService jsonSerializationService = null)
        {
            return new PostcodesRepositoryService(referenceDataOptions, jsonSerializationService);
        }

        private Mock<PostcodesRepositoryService> NewServiceMock(IReferenceDataOptions referenceDataOptions = null, IJsonSerializationService jsonSerializationService = null)
        {
            return new Mock<PostcodesRepositoryService>(referenceDataOptions, jsonSerializationService);
        }
    }
}
