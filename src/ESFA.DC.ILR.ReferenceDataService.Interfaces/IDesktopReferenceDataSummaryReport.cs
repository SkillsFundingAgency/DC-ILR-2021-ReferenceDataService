using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.ILR.ReferenceDataService.Interfaces
{
    public interface IDesktopReferenceDataSummaryReport
    {
        string DataSource { get;  }

        int NumberOfRecords { get;  }
    }
}
