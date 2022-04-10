using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACBACustomer.Data.Sql
{
    /// <summary>
    /// DBConstants static class contains sql string constants
    /// </summary>
    public static class Scripts
    {
        /// <summary>
        /// Sql to get a customer details by Id
        /// </summary>
        public static readonly string sqlGetCustomerById =
            "SELECT c.Id, c.FirstName, c.LastName, c.Email, c.DateOfBirth, c.Gender, c.IsActive," +
            " d.DocumentId, d.TypeOfDocument, d.DocumentNumber, d.DateOfIssue, d.IssuingAuthority, d.Country" +
            " FROM Customer c" +
            " JOIN Document AS d" +
            " ON c.Id = d.CustomerId" +
            " Where c.Id = @Id";

        public static readonly string sqlGetDocumentById =
            " SELECT DocumentId, TypeOfDocument, DocumentNumber, DateOfIssue, IssuingAuthority, Country" +
            " FROM Document" +
            " Where DocumentId = @DocumentId";

        /// <summary>
        /// Sql to get all customers
        /// </summary>
        public static readonly string sqlGetAllCustomers =
            "SELECT c.Id, c.FirstName, c.LastName, c.Email, c.DateOfBirth, c.Gender, c.IsActive, c.CreatedOn, c.ModifiedOn," +
            " d.DocumentId, d.CustomerId, d.TypeOfDocument, d.DocumentNumber, d.DateOfIssue, d.IssuingAuthority, d.Country, d.CreatedOn As dc, d.ModifiedOn As dm" +
            " FROM Customer c" +
            " LEFT JOIN Document AS d" +
            " ON c.Id = d.CustomerId" +
            " Where c.IsActive = 'true'";

        /// <summary>
        /// sql to insert a customer details
        /// </summary>
        public static readonly string sqlInsertCustomer = 
            "Insert Into" +
            " Customer(FirstName, LastName, Email, DateOfBirth, Gender, IsActive, CreatedOn, ModifiedOn)" +
            " Values(@FirstName, @LastName, @Email, @DateOfBirth, @Gender, @IsActive, @CreatedOn, @ModifiedOn)";

        public static readonly string sqlInsertDocument =
            "Insert Into" +
            " Document(CustomerId, TypeOfDocument, DocumentNumber, DateOfIssue, IssuingAuthority, Country, CreatedOn, ModifiedOn)" +
            " Values(@CustomerId, @TypeOfDocument, @DocumentNumber, @DateOfIssue, @IssuingAuthority, @Country, @CreatedOn, @ModifiedOn)";

        /// <summary>
        /// sql to search for customers
        /// </summary>
        //public static readonly string sqlSearchCustomers = "Select" +
        //    " Id, FirstName, LastName, Email, DateOfBirth, Gender, IsActive" +
        //    " From Customer Where (FirstName Like @FirstName + '%') AND (LastName Like @LastName + '%') AND (Email Like @Email + '%')";

        public static readonly string sqlSearchCustomers =
            "SELECT c.Id, c.FirstName, c.LastName, c.Email, c.DateOfBirth, c.Gender, c.IsActive, c.CreatedOn, c.ModifiedOn," +
            " d.DocumentId, d.CustomerId, d.TypeOfDocument, d.DocumentNumber, d.DateOfIssue, d.IssuingAuthority, d.Country, d.CreatedOn As dc, d.ModifiedOn As dm" +
            " FROM Customer c" +
            " LEFT JOIN Document AS d" +
            " ON c.Id = d.CustomerId" +
            " Where c.IsActive = 'true' AND (FirstName Like @FirstName + '%') AND (LastName Like @LastName + '%') AND (Email Like @Email + '%')";

        /// <summary>
        /// sql to update customer details
        /// </summary>
        public static readonly string sqlUpdateCustomer = "Update Customer " +
            " Set [FirstName] = @FirstName, [LastName] = @LastName, [Email] = @Email, [ModifiedOn] = @ModifiedOn," +
            " [DateOfBirth] = @DateOfBirth, [Gender] = @Gender Where ([Id] = @Id)";

        /// <summary>
        /// sql to delete a customer record
        /// </summary>
        public static readonly string sqlDeleteCustomer = "Update Customer " +
            "Set IsActive = @IsActive, ModifiedOn = @ModifiedOn" +
            " Where Id = @Id";

        public static readonly string sqlDeleteDocument = "Delete From Document Where (DocumentId = @DocumentId)";

        public static readonly string sqlUtilSelect =
            "spUtilSelect";

        public static readonly string sqlUpdateDocument = "Update Document " +
            " Set [TypeOfDocument] = @TypeOfDocument, [DocumentNumber] = @DocumentNumber, [DateOfIssue] = @DateOfIssue," +
            " [IssuingAuthority] = @IssuingAuthority, [Country] = @Country,[ModifiedOn] = @ModifiedOn Where ([DocumentId] = @DocumentId)";
    }
}
