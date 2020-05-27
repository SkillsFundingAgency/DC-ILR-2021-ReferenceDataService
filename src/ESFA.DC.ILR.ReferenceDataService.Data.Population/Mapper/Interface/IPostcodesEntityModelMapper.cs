using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ReferenceData.Postcodes.Model;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface
{
    public interface IPostcodesEntityModelMapper
    {
        SfaDisadvantage SfaPostcodeDisadvantagesToEntity(SfaPostcodeDisadvantage sfaPostcodeDisadvantage);

        SfaAreaCost SfaAreaCostsToEntity(SfaPostcodeAreaCost sfaPostcodeAreaCost);

        DasDisadvantage DasPostcodeDisadvantagesToEntity(DasPostcodeDisadvantage dasPostcodeDisadvantage);

        EfaDisadvantage EfaPostcodeDisadvantagesToEntity(EfaPostcodeDisadvantage efaPostcodeDisadvantage);

        ONSData ONSDataToEntity(OnsPostcode onsPostcode);
    }
}
