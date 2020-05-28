using System;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Entity;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ReferenceData.Postcodes.Model;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Mapper
{
    public class PostcodeEntityModelMapperTests
    {
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

            NewMapper().SfaPostcodeDisadvantagesToEntity(sfaPostcodeDisadvantage).Should().BeEquivalentTo(sfaDisdvantage);
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

            NewMapper().EfaPostcodeDisadvantagesToEntity(efaPostcodeDisadvantage).Should().BeEquivalentTo(efaDisdvantage);
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

            NewMapper().DasPostcodeDisadvantagesToEntity(dasPostcodeDisadvantage).Should().BeEquivalentTo(dasDisdvantage);
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

            NewMapper().SfaAreaCostsToEntity(sfaPostcodeAreaCost).Should().BeEquivalentTo(sfaAreaCost);
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

            NewMapper().ONSDataToEntity(onsPostcode).Should().BeEquivalentTo(onsData);
        }

        private PostcodesEntityModelMapper NewMapper()
        {
            return new PostcodesEntityModelMapper();
        }
    }
}
