using ACBACustomer.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACBACustomer.Data.DataModel
{
    public class Document : BaseModel
    {
        public int DocumentId { get; set; }
        public int CustomerId { get; set; }
        public TypeOfDocument TypeOfDocument { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime DateOfIssue { get; set; }
        public string IssuingAuthority { get; set; }
        public string Country { get; set; }
        private string errorDocumentMessage { get; set; }

        public Document()
        {

        }
        public Document(int customerId, TypeOfDocument typeOfDocument, string documentNumber, DateTime dateOfIssue, string issuingAuthority, string country)
        {
           if(!Validate(documentNumber, issuingAuthority, typeOfDocument, country))
            {
                throw new ArgumentException(errorDocumentMessage);
            }
            else
            {
                this.CustomerId = customerId;
                this.TypeOfDocument = typeOfDocument;
                this.DocumentNumber = documentNumber;
                this.DateOfIssue = dateOfIssue;
                this.Country = country;
                this.IssuingAuthority = issuingAuthority;
            }
        }
        public Document Edit(TypeOfDocument typeOfDocument, string docNumber, DateTime dateOfIssue, string issuingAuthority, string country)
        {
            if (!Validate(docNumber, issuingAuthority, typeOfDocument, country))
            {
                throw new ArgumentException(errorDocumentMessage);
            }
            else
            {
                this.DocumentNumber = docNumber;
                this.IssuingAuthority = issuingAuthority;
                this.TypeOfDocument = typeOfDocument;
                this.DateOfIssue = dateOfIssue;
                this.Country = country;
                return this;
            }
        }
        private void AddDocumentErrorMessage(string error)
        {
            if (this.errorDocumentMessage == string.Empty)
            {
                this.errorDocumentMessage = Resources.Error_Document_Message_Header + "\n\n";
            }

            this.errorDocumentMessage += error + "\n";
        }
        public bool Validate(string docNumber, string issAuthority, TypeOfDocument typeOfDoc, string country)
        {
            this.errorDocumentMessage = string.Empty;

            if (docNumber.Trim() == string.Empty)
            {
                this.AddDocumentErrorMessage(Resources.Registration_DocumentNumber_Required_Text);
            }

            if (issAuthority.Trim() == string.Empty)
            {
                this.AddDocumentErrorMessage(Resources.Registration_IssuingAuthority_Required_Text);
            }

            if (typeOfDoc <= 0)
            {
                this.AddDocumentErrorMessage(Resources.Registration_TypeOfDocument_Select_Text);
            }

            if (country.Trim() == string.Empty || country.Trim() == "-Please Select-")
            {
                this.AddDocumentErrorMessage(Resources.Registration_Country_Required_Text);
            }

            return this.errorDocumentMessage == string.Empty;
        }
    }


}
