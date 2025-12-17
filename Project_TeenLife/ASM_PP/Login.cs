using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using BCrypt.Net;
using System.Drawing;


namespace ASM_PP
{
    public partial class Login : Form
    {
        private const string connectionString = "server = AQUA\\SQLEXPRESS; database = PP_Location_DATA; Integrated Security=True";

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // 1. Kiểm tra đầu vào
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both Username and Password.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // 2. Tìm user theo Username (không so sánh pass ở đây)
                    string query = @"SELECT UserId, PasswordHash, Role, FullName 
                                     FROM Users 
                                     WHERE Username = @user AND IsActive = 1";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", username);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // 3. Lấy thông tin user từ DB
                                int userId = Convert.ToInt32(reader["UserId"]);
                                string dbHash = reader["PasswordHash"].ToString();
                                string role = reader["Role"].ToString();
                                string fullName = reader["FullName"].ToString();
                                // 4. Xác thực mật khẩu bằng BCrypt
                                bool isPasswordCorrect = false;
                                try
                                {
                                    isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, dbHash);
                                }
                                catch
                                {
                                    isPasswordCorrect = false;
                                }

                                if (isPasswordCorrect)
                                {
                                    MessageBox.Show($"Welcome, {fullName}!", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    // 5. Chuyển hướng dựa trên Role
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
                                    else
                                    {
                                        MessageBox.Show("Invalid Role configuration.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
                                {
                                    // Mật khẩu sai
                                    MessageBox.Show("Invalid Password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                // Không tìm thấy Username
                                MessageBox.Show("Username does not exist.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            Register frmRegister = new Register();
            this.Hide();
            frmRegister.ShowDialog();
            this.Show();
            ResetForLogout();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to exít!", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                { Application.Exit();}
        }

        public void ResetForLogout()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtUsername.Focus();
        }
        // làm mờ text khi chứa nhập và hiện thị lại khi có nhập
        // khi dùng chỉ cần truyền vào giá trị TextBox và Placeholder
        //cách dùng: chỉ cần gọi trong formload ApplyPlaceholder(TextBoxA, "nội dung")
        private void ApplyPlaceholder(TextBox tb, string placeholder)
        {
            // Lưu placeholder vào Tag
            tb.Tag = placeholder;

            // Set mặc định
            tb.Text = placeholder;
            tb.ForeColor = Color.Gray;

            // Khi click vào
            tb.Enter += (s, e) =>
            {
                if (tb.ForeColor == Color.Gray)
                {
                    tb.Text = "";
                    tb.ForeColor = Color.Black;
                }
            };

            // Khi rời textbox
            tb.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.Text = tb.Tag.ToString();
                    tb.ForeColor = Color.Gray;
                }
            };
        }
        private void txtUsername_TextChanged(object sender, EventArgs e) { }
        private void txtPassword_TextChanged(object sender, EventArgs e) { }
        private void Login_Load(object sender, EventArgs e) {}

        private void Login_Shown(object sender, EventArgs e)
        {
            ApplyPlaceholder(txtUsername, "Username");
            ApplyPlaceholder(txtPassword, "Password");
            this.ActiveControl = null;
        }
    }

}