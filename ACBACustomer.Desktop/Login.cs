using ACBACustomer.Data.DataAccess;
using ACBACustomer.Desktop.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ACBACustomer.Desktop
{
    public partial class Login : Form
    {
        public Login()
        {
            this.InitializeComponent();
            this.InitializeResourceString();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
        }

        /// <summary>
        /// Initializes resource strings
        /// </summary>
        private void InitializeResourceString()
        {
            lblUserName.Text = DesktopResources.Login_Username_Label_Text;
            lblPassword.Text = DesktopResources.Login_Password_Label_Text;
            btnLogin.Text = DesktopResources.Login_Login_Button_Text;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //Passwords and Hashing

            string queryLog = "SELECT UserID FROM [ACBAUser] WHERE LoginName = @pLoginName And PasswordHash=HASHBYTES('SHA2_512', @pPassword+CAST(Salt AS NVARCHAR(36)))";

            SqlConnection connection = new SqlConnection(CustomerAccess.ConnectionString);

            using (SqlCommand command = new SqlCommand(queryLog, connection))
            {

                command.Parameters.AddWithValue("@pLoginName", txtUsername.Text);
                command.Parameters.AddWithValue("@pPassword", txtPassword.Text);

                connection.Open();
                var result = command.ExecuteScalar();
                connection.Close();

                if (result != null)
                {
                    var frmManage = new Manage();
                    frmManage.Show();
                    this.Hide();
                }
                else 
                {
                    MessageBox.Show(
                    DesktopResources.Login_Validation_Message,
                    DesktopResources.Login_Validation_Message_Title,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                }
            }
        }
    }
}
