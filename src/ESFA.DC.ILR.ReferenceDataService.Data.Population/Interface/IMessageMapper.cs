using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface
{
    public interface IMessageMapper<TMapOut>
    {
        TMapOut MapFromMessage(IMessage input);
    }
}
