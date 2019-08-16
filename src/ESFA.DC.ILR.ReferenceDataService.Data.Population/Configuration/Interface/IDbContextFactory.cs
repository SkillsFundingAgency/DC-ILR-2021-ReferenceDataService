namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface
{
    public interface IDbContextFactory<T>
    {
        T Create();
    }
}
