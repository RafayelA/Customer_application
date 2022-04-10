using ACBACustomer.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACBACustomer.Data.DataModel.Repositories
{
    public abstract class BaseRepository<TEntity>
    {
        protected List<TEntity> Execute(string query, params SqlParameter[] parameters)
        {
            
            var list = new List<TEntity>();
            using (var connection = new SqlConnection(ConnectionSettings.ConnectionString))
            {
                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    if (query.Substring(0, 2) == "sp")
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                    }
                    cmd.CommandText = query;
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var entity = CreateEntity(reader);
                            list.Add(entity);
                        }
                    }
                }
                return list;
            }
        }

        protected List<TEntity> ExecuteModels(string query, params SqlParameter[] parameters)
        {
            var list = new List<TEntity>();
            using (var connection = new SqlConnection(ConnectionSettings.ConnectionString))
            {
                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }


                    //implement for datatable
                    var reader = cmd.ExecuteReader();
                    var dataTable = new DataTable();
                    dataTable.Load(reader);
                    
                    if (dataTable.Rows.Count > 0 )
                        {                           
                            var entity = CreateEntities(dataTable);
                            list = entity;
                        }
                }
                return list;
            }
        }

        protected int ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(ConnectionSettings.ConnectionString))
            {
                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "Select @@Identity";

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
        protected int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(ConnectionSettings.ConnectionString))
            {
                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    return cmd.ExecuteNonQuery();
                }
            }
        }

        protected bool SoftDelete(string tableName, int id)
        {
            var pModifiedOn = new SqlParameter("@ModifiedOn", DateTime.Now);
            var pIsActive = new SqlParameter("@IsActive", false);
            var pId = new SqlParameter("@Id", id);
            int count = ExecuteNonQuery(Scripts.sqlDeleteCustomer, pIsActive, pId, pModifiedOn);
            return count > 0;
        }

        protected abstract TEntity CreateEntity(IDataRecord dataRecord);
        protected abstract List<TEntity> CreateEntities(DataTable table);
    }
}
