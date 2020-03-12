using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
