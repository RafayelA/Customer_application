using ACBACustomer.Data.DataModel;
using ACBACustomer.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACBACustomer.Data.DataAccess
{
    public class CustomerAccess : ConnectionAccess
    {
        
        public DataTable GetAllCustomers()
        {
            DataTable dataTable = new DataTable();

            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter())
            {
                // Create the command and set its properties
                sqlDataAdapter.SelectCommand = new SqlCommand();
                sqlDataAdapter.SelectCommand.Connection = new SqlConnection(ConnectionString);
                sqlDataAdapter.SelectCommand.CommandType = CommandType.Text;

                // Assign the SQL to the command object
                sqlDataAdapter.SelectCommand.CommandText = Scripts.sqlGetAllCustomers;

                // Fill the datatable from adapter
                sqlDataAdapter.Fill(dataTable);
            }

            return dataTable;
        }

        /// <summary>
        /// Service method to get customer by Id
        /// </summary>
        /// <param name="id">customer id</param>
        /// <returns>Data row</returns>
        public DataRow GetCustomerById(int id)
        {
            DataTable dataTable = new DataTable();
            DataRow dataRow;

            using (SqlDataAdapter dataAdapter = new SqlDataAdapter())
            {
                // Create the command and set its properties
                dataAdapter.SelectCommand = new SqlCommand();
                dataAdapter.SelectCommand.Connection = new SqlConnection(ConnectionString);
                dataAdapter.SelectCommand.CommandType = CommandType.Text;
                dataAdapter.SelectCommand.CommandText = Scripts.sqlGetCustomerById;

                // Add the parameter to the parameter collection
                dataAdapter.SelectCommand.Parameters.AddWithValue("@Id", id);

                // Fill the datatable From adapter
                dataAdapter.Fill(dataTable);

                // Get the datarow from the table
                dataRow = dataTable.Rows.Count > 0 ? dataTable.Rows[0] : null;

                return dataRow;
            }
        }

        public DataRow GetDocumentById(int id)
        {
            DataTable dataTable = new DataTable();
            DataRow dataRow;

            using (SqlDataAdapter dataAdapter = new SqlDataAdapter())
            {
                // Create the command and set its properties
                dataAdapter.SelectCommand = new SqlCommand();
                dataAdapter.SelectCommand.Connection = new SqlConnection(ConnectionString);
                dataAdapter.SelectCommand.CommandType = CommandType.Text;
                dataAdapter.SelectCommand.CommandText = Scripts.sqlGetDocumentById;

                // Add the parameter to the parameter collection
                dataAdapter.SelectCommand.Parameters.AddWithValue("@Id", id);

                // Fill the datatable From adapter
                dataAdapter.Fill(dataTable);

                // Get the datarow from the table
                dataRow = dataTable.Rows.Count > 0 ? dataTable.Rows[0] : null;

                return dataRow;
            }
        }

        /// <summary>
        /// Service method to search records by multiple parameters
        /// </summary>
        /// <param name="firstName">first name value</param>
        /// <param name="lastName">last name value</param>
        /// <param name="docNumber">document number value</param>
        /// <returns>Data table</returns>
        //public DataTable SearchCustomers(string firstName, string lastName, string email)
        //{
        //    DataTable dataTable = new DataTable();

        //    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter())
        //    {
        //        // Create the command and set its properties
        //        sqlDataAdapter.SelectCommand = new SqlCommand();
        //        sqlDataAdapter.SelectCommand.Connection = new SqlConnection(ConnectionString);
        //        sqlDataAdapter.SelectCommand.CommandType = CommandType.Text;

        //        // Assign the SQL to the command object
        //        sqlDataAdapter.SelectCommand.CommandText = Scripts.sqlSearchCustomers;

        //        // Add the input parameters to the parameter collection
        //        sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@FirstName", firstName == null ? DBNull.Value : firstName);

        //        sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@LastName", lastName == null ? DBNull.Value : lastName);

        //        sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@Email", email == null ? DBNull.Value : email);

        //        // Fill the table from adapter
        //        sqlDataAdapter.Fill(dataTable);
        //    }

        //    return dataTable;
        //}

        /// <summary>
        /// Service method to create new customer
        /// </summary>
        /// <param name="customer">customer model</param>
        /// <returns>true or false</returns>
        public bool AddCustomer(Customer customer)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                // Set the command object properties
                sqlCommand.Connection = new SqlConnection(ConnectionString);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = Scripts.sqlInsertCustomer;

                // Add the input parameters to the parameter collection
                sqlCommand.Parameters.AddWithValue("@FirstName", customer.FirstName);
                sqlCommand.Parameters.AddWithValue("@LastName", customer.LastName);
                sqlCommand.Parameters.AddWithValue("@Gender", customer.Gender);
                sqlCommand.Parameters.AddWithValue("@DateOfBirth", customer.DateOfBirth.ToShortDateString());
                sqlCommand.Parameters.AddWithValue("@Email",customer.Email);
                //sqlCommand.Parameters.AddWithValue("@TypeOfDocument", (int)customer.TypeOfDocument);

                // Open the connection, execute the query and close the connection
                sqlCommand.Connection.Open();
                var rowsAffected = sqlCommand.ExecuteNonQuery();
                sqlCommand.Connection.Close();

                return rowsAffected > 0;
            }
        }

        public bool AddDocument(Document document)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                // Set the command object properties
                sqlCommand.Connection = new SqlConnection(ConnectionString);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = Scripts.sqlInsertDocument;

                // Add the input parameters to the parameter collection
                
                sqlCommand.Parameters.AddWithValue("@TypeOfDocument", (int)document.TypeOfDocument);

                // Open the connection, execute the query and close the connection
                sqlCommand.Connection.Open();
                var rowsAffected = sqlCommand.ExecuteNonQuery();
                sqlCommand.Connection.Close();

                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// Service method to update customer
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>true / false</returns>
        public bool UpdateCustomer(Customer customer)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                // Set the command object properties
                sqlCommand.Connection = new SqlConnection(ConnectionString);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = Scripts.sqlUpdateCustomer;

                // Add the input parameters to the parameter collection
                sqlCommand.Parameters.AddWithValue("@FirstName", customer.FirstName);
                sqlCommand.Parameters.AddWithValue("@LastName", customer.LastName);
                sqlCommand.Parameters.AddWithValue("@Gender", customer.Gender);
                sqlCommand.Parameters.AddWithValue("@DateOfBirth", customer.DateOfBirth.ToShortDateString());
                sqlCommand.Parameters.AddWithValue("@Email", customer.Email);
                sqlCommand.Parameters.AddWithValue("@Id", customer.Id);

                // Open the connection, execute the query and close the connection
                sqlCommand.Connection.Open();
                var rowsAffected = sqlCommand.ExecuteNonQuery();
                sqlCommand.Connection.Close();

                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// Method to delete a customer
        /// </summary>
        /// <param name="id">customer id</param>
        /// <returns>true / false</returns>
        public bool DeleteCustomer(int id)
        {
            
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                // Set the command object properties
                sqlCommand.Connection = new SqlConnection(ConnectionString);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = Scripts.sqlDeleteCustomer;

                // Add the input parameter to the parameter collection
                sqlCommand.Parameters.AddWithValue("@Id", id);

                // Open the connection, execute the query and close the connection
                sqlCommand.Connection.Open();
                var rowsAffected = sqlCommand.ExecuteNonQuery();
                sqlCommand.Connection.Close();

                return rowsAffected > 0;
            }
        }
    }
}
