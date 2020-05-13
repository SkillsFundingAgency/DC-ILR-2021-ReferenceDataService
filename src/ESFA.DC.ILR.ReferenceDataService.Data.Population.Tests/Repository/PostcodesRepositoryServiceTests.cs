using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Entity;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ReferenceData.Postcodes.Model;
using ESFA.DC.ReferenceData.Postcodes.Model.Interface;
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

            masterPostcodeTaskResult.SetResult(masterPostcodes);
            sfaAreaCostTaskResult.SetResult(sfaAreaCost);
            sfaDisadvantageTaskResult.SetResult(sfaDisadvantage);
            efaDisadvantageTaskResult.SetResult(efaDisadvantage);
            dasDisadvantageTaskResult.SetResult(dasDisadvantage);
            onsDataTaskResult.SetResult(onsPostcodes);

            var jsonSerializationMock = new Mock<IJsonSerializationService>();

            jsonSerializationMock.Setup(sm => sm.Serialize(postcodes)).Returns(json);

            var postcodesContextMock = new Mock<IPostcodesContext>();
            var postcodesContextFactoryMock = new Mock<IDbContextFactory<IPostcodesContext>>();
            postcodesContextFactoryMock.Setup(c => c.Create()).Returns(postcodesContextMock.Object);

            var service = NewServiceMock(postcodesContextFactory: postcodesContextFactoryMock.Object, jsonSerializationService: jsonSerializationMock.Object);

            service.Setup(s => s.RetrieveAsync<MasterPostcode>(json, It.IsAny<string>(), cancellationToken)).Returns(masterPostcodeTaskResult.Task);
            service.Setup(s => s.RetrieveAsync<SfaPostcodeAreaCost>(json, It.IsAny<string>(), cancellationToken)).Returns(sfaAreaCostTaskResult.Task);
            service.Setup(s => s.RetrieveAsync<SfaPostcodeDisadvantage>(json, It.IsAny<string>(), cancellationToken)).Returns(sfaDisadvantageTaskResult.Task);
            service.Setup(s => s.RetrieveAsync<EfaPostcodeDisadvantage>(json, It.IsAny<string>(), cancellationToken)).Returns(efaDisadvantageTaskResult.Task);
            service.Setup(s => s.RetrieveAsync<DasPostcodeDisadvantage>(json, It.IsAny<string>(), cancellationToken)).Returns(dasDisadvantageTaskResult.Task);
            service.Setup(s => s.RetrieveAsync<OnsPostcode>(json, It.IsAny<string>(), cancellationToken)).Returns(onsDataTaskResult.Task);
            //service.Setup(s => s.RetrieveAsync<PostcodeSpecialistResource>(json, It.IsAny<string>(), cancellationToken)).Returns(specialistResources.Task);

            var serviceResult = await service.Object.RetrieveAsync(postcodes, cancellationToken);

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
                    ONSData = new List<ONSData>
                    {
                        new ONSData
                        {
                            Lep1 = "Lep1",
                            LocalAuthority = "LocalAuthority",
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    },
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

        private Mock<PostcodesRepositoryService> NewServiceMock(
             IDbContextFactory<IPostcodesContext> postcodesContextFactory = null,
             IReferenceDataOptions referenceDataOptions = null,
             IJsonSerializationService jsonSerializationService = null,
             IPostcodesEntityModelMapper postcodesEntityModelMapper = null)
        {
            return new Mock<PostcodesRepositoryService>(postcodesContextFactory, referenceDataOptions, jsonSerializationService, postcodesEntityModelMapper ?? new PostcodesEntityModelMapper());
        }
    }
}
