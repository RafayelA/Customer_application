using ACBACustomer.Data.Enums;
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
    public interface IDocumentRepository
    {
        public bool Insert(Document doc);
        public bool Delete(Document doc);
        public bool Update(Document doc);
    }
    public class DocumentRepository : BaseRepository<Document>, IDocumentRepository
    {
        public Document GetDocumentById(int id)
        {
            var pId = new SqlParameter("@DocumentId", id);
            return Execute(Scripts.sqlGetDocumentById, pId)
                .FirstOrDefault();
        }

        public bool Insert(Document doc)
        {
            var pCustomerId = new SqlParameter("@CustomerId", doc.CustomerId);
            var pTypeOfDocument = new SqlParameter("@TypeOfDocument", doc.TypeOfDocument);
            var pDocumentNumber = new SqlParameter("@DocumentNumber", doc.DocumentNumber);
            var pDateOfIssue = new SqlParameter("@DateOfIssue", doc.DateOfIssue);
            var pIssuingAuthority = new SqlParameter("@IssuingAuthority", doc.IssuingAuthority);
            var pCountry = new SqlParameter("@Country", doc.Country);

            var pCreatedOn = new SqlParameter("@CreatedOn", DateTime.Now);
            var pModifiedOn = new SqlParameter("@ModifiedOn", DateTime.Now);

            var count = ExecuteNonQuery(Scripts.sqlInsertDocument,
                pCustomerId,
                pTypeOfDocument,
                pDocumentNumber,
                pDateOfIssue,
                pIssuingAuthority,
                pCreatedOn,
                pModifiedOn,
                pCountry);

            return count > 0;
        }
        public bool Update(Document doc)
        {
            var pDocumentId = new SqlParameter("@DocumentId", doc.DocumentId);
            var pTypeOfDocument = new SqlParameter("@TypeOfDocument", doc.TypeOfDocument);
            var pDocumentNumber = new SqlParameter("@DocumentNumber", doc.DocumentNumber);
            var pDateOfIssue = new SqlParameter("@DateOfIssue", doc.DateOfIssue);
            var pIssuingAuthority = new SqlParameter("@IssuingAuthority", doc.IssuingAuthority);
            var pCountry = new SqlParameter("@Country", doc.Country);
            var pModifiedOn = new SqlParameter("@ModifiedOn", DateTime.Now);

            var count = ExecuteNonQuery(Scripts.sqlUpdateDocument,
                pDocumentId,
                pTypeOfDocument,
                pDocumentNumber,
                pDateOfIssue,
                pIssuingAuthority,
                pModifiedOn,
                pCountry);

            return (count > 0);
        }
        public bool Delete(Document doc)
        {
            var pId = new SqlParameter("@DocumentId", doc.DocumentId);
            int count = ExecuteNonQuery(Scripts.sqlDeleteDocument, pId);
            return count > 0;
        }

        protected override List<Document> CreateEntities(DataTable table)
        {
            throw new NotImplementedException();
        }

        protected override Document CreateEntity(IDataRecord dataRecord)
        {
            throw new NotImplementedException();
        }


    }
}
