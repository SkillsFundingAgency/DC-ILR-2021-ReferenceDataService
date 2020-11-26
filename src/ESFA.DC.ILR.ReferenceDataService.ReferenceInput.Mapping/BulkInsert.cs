using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using FastMember;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping
{
    public class BulkInsert
    {
        public void InsertWithIds<T>(string table, IEnumerable<T> source, SqlConnection sqlConnection, SqlTransaction sqlTransaction, int bulkCopyTimeout)
        {
            using (var sqlBulkCopy = BuildSqlBulkCopy(sqlConnection, sqlTransaction, bulkCopyTimeout))
            {
                try
                {
                    if (source == null || !source.Any())
                    {
                        return;
                    }

                    var columnNames = typeof(T).GetProperties().Where(p => !p.GetMethod.IsVirtual).Select(p => p.Name)
                        .ToArray();

                    using (var reader = ObjectReader.Create(source, columnNames))
                    {
                        sqlBulkCopy.DestinationTableName = table;

                        foreach (var name in columnNames)
                        {
                            sqlBulkCopy.ColumnMappings.Add(name, name);
                        }

                        sqlBulkCopy.WriteToServer(reader);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(table);
                    Console.Write(ex);
                    throw;
                }
            }
        }

        private SqlBulkCopy BuildSqlBulkCopy(SqlConnection sqlConnection, SqlTransaction sqlTransaction, int bulkCopyTimeout)
        {
            var sqlBulkCopyOptions = SqlBulkCopyOptions.KeepIdentity;

            return new SqlBulkCopy(sqlConnection, sqlBulkCopyOptions, sqlTransaction)
            {
                BatchSize = 20_000, // https://stackoverflow.com/questions/779690/what-is-the-recommended-batch-size-for-sqlbulkcopy
                BulkCopyTimeout = bulkCopyTimeout
            };
        }
    }
}
