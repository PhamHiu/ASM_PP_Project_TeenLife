using BCrypt.Net;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Policy;
using System.Windows.Forms;

namespace ASM_PP
{
    public partial class Register : Form
    {
        // Chuỗi kết nối (nên tách ra class riêng sau này)
        private const string connectionString = "server = AQUA\\SQLEXPRESS; database = PP_Location_DATA; Integrated Security=True";

        public Register()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {

            // 1. Lấy dữ liệu từ giao diện
            // Lưu ý: Đảm bảo tên biến (txtUsername, txtPassword...) khớp với thiết kế của bạn
            string username = txtAccount.Text.Trim(); // Kiểm tra lại tên textbox bên Design
            string password = txtPassword.Text.Trim();
            string fullName = txtStudentName.Text.Trim(); // Kiểm tra lại tên textbox bên Design
            string email = txtEmail.Text.Trim();
            string studentCode = txtStudentCode.Text.Trim();


            // 2. Validate đầu vào (Kiểm tra dữ liệu trống)
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Please enter all required fields: Username, Password, and Full Name.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (username.Length <= 5) {
                MessageBox.Show("The username must be longer than 5 characters.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (username.Length <= 5)
            {
                MessageBox.Show("The Password must be longer than 5 characters.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // 3. Mã hóa mật khẩu (Hashing) - Quan trọng để bảo mật
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // 4. Kiểm tra xem Username đã tồn tại chưa
                    string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE Username = @user";
                    using (var cmdCheck = new SqlCommand(checkUserQuery, conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@user", username);
                        int count = (int)cmdCheck.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("Username already exists. Please choose another one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    // 5. Thực hiện Insert vào DB
                    // Mặc định Role là 'Student' và IsActive là 1
                    string insertQuery = @"
                        INSERT INTO Users (Username, PasswordHash, FullName, Email, Role, StudentCode, IsActive, CreatedAt)
                        VALUES (@user, @passHash, @name, @email, 'Student', @code, 1, GETDATE())";

                    using (var cmdInsert = new SqlCommand(insertQuery, conn))
                    {
                        cmdInsert.Parameters.AddWithValue("@user", username);
                        cmdInsert.Parameters.AddWithValue("@passHash", passwordHash);
                        cmdInsert.Parameters.AddWithValue("@name", fullName);
                        cmdInsert.Parameters.AddWithValue("@email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);
                        cmdInsert.Parameters.AddWithValue("@code", string.IsNullOrEmpty(studentCode) ? (object)DBNull.Value : studentCode);

                        int rowsAffected = cmdInsert.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Registration successful! You can login now.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close(); // Đóng form để quay về Login
                        }
                        else
                        {
                            MessageBox.Show("Registration failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("System Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        






        private void lblBackToLogin_Click(object sender, EventArgs e)
        {
            using (Login login = new Login())
            {
                this.Hide();
                login.ShowDialog();
                this.Show();

            }
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

        private void Register_Shown(object sender, EventArgs e)
        {
            
            ApplyPlaceholder(txtStudentName, "Full name ");
            ApplyPlaceholder(txtAccount, "Username");
            ApplyPlaceholder(txtPassword, "Password");
            ApplyPlaceholder(txtEmail, "Email");
            ApplyPlaceholder(txtStudentCode, "Student ID");

        }
    }
}
