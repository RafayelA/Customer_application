using ACBACustomer.Data.Enums;
using ACBACustomer.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACBACustomer.Data.DataModel
{
    public class Customer : BaseModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public bool IsActive { get; set; }
        public List<Document> Documents { get; set; }

        string errorCustomerMessage;

        public Customer()
        {

        }
        public Customer(string firstName, string lastName, string email, DateTime dateOfBirth, string gender, List<Document> documents)
        {
            if (!Validate(firstName, lastName, email, gender))
            {
                throw new ArgumentException(errorCustomerMessage);
            }
            else
            {
                this.FirstName = firstName;
                this.LastName = lastName;
                this.Email = email;
                this.DateOfBirth = dateOfBirth;
                this.Gender = gender;
                this.IsActive = true;
                this.Documents = documents;
                this.ModifiedOn = DateTime.Now;
                this.CreatedOn = DateTime.Now;
            }
        }
        private bool Validate(string firstName, string lastName, string email, string gender)
        {
            this.errorCustomerMessage = string.Empty;

            if (firstName.Trim() == string.Empty)
            {
                this.AddErrorMessage(Resources.Registration_Firstname_Required_Text);
            }

            if (email.Trim() == string.Empty)
            {
                this.AddErrorMessage(Resources.Registration_Email_Required_Text);
            }
            if (!email.Contains("@"))
            {
                this.AddErrorMessage(Resources.Registration_Email_Valid_Text);
            }

            if (lastName.Trim() == string.Empty)
            {
                this.AddErrorMessage(Resources.Registration_Lastname_Required_Text);
            }

            if (gender.Trim() == string.Empty || gender.Trim() == "-Please Select-")
            {
                this.AddErrorMessage(Resources.Registration_Gender_Select_Text);
            }


            return this.errorCustomerMessage == string.Empty;
        }
        private void AddErrorMessage(string error)
        {
            if (this.errorCustomerMessage == string.Empty)
            {
                this.errorCustomerMessage = Resources.Error_Message_Header + "\n\n";
            }

            this.errorCustomerMessage += error + "\n";
        }
        public Customer Edit(string firstName, string lastName, string email, DateTime dateOfBirth, string gender)
        {
            if (!Validate(firstName, lastName, email, gender))
            {
                throw new ArgumentException(errorCustomerMessage);
            }
            else
            {
                this.FirstName = firstName;
                this.LastName = lastName;
                this.Email = email;
                this.DateOfBirth = dateOfBirth;
                this.Gender = gender;
                return this;
            }
        }
        public Document AddDocument(TypeOfDocument typeOfDocument, string documentNumber, DateTime dateOfIssue, string issuingAuthority, string country)
        {
            foreach (var doc in this.Documents)
            {
                if (doc.TypeOfDocument == typeOfDocument && doc.Country == country)
                {
                    throw new ArgumentException(Resources.AddDocument_Error_Message);
                }
            }
            var document = new Document(this.Id, typeOfDocument, documentNumber, dateOfIssue, issuingAuthority, country);
            this.Documents.Add(document);
            return document;
        }

        public Document RemoveDocument(int id)
        {
            try
            {
                foreach (var doc in this.Documents)
                {
                    if (doc.DocumentId == id)
                    {
                        this.Documents.RemoveAll(x => x.DocumentId == id);
                        return doc;
                    }
                }
                return new Document();
            }
            catch (Exception)
            {

                return new Document();
            }
        }
    }
}
