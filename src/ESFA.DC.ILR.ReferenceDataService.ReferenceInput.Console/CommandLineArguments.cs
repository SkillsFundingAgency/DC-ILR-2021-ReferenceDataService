namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Console
{
    public class CommandLineArguments
    {
        [Option('s', "sourcefile", Required = true)]
        public string SourceFile { get; set; }
    }
}
