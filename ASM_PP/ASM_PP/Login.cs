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

namespace ASM_PP
{
    public partial class Login : Form
    {
        SqlConnection conn;
        private const string connectionString = "server = AQUA\\SQLEXPRESS; database = PP_Location_DATA; Integrated Security=True";

        public Login()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (username == "" || password == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Username và Password!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT UserId, Username, PasswordHash, Role 
                                 FROM Users 
                                 WHERE Username = @user AND PasswordHash = @pass AND IsActive = 1";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@user", username);
                cmd.Parameters.AddWithValue("@pass", password);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string role = reader["Role"].ToString();
                    int userId = Convert.ToInt32(reader["UserId"]);

                    MessageBox.Show("Đăng nhập thành công!");

                    // Chuyển form theo Role
                    if (role == "Admin")
                    {
                        Admin f = new Admin(userId);
                        f.Show();
                        this.Hide();
                    }
                    else if (role == "Student")
                    {
                        Student f = new Student(userId);
                        f.Show();
                        this.Hide();
                    }
                }
                else
                {
                    MessageBox.Show("Sai Username hoặc Password!");
                }
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        // Add this public helper inside the Login class (paste anywhere inside the class body)
        public void ResetForLogout()
        {
            // Clear credentials and set focus to username
            if (this.InvokeRequired)
            {
                this.Invoke((Action)(() => {
                    txtUsername.Clear();
                    txtPassword.Clear();
                    txtUsername.Focus();
                }));
            }
            else
            {
                txtUsername.Clear();
                txtPassword.Clear();
                txtUsername.Focus();
            }
        }
    }
}
