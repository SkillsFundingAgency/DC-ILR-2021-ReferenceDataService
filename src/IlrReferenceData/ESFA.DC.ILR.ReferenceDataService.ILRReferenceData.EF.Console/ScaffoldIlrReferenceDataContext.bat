dotnet.exe ef dbcontext scaffold "Server=.\;Database=ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Database;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -c IlrReferenceDataContext --schema dbo --force --startup-project . --project ..\ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model --verbose

pause