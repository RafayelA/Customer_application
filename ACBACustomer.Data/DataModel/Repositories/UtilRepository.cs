using ACBACustomer.Data.Enums;
using ACBACustomer.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ACBACustomer.Data.DataModel.Repositories
{
    public interface IUtilRepository
    {
        List<string> Select(string table);
    }

    public class UtilRepository : BaseRepository<string>, IUtilRepository
    {
        public List<string> Select(string table)
        {

            var pTable = new SqlParameter("@Table", table);
            return Execute(Scripts.sqlUtilSelect, pTable);
        }       

        protected override string CreateEntity(IDataRecord dataRecord)
        {
            return dataRecord["Name"].ToString();
        }

        protected override List<string> CreateEntities(DataTable table)
        {
            throw new NotImplementedException();
        }
    }
}
