using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using CommandLine;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Context;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Modules;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Console.modules;
using ESFA.DC.Logging.Desktop.Config;
using ESFA.DC.Logging.Desktop.Config.Interfaces;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.Serialization.Json;
using Microsoft.Extensions.Configuration;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var importClass = new ImportClass();
            importClass.ImportFile(args);
        }
    }
}
