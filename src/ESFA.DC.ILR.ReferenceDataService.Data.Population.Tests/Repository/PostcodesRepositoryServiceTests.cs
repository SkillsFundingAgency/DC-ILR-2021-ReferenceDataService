using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ReferenceData.Postcodes.Model;
using ESFA.DC.ReferenceData.Postcodes.Model.Interface;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Repository
{
    public class PostcodesRepositoryServiceTests
    {
        [Fact]
        public async Task RetrieveAsync()
        {
            var postcodes = new List<string> { "Postcode1", "Postcode2", "Postcode3", "Postcode4", "Postcode5", "Postcode6" };

            var postcodesMock = new Mock<IPostcodesContext>();

            var sfaDisad = new List<SfaDisadvantage>
            {
                new SfaDisadvantage
                {
                    Uplift = 1.0m,
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    EffectiveTo = new DateTime(2018, 8, 31)
                },
                new SfaDisadvantage
                {
                    Uplift = 1.0m,
                    EffectiveFrom = new DateTime(2018, 9, 1)
                }
            };

            var efaDisad = new List<EfaDisadvantage>
            {
                new EfaDisadvantage
                {
                    Uplift = 2.0m,
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    EffectiveTo = new DateTime(2018, 8, 31)
                },
                new EfaDisadvantage
                {
                    Uplift = 2.0m,
                    EffectiveFrom = new DateTime(2018, 9, 1)
                }
            };

            var dasDisad = new List<DasDisadvantage>
            {
                new DasDisadvantage
                {
                    Uplift = 3.0m,
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    EffectiveTo = new DateTime(2018, 8, 31)
                },
                new DasDisadvantage
                {
                    Uplift = 3.0m,
                    EffectiveFrom = new DateTime(2018, 9, 1)
                }
            };

            var sfaAreaCost = new List<SfaAreaCost>
            {
                new SfaAreaCost
                {
                    AreaCostFactor = 4.0m,
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    EffectiveTo = new DateTime(2018, 8, 31)
                },
                new SfaAreaCost
                {
                    AreaCostFactor = 4.0m,
                    EffectiveFrom = new DateTime(2018, 9, 1)
                }
            };

            var careerLearningPilot = new List<CareerLearningPilot>
            {
                new CareerLearningPilot
                {
                    AreaCode = "Area1",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    EffectiveTo = new DateTime(2018, 8, 31)
                },
                new CareerLearningPilot
                {
                    AreaCode = "Area1",
                    EffectiveFrom = new DateTime(2018, 9, 1)
                }
            };

            var onsData = new List<ONSData>
            {
                new ONSData
                {
                    LocalAuthority = "Authority",
                    Lep1 = "Lep1",
                    Lep2 = "Lep2",
                    Nuts = "Nuts",
                    Termination = new DateTime(2020, 8, 31),
                    EffectiveFrom = new DateTime(2018, 8, 1)
                }
            };

            IEnumerable<MasterPostcode> postcodesList = new List<MasterPostcode>
            {
                new MasterPostcode
                {
                    Postcode = "Postcode1",
                    SfaPostcodeDisadvantages = new List<SfaPostcodeDisadvantage>
                    {
                        new SfaPostcodeDisadvantage
                        {
                            Uplift = 1.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = new DateTime(2018, 8, 31)
                        },
                        new SfaPostcodeDisadvantage
                        {
                            Uplift = 1.0m,
                            EffectiveFrom = new DateTime(2018, 9, 1)
                        }
                    },
                },
                new MasterPostcode
                {
                    Postcode = "Postcode2",
                    EfaPostcodeDisadvantages = new List<EfaPostcodeDisadvantage>
                    {
                        new EfaPostcodeDisadvantage
                        {
                            Uplift = 2.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = new DateTime(2018, 8, 31)
                        },
                        new EfaPostcodeDisadvantage
                        {
                            Uplift = 2.0m,
                            EffectiveFrom = new DateTime(2018, 9, 1)
                        }
                    },
                },
                new MasterPostcode
                {
                    Postcode = "Postcode3",
                    DasPostcodeDisadvantages = new List<DasPostcodeDisadvantage>
                    {
                        new DasPostcodeDisadvantage
                        {
                            Uplift = 3.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = new DateTime(2018, 8, 31)
                        },
                        new DasPostcodeDisadvantage
                        {
                            Uplift = 3.0m,
                            EffectiveFrom = new DateTime(2018, 9, 1)
                        }
                    },
                },
                new MasterPostcode
                {
                    Postcode = "Postcode4",
                    SfaPostcodeAreaCosts = new List<SfaPostcodeAreaCost>
                    {
                        new SfaPostcodeAreaCost
                        {
                            AreaCostFactor = 4.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = new DateTime(2018, 8, 31)
                        },
                        new SfaPostcodeAreaCost
                        {
                            AreaCostFactor = 4.0m,
                            EffectiveFrom = new DateTime(2018, 9, 1)
                        }
                    },
                },
                new MasterPostcode
                {
                    Postcode = "Postcode5",
                    CareerLearningPilotPostcodes = new List<CareerLearningPilotPostcode>
                    {
                        new CareerLearningPilotPostcode
                        {
                            AreaCode = "Area1",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = new DateTime(2018, 8, 31)
                        },
                        new CareerLearningPilotPostcode
                        {
                            AreaCode = "Area1",
                            EffectiveFrom = new DateTime(2018, 9, 1)
                        }
                    },
                },
                new MasterPostcode
                {
                    Postcode = "Postcode6",
                    OnsPostcodes = new List<OnsPostcode>
                    {
                        new OnsPostcode
                        {
                            LocalAuthority = "Authority",
                            Lep1 = "Lep1",
                            Lep2 = "Lep2",
                            Nuts = "Nuts",
                            Termination = "202008",
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    },
                },
                new MasterPostcode { Postcode = "Postcode7" },
                new MasterPostcode { Postcode = "Postcode8" },
                new MasterPostcode { Postcode = "Postcode9" },
                new MasterPostcode { Postcode = "Postcode10" },
            };

            var postcodesDbMock = postcodesList.AsQueryable().BuildMockDbSet();

            postcodesMock.Setup(p => p.MasterPostcodes).Returns(postcodesDbMock.Object);

            var serviceResult = await NewService(postcodesMock.Object).RetrieveAsync(postcodes, CancellationToken.None);

            serviceResult.Count().Should().Be(6);
            serviceResult.Keys.ToList().Should().BeEquivalentTo(postcodes);
            serviceResult["Postcode1"].SfaDisadvantages.Should().BeEquivalentTo(sfaDisad);
            serviceResult["Postcode2"].EfaDisadvantages.Should().BeEquivalentTo(efaDisad);
            serviceResult["Postcode3"].DasDisadvantages.Should().BeEquivalentTo(dasDisad);
            serviceResult["Postcode4"].SfaAreaCosts.Should().BeEquivalentTo(sfaAreaCost);
            serviceResult["Postcode5"].CareerLearningPilots.Should().BeEquivalentTo(careerLearningPilot);
            serviceResult["Postcode6"].ONSData.Should().BeEquivalentTo(onsData);
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

        private PostcodesRepositoryService NewService(IPostcodesContext postcodesContext = null)
        {
            return new PostcodesRepositoryService(postcodesContext);
        }
    }
}
