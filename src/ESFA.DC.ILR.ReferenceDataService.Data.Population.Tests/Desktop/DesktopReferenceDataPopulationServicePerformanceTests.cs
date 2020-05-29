using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData.Repository;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Entity;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Desktop
{
    public class DesktopReferenceDataPopulationServicePerformanceTests
    {
        [Fact(Skip = "Used for performance testing, needs connection to populated postcode Db")]
        /* Original  - Before 38158336 after 5749288960 consumed 5711130624 duration 43567 retrieved 2273034
           Optimised - Before 38137856 after 3083907072 consumed 3045769216 duration 36318 retrieved 2273034
         */
        public async void RetrieveAsyncPerformance()
        {
            using (Process currentProc = Process.GetCurrentProcess())
            using (var outFile =
                File.CreateText($@"c:\temp\performance\RetrieveAsyncPerformance-{DateTime.Now:HH-mm-ss}.txt"))
            {
                var connString =
                    @"Server=DCOL-DEV-SqlServer-WEU.database.windows.net;Database=Postcodes;User Id=<userid>;Password=<password>;";

                var referenceDataOptions = new ReferenceDataOptions { PostcodesConnectionString = connString };

                var postcodesEntityModelMapper = new PostcodesEntityModelMapper();

                var desktopPostcodesRepositoryService =
                    new DesktopPostcodesRepositoryService(referenceDataOptions, postcodesEntityModelMapper);

                currentProc.Refresh();
                long beforeMemoryUsed = currentProc.PrivateMemorySize64;

                var sw = new Stopwatch();
                sw.Start();

                var data = await desktopPostcodesRepositoryService.RetrieveAsync(new CancellationToken());

                sw.Stop();
                currentProc.Refresh();
                long afterMemoryUsed = currentProc.PrivateMemorySize64;

                outFile.WriteLine(
                    $"Before {beforeMemoryUsed} after {afterMemoryUsed} consumed {afterMemoryUsed - beforeMemoryUsed} duration {sw.ElapsedMilliseconds} retrieved {data.Count}");
                outFile.Flush();
            }
        }
    }
}