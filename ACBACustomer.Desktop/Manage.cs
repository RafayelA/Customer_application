using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using ACBACustomer.Data.BusinessService;
using ACBACustomer.Data.DataAccess;
using ACBACustomer.Data.DataModel;
using ACBACustomer.Data.DataModel.Repositories;
using ACBACustomer.Data.Enums;
using ACBACustomer.Data.Sql;
using ACBACustomer.Desktop.Properties;

namespace ACBACustomer.Desktop
{
    public partial class Manage : Form
    {
        private int customerId;

        private int docId;

        private readonly IUtilRepository utilRepository;

        private readonly IDocumentRepository docRepository;

        private readonly ICustomerRepository customerRepository;

        public List<Customer> Customers;
        public Manage()
        {
            this.InitializeComponent();
            utilRepository = new UtilRepository();
            docRepository = new DocumentRepository();
            customerRepository = new CustomerRepository();
            this.InitializeDropDownList();
            this.LoadDgvCustomer(ref Customers);
            this.ResetRegistration();

            FillCountryCombo();
            FillGenderCombo();
        }

        private List<Document> GetDocumentFromCustomer(int Id)
        {
            var customers = Customers;
            foreach (var customer in customers)
            {
                if (customer.Id == Id && customer.Documents.Count > 0)
                {
                    return customer.Documents;
                }
            }

            return new List<Document>();
        }

        private void InitializeDropDownList()
        {
            cmbTypeOfDocument.DataSource = Enum.GetValues(typeof(TypeOfDocument));
        }

        
        private void DataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {

                if (e.ColumnIndex == 4)
                {
                    e.Value = string.Format("{0:dd/MM/yyyy}", e.Value);
                }

                if (e.ColumnIndex == 7)
                {
                    e.Value = string.Format("{0:dd/MM/yyyy hh:mm:ss}", e.Value);
                }

                if (e.ColumnIndex == 8)
                {
                    e.Value = string.Format("{0:dd/MM/yyyy hh:mm:ss}", e.Value);
                }
            }
            catch (Exception ex)
            {
                this.ShowErrorMessage(ex);
            }
        }
        private void dgvDocuments_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 2)
                {
                    e.Value = Enum.GetName(typeof(TypeOfDocument), e.Value);
                }

                if (e.ColumnIndex == 4)
                {
                    e.Value = string.Format("{0:dd/MM/yyyy}", e.Value);
                }

                if (e.ColumnIndex == 6)
                {
                    e.Value = string.Format("{0:dd/MM/yyyy hh:mm:ss}", e.Value);
                }

                if (e.ColumnIndex == 7)
                {
                    e.Value = string.Format("{0:dd/MM/yyyy hh:mm:ss}", e.Value);
                }
            }
            catch (Exception ex)
            {
                this.ShowErrorMessage(ex);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var customer = new Customer(txtFirstName.Text, txtLastName.Text, txtEmail.Text, dtDateOfBirth.Value, cmbGender.SelectedValue.ToString(), new List<Document>());

                var doc = new Document(customer.Id, (TypeOfDocument)(cmbTypeOfDocument.SelectedValue ?? -1), txtDocumentNumber.Text, dtDateOfIssue.Value, txtIssuingAuthority.Text, cmbCountry.SelectedValue.ToString());

                var success = customerRepository.InsertWithinTransaction(customer, doc);

                if (success)
                {
                    LoadDgvCustomer(ref Customers);
                    MessageBox.Show(
                        DesktopResources.Registration_Successful_Message,
                        DesktopResources.Registration_Successful_Message_Title,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        DesktopResources.Registration_Error_Message,
                        DesktopResources.Registration_Error_Message_Title,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(
                        ex.Message,
                        DesktopResources.Registration_Error_Message_Title,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }
        private void btnUpdateCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                var customer = Customers.Where(c => c.Id == customerId).FirstOrDefault();
                customer = customer.Edit(txtFirstName.Text, txtLastName.Text, txtEmail.Text, dtDateOfBirth.Value, cmbGender.SelectedValue.ToString());

                bool success;
                if (customer.Documents.Where(d => d.DocumentId == docId).FirstOrDefault() != null)
                {
                    var doc = customer.Documents.Where(d => d.DocumentId == docId).FirstOrDefault();
                    doc = doc.Edit((TypeOfDocument)cmbTypeOfDocument.SelectedValue, txtDocumentNumber.Text, dtDateOfIssue.Value, txtIssuingAuthority.Text, cmbCountry.SelectedValue.ToString());

                    success = customerRepository.UpdateWithinTransaction(customer, doc);
                }
                else
                {
                    success = customerRepository.Update(customer);
                }                

                if (success)
                {
                    LoadDgvCustomer(ref Customers);
                    MessageBox.Show(
                        DesktopResources.Registration_Successful_Message,
                        DesktopResources.Registration_Successful_Message_Title,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        DesktopResources.Registration_Error_Message,
                        DesktopResources.Registration_Error_Message_Title,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(
                        ex.Message,
                        DesktopResources.Registration_Error_Message_Title,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }

        private void btnAddDocument_Click(object sender, EventArgs e)
        {
            try
            {
                var customer = Customers.Where(c => c.Id == customerId).FirstOrDefault();
                var doc = customer.AddDocument((TypeOfDocument)cmbTypeOfDocument.SelectedValue, txtDocumentNumber.Text, dtDateOfIssue.Value, txtIssuingAuthority.Text, cmbCountry.SelectedValue.ToString());
                var document = docRepository.Insert(doc);

                MessageBox.Show(
                            DesktopResources.AddDocument_Succeddful_Message, DesktopResources.AddDocument_Succeddful_Message_Title,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                LoadDgvDocument();

            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(
                        ex.Message,
                        DesktopResources.Registration_Document_Error_Message_Title,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }
        private void btnUpdateDocument_Click(object sender, EventArgs e)
        {
            try
            {
                var customer = Customers.Where(c => c.Id == customerId).FirstOrDefault();
                var doc = customer.Documents.Where(d => d.DocumentId == docId).FirstOrDefault();
                doc = doc.Edit((TypeOfDocument)cmbTypeOfDocument.SelectedValue, txtDocumentNumber.Text, dtDateOfIssue.Value, txtIssuingAuthority.Text, cmbCountry.SelectedValue.ToString());
                var success = docRepository.Update(doc);

                if (success)
                {
                    LoadDgvDocument();

                    MessageBox.Show(
                        DesktopResources.Update_Successful_Message,
                        DesktopResources.Update_Successful_Message_Title,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        DesktopResources.Update_Error_Message,
                        DesktopResources.Update_Error_Message_Title,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(
                        ex.Message,
                        DesktopResources.Registration_Error_Message_Title,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }
        private void btnRemoveDocument_Click(object sender, EventArgs e)
        {
            var customer = Customers
                .Where(c => c.Id == customerId)                
                .FirstOrDefault();
            var doc = customer.RemoveDocument(docId);
            var success = docRepository.Delete(doc);
            if (success)
            {
                if (MessageBox.Show("Are you sure to delete this Regard?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    MessageBox.Show(
                           DesktopResources.RemoveDocument_Successful_Message, DesktopResources.RemoveDocument_Successful_Message_Title,
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information);
                    ResetDocument();
                    LoadDgvDocument();
                }
            }
            else
            {
                MessageBox.Show(
                            DesktopResources.RemoveDocument_Error_Message, DesktopResources.RemoveDocument_Error_Message_Title,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Validates registration input
        /// </summary>
        /// <returns>true or false</returns>

        private void ResetRegistration()
        {
            LoadDgvCustomer(ref Customers);
            LoadDgvDocument(false);
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            cmbGender.SelectedIndex = -1;
            this.ActiveControl = txtFirstName;
            ResetDocument();
        }
        private void ResetDocument()
        {
            cmbTypeOfDocument.SelectedIndex = -1;
            txtDocumentNumber.Text = string.Empty;
            txtIssuingAuthority.Text = string.Empty;
            cmbCountry.SelectedIndex = -1;
        }



        /// <summary>
        /// Method to show general error message on any system level exception
        /// </summary>
        private void ShowErrorMessage(Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                DesktopResources.System_Error_Message_Title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        private void LoadData()
        {

            Customers = customerRepository.SelectAll();
        }
        private void LoadDgvCustomer(ref List<Customer> customers)
        {
            LoadData();
            dgvCustomers.DataSource = customers;
        }
        private void LoadDgvDocument(bool show = true)
        {
            var customer = show ? customerId : -1;

            LoadData();
            var documents = GetDocumentFromCustomer(customer);
            if (documents != null)
            {
                dgvDocuments.DataSource = documents;
            }
        }
        private void dataGridViewCustomers_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;


            try
            {
                if (dgv.SelectedRows.Count > 0)
                {
                    customerId = (int)dgv.SelectedRows[0].Cells[0].Value;

                    var customers = Customers;
                    foreach (var customer in customers)
                    {
                        if (customer.Id == customerId)
                        {
                            txtFirstName.Text = customer.FirstName;

                            txtLastName.Text = customer.LastName;

                            cmbGender.SelectedItem = customer.Gender;

                            dtDateOfBirth.Value = Convert.ToDateTime(customer.DateOfBirth);

                            txtEmail.Text = customer.Email;
                        }
                    }
                    ResetDocument();
                }

                LoadDgvDocument();
            }
            catch (Exception ex)
            {
                this.ShowErrorMessage(ex);
            }
        }
        private void dgvDocuments_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            try
            {
                if (dgv.SelectedRows.Count > 0)
                {
                    docId = (int)dgv.SelectedRows[0].Cells[0].Value;

                    var documents = GetDocumentFromCustomer(customerId);
                    foreach (var document in documents)
                    {
                        if (document.DocumentId == docId)
                        {
                            txtDocumentNumber.Text = document.DocumentNumber;
                            txtIssuingAuthority.Text = document.IssuingAuthority;
                            cmbTypeOfDocument.SelectedItem = document.TypeOfDocument;
                            dtDateOfIssue.Value = Convert.ToDateTime(document.DateOfIssue);
                            cmbCountry.SelectedItem = document.Country;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowErrorMessage(ex);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure to delete this Regard?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    var repository = new CustomerRepository();
                    bool success = repository.Delete(customerId);

                    if (success)
                    {
                        MessageBox.Show(
                            DesktopResources.Delete_Successful_Message,
                            DesktopResources.Delete_Successful_Message_Title,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        LoadDgvCustomer(ref Customers);
                        ResetRegistration();
                    }
                    else
                    {
                        MessageBox.Show(
                            DesktopResources.Delete_Error_Message,
                            DesktopResources.Delete_Error_Message_Title,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowErrorMessage(ex);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.ResetRegistration();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                var customer = Customers.Where(c => c.Id == customerId).FirstOrDefault();
                customer = customer.Edit(txtFirstName.Text, txtLastName.Text, txtEmail.Text, dtDateOfBirth.Value, cmbGender.SelectedValue.ToString());
                var success = customerRepository.Update(customer);

                if (success)
                {
                    LoadDgvCustomer(ref Customers);

                    MessageBox.Show(
                        DesktopResources.Update_Successful_Message,
                        DesktopResources.Update_Successful_Message_Title,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        DesktopResources.Update_Error_Message,
                        DesktopResources.Update_Error_Message_Title,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(
                        ex.Message,
                        DesktopResources.Registration_Error_Message_Title,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var repository = new CustomerRepository();
                var customers = repository.Search(txtFirstName.Text, txtLastName.Text, txtEmail.Text);

                LoadDgvCustomer(ref customers);
                LoadDgvDocument(false);
            }
            catch (Exception ex)
            {
                this.ShowErrorMessage(ex);
            }
        }
        private void FillCountryCombo()
        {
            var countries = new List<string>();
            countries.Add("-Please Select-");
            countries.AddRange(utilRepository.Select("Country"));
            cmbCountry.DataSource = countries;
        }
        private void FillGenderCombo()
        {
            var genders = new List<string>();
            genders.Add("-Please Select-");
            genders.AddRange(utilRepository.Select("Gender"));
            cmbGender.DataSource = genders;
        }
        private void Manage_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

       
    }
}
