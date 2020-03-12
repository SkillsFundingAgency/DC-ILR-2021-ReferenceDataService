namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping.Interface
{
    public interface IReferenceInputEFMapper
    {
        TTarget MapByType<TSource, TTarget>(TSource source);
    }
}
