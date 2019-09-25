﻿using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface
{
    public interface IBulkInsert
    {
        Task Insert<T>(string table, IEnumerable<T> source, SqlConnection sqlConnection, SqlTransaction sqlTransaction, CancellationToken cancellationToken);
    }
}
