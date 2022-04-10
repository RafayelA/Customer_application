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
    public interface ICustomerRepository
    {
        Customer GetCustomerById(int id);
        List<Customer> SelectAll();
        public bool Update(Customer customer);
        public bool InsertWithinTransaction(Customer customer, Document doc);
        public bool UpdateWithinTransaction(Customer customer, Document doc);
    }

    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public List<Customer> SelectAll()
        {
            return ExecuteModels(Scripts.sqlGetAllCustomers);
        }

        public Customer GetCustomerById(int id)
        {
            var pId = new SqlParameter("@Id", id);
            return ExecuteModels(Scripts.sqlGetCustomerById, pId)
                .FirstOrDefault();
        }

        public bool InsertWithinTransaction(Customer customer, Document doc)
        {
            try
            {
                var options = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted};

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    var customerId = Insert(customer);
                    doc.CustomerId = customerId;
                    var repository = new DocumentRepository();
                    repository.Insert(doc);                    
                    scope.Complete();
                }

                return true;
            }
            catch (Exception)
            {                
                return false;
            }
        }
        public bool UpdateWithinTransaction(Customer customer, Document doc)
        {
            try
            {
                var options = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted };

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    var customerId = Update(customer);
                    var repository = new DocumentRepository();
                    repository.Update(doc);
                    scope.Complete();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private int Insert(Customer customer)
        {
            var pName = new SqlParameter("@FirstName", customer.FirstName);
            var pLastName = new SqlParameter("@LastName", customer.LastName);
            var pEmail = new SqlParameter("@Email", customer.Email);
            var pDateOfBirth = new SqlParameter("@DateOfBirth", customer.DateOfBirth);
            var pGender = new SqlParameter("@Gender", customer.Gender);
            var pIsActive = new SqlParameter("@IsActive", customer.IsActive);
            var pCreatedOn = new SqlParameter("@CreatedOn", customer.CreatedOn);
            var pModifiedOn = new SqlParameter("@ModifiedOn", customer.ModifiedOn);

            var id = ExecuteScalar(Scripts.sqlInsertCustomer,
                pName,
                pLastName,
                pEmail,
                pDateOfBirth,
                pGender,
                pIsActive,
                pCreatedOn,
                pModifiedOn);

            return id;
        }

        public bool Update(Customer customer)
        {         
            var pId = new SqlParameter("@Id", customer.Id);
            var pFirstName = new SqlParameter("@FirstName", customer.FirstName);
            var pLastName = new SqlParameter("@LastName", customer.LastName);
            var pEmail = new SqlParameter("@Email", customer.Email);
            var pDateOfBirth = new SqlParameter("@DateOfBirth", customer.DateOfBirth);
            var pGender = new SqlParameter("@Gender", customer.Gender);
            var pModifiedOn = new SqlParameter("@ModifiedOn", DateTime.Now);

            int count = ExecuteNonQuery(Scripts.sqlUpdateCustomer,
                pId,
                pFirstName,
                pLastName,
                pEmail,
                pDateOfBirth,
                pGender,
                pModifiedOn);
            return count > 0;
        }
        public List<Customer> Search(string firstName, string lastName, string email)
        {
            var pFirstName = new SqlParameter("@FirstName", firstName);
            var pLastName = new SqlParameter("@LastName", lastName);
            var pEmail = new SqlParameter("@Email", email);

            return ExecuteModels(Scripts.sqlSearchCustomers,
                pFirstName,
                pLastName,
                pEmail);
        }
        public bool Delete(int id)
        {
            return SoftDelete("Customer", id);
        }

        protected override Customer CreateEntity(IDataRecord dataRecord)
        {
            var customer = new Customer
            {
                Id = (int)dataRecord["Id"],
                FirstName = dataRecord["FirstName"].ToString(),
                LastName = dataRecord["LastName"].ToString(),
                Email = dataRecord["Email"].ToString(),
                DateOfBirth = DateTime.Parse(dataRecord["DateOfBirth"].ToString()),
                Gender = dataRecord["Gender"].ToString()
            };

            return customer;
        }


        protected override List<Customer> CreateEntities(DataTable table)
        {
            var map = new Dictionary<int, Customer>();
            foreach (DataRow row in table.Rows)
            {
                int id = (int)row["Id"];
                if (!map.TryGetValue(id, out var customer))
                {
                    customer = new Customer { Id = id };
                    customer.FirstName = row["FirstName"].ToString();
                    customer.LastName = row["LastName"].ToString();
                    customer.Email = row["Email"].ToString();
                    customer.DateOfBirth = DateTime.Parse(row["DateOfBirth"].ToString());
                    customer.Gender = row["Gender"].ToString();
                    customer.IsActive = Convert.ToBoolean(row["IsActive"]);
                    customer.CreatedOn = DateTime.Parse(row["CreatedOn"].ToString());
                    customer.ModifiedOn = DateTime.Parse(row["ModifiedOn"].ToString());

                    customer.Documents = new List<Document>();
                    map.Add(id, customer);
                }

                var doc = new Document();
                if (row["DocumentId"] != DBNull.Value)
                {
                    doc.DocumentId = (int)row["DocumentId"];
                    doc.CustomerId = (int)row["CustomerId"];
                    doc.TypeOfDocument = (TypeOfDocument)Enum.Parse(typeof(TypeOfDocument), row["TypeOfDocument"].ToString());
                    doc.DocumentNumber = (string)row["DocumentNumber"];
                    doc.DateOfIssue = DateTime.Parse(row["DateOfIssue"].ToString());
                    doc.IssuingAuthority = (string)row["IssuingAuthority"];
                    doc.Country = row["Country"].ToString();
                    doc.CreatedOn = DateTime.Parse(row["dc"].ToString());
                    doc.ModifiedOn = DateTime.Parse(row["dm"].ToString());

                    customer.Documents.Add(doc);
                }
            }

            return map.Values.ToList();
        }
    }
}
