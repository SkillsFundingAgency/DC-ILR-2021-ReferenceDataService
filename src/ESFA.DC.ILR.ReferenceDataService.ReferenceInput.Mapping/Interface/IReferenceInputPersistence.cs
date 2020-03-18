using System.Collections.Generic;
using System.Data.SqlClient;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping.Interface
{
    public interface IReferenceInputPersistence
    {
        void PersistEfModelByType<T>(SqlConnection connection, SqlTransaction sqlTransaction, IEnumerable<T> source);

        void PersistEfModelByTypeWithoutCollections<T>(SqlConnection connection, SqlTransaction sqlTransaction, IEnumerable<T> source);
    }
}
