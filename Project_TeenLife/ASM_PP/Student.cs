using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ASM_PP
{
    public partial class Student : Form
    {
        SqlConnection conn;
        private const string connectionString = "server = AQUA\\SQLEXPRESS; database = PP_Location_DATA; Integrated Security=True";

        private int userId;
        private int selectedProviderId = -1; // Biến lưu ID quán đang chọn
        private bool isViewDetailMode = false;
        public Student(int id)
        {
            InitializeComponent();
            userId = id;
            conn = new SqlConnection(connectionString);
            
        }

        private void Student_Load(object sender, EventArgs e)
        {
            LoadCategory();
            LoadProviders("");
        }

        private void LoadCategory()
        {
            // Quan trọng: Cho phép người dùng nhập chữ vào ComboBox (DropDown)
            cbCategory.DropDownStyle = ComboBoxStyle.DropDown;

            cbCategory.Items.Clear();
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT CategoryId, CategoryName FROM ProviderCategories", conn))
            {
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(rdr);
                    cbCategory.DisplayMember = "CategoryName";
                    cbCategory.ValueMember = "CategoryId";
                    cbCategory.DataSource = dt;
                }
            }
            cbCategory.SelectedIndex = -1;
        }

        private void LoadProviders(string keyword)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT 
                    p.ProviderId,
                    p.Name,
                    c.CategoryName AS Category,
                    p.Address,
                    p.Phone,
                    ISNULL(
                        (SELECT AVG(CAST(Rating AS DECIMAL(3,2))) FROM ProviderRatings WHERE ProviderId = p.ProviderId),
                        0
                    ) AS Rating,
                    p.Status
                FROM Providers p
                JOIN ProviderCategories c ON p.CategoryId = c.CategoryId
                WHERE 
                    (p.Name LIKE @k OR c.CategoryName LIKE @k OR p.Address LIKE @k)
                    AND p.Status = 'Active'";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@k", "%" + keyword + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvProvider.DataSource = dt;

                if (dgvProvider.Columns.Contains("ProviderId"))
                    dgvProvider.Columns["ProviderId"].Visible = false;

                if (dgvProvider.Columns.Contains("Rating"))
                    dgvProvider.Columns["Rating"].DefaultCellStyle.Format = "N1";
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProviders(txtSearch.Text.Trim());
        }

        // Sự kiện click vào bảng -> Đổ dữ liệu vào textbox
        private void dgvProvider_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Nếu đang ở chế độ xem Feedback thì không cho click để điền dữ liệu
            if (isViewDetailMode) return;
            // ------------------------------------

            if (e.RowIndex < 0) return;


            DataGridViewRow row = dgvProvider.Rows[e.RowIndex];

            // 1. Lưu ProviderId
            if (dgvProvider.Columns.Contains("ProviderId"))
            {
                var idObj = row.Cells["ProviderId"].Value;
                if (idObj != null && int.TryParse(idObj.ToString(), out int id))
                    selectedProviderId = id;
            }

            // 2. Hiển thị thông tin lên TextBox để người dùng biết mình đang chọn quán nào
            txtProviderName.Text = row.Cells["Name"].Value?.ToString() ?? "";
            txtAddress.Text = row.Cells["Address"].Value?.ToString() ?? "";

            // 3. Hiển thị Category
            string categoryName = row.Cells["Category"].Value?.ToString() ?? "";
            cbCategory.SelectedIndex = -1;
            cbCategory.Text = categoryName; // Gán text trực tiếp cho trường hợp DropDown
        }

        private int GetSelectedRating()
        {
            if (rdo1.Checked) return 1;
            if (rdo2.Checked) return 2;
            if (rdo3.Checked) return 3;
            if (rdo4.Checked) return 4;
            if (rdo5.Checked) return 5;
            return 0;
        }

        // HÀM HỖ TRỢ: Lấy ID danh mục hoặc Tạo mới nếu chưa có
        private int GetOrCreateCategoryId(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName)) return -1;

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // 1. Kiểm tra xem tên danh mục đã có chưa
                using (var cmd = new SqlCommand("SELECT CategoryId FROM ProviderCategories WHERE CategoryName = @name", conn))
                {
                    cmd.Parameters.AddWithValue("@name", categoryName);
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result); // Đã có -> Trả về ID
                    }
                }

                // 2. Chưa có -> Tạo mới
                using (var cmd = new SqlCommand("INSERT INTO ProviderCategories (CategoryName) VALUES (@name); SELECT SCOPE_IDENTITY();", conn))
                {
                    cmd.Parameters.AddWithValue("@name", categoryName);
                    return Convert.ToInt32(cmd.ExecuteScalar()); // Trả về ID vừa tạo
                }
            }
        }

        // =========================================================
        // LOGIC 1: COMPLAINT (Khiếu nại quán cũ)
        // =========================================================
        private void btncomplaint_Click(object sender, EventArgs e)
        {
            // Bắt buộc phải chọn quán từ lưới trước
            if (selectedProviderId == -1)
            {
                MessageBox.Show("Please select a provider from the list to complain about.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Bắt buộc nhập Title và Note
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txt_Recommend_complaint.Text))
            {
                MessageBox.Show("Please enter a Title and Note content for your complaint.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Sửa Status -> FbStatus
                    string query = @"INSERT INTO Feedbacks 
                             (UserID, ProviderID, FbTitle, FbNote, FbRating, FbStatus, CreatedAt)
                             VALUES 
                             (@uid, @pid, @title, @note, @rating, 'Pending', GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // ... (Parameters giữ nguyên) ...
                        cmd.Parameters.AddWithValue("@uid", userId);
                        cmd.Parameters.AddWithValue("@pid", selectedProviderId);
                        cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
                        cmd.Parameters.AddWithValue("@note", txt_Recommend_complaint.Text.Trim());
                        cmd.Parameters.AddWithValue("@rating", GetSelectedRating()); // Helper lấy sao

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Complaint sent successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // =========================================================
        // LOGIC 2: RECOMMEND (Đề xuất quán mới + Đánh giá sao)
        // =========================================================
        //private void btnRecommend_Click(object sender, EventArgs e)
        //{
        //    // 1. Validate: Phải nhập đầy đủ thông tin
        //    if (string.IsNullOrWhiteSpace(txtProviderName.Text) ||
        //        string.IsNullOrWhiteSpace(txtAddress.Text) ||
        //        string.IsNullOrWhiteSpace(cbCategory.Text) ||
        //        string.IsNullOrWhiteSpace(txtTitle.Text) ||   // Title
        //        string.IsNullOrWhiteSpace(txt_Recommend_complaint.Text)) // Note
        //    {
        //        MessageBox.Show("Please fill in all fields (Provider Name, Address, Category, Title, Note) to recommend.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    // 2. Validate: Phải chọn sao đánh giá
        //    int rating = GetSelectedRating();
        //    if (rating == 0)
        //    {
        //        MessageBox.Show("Please select a star rating for this new place.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    try
        //    {
        //        // Xử lý Category...
        //        string catNameInput = cbCategory.Text.Trim();
        //        int finalCategoryId = GetOrCreateCategoryId(catNameInput);

        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            // Sửa Status -> ReStatus
        //            string query = @"INSERT INTO Recommends 
        //                     (UserID, ReProvider, ReAddress, ReCategoryId, ReTitle, ReNote, ReRating, ReStatus, CreatedAt)
        //                     VALUES 
        //                     (@uid, @name, @addr, @cat, @title, @note, @rating, 'Pending', GETDATE())";

        //            using (SqlCommand cmd = new SqlCommand(query, conn))
        //            {
        //                // ... (Parameters giữ nguyên) ...
        //                cmd.Parameters.AddWithValue("@uid", userId);
        //                cmd.Parameters.AddWithValue("@name", txtProviderName.Text.Trim());
        //                cmd.Parameters.AddWithValue("@addr", txtAddress.Text.Trim());
        //                cmd.Parameters.AddWithValue("@cat", finalCategoryId);
        //                cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
        //                cmd.Parameters.AddWithValue("@note", txt_Recommend_complaint.Text.Trim());
        //                cmd.Parameters.AddWithValue("@rating", GetSelectedRating());

        //                conn.Open();
        //                cmd.ExecuteNonQuery();
        //            }
        //        }
        //        MessageBox.Show("Recommendation sent successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        ClearFields();
        //        LoadCategory();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error: " + ex.Message);
        //    }
        //}
        private void btnRecommend_Click(object sender, EventArgs e)
        {
            // 1. Validate (Giữ nguyên)
            if (string.IsNullOrWhiteSpace(txtProviderName.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text) ||
                string.IsNullOrWhiteSpace(cbCategory.Text) ||
                string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(txt_Recommend_complaint.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int rating = GetSelectedRating();
            if (rating == 0)
            {
                MessageBox.Show("Please select a star rating.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Lấy thẳng Text người dùng nhập (hoặc chọn), KHÔNG tạo ID ở đây nữa
                string catName = cbCategory.Text.Trim();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Sửa câu lệnh INSERT: dùng ReCategoryName thay vì ReCategoryId
                    string query = @"INSERT INTO Recommends 
                     (UserID, ReProvider, ReAddress, ReCategoryName, ReTitle, ReNote, ReRating, ReStatus, CreatedAt)
                     VALUES 
                     (@uid, @name, @addr, @catName, @title, @note, @rating, 'Pending', GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@uid", userId);
                        cmd.Parameters.AddWithValue("@name", txtProviderName.Text.Trim());
                        cmd.Parameters.AddWithValue("@addr", txtAddress.Text.Trim());

                        // Lưu text
                        cmd.Parameters.AddWithValue("@catName", catName);

                        cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
                        cmd.Parameters.AddWithValue("@note", txt_Recommend_complaint.Text.Trim());
                        cmd.Parameters.AddWithValue("@rating", rating);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Recommendation sent successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            // RESET CỜ VỀ FALSE ĐỂ TRỞ LẠI BÌNH THƯỜNG
            isViewDetailMode = false;


            txtProviderName.Clear();
            txtAddress.Clear();
            txtSearch.Clear();
            txt_Recommend_complaint.Clear();
            txtTitle.Clear();

            cbCategory.SelectedIndex = -1;
            cbCategory.Text = ""; // Xóa cả text nhập tay

            rdo1.Checked = false;
            rdo2.Checked = false;
            rdo3.Checked = false;
            rdo4.Checked = false;
            rdo5.Checked = false;

            selectedProviderId = -1;
            dgvProvider.ClearSelection();
            LoadProviders("");
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txt_Recommend_complaint_TextChanged(object sender, EventArgs e) { }
        private void lbRecomment_Complaint_Click(object sender, EventArgs e) { }        
        private void txtAddress_TextChanged(object sender, EventArgs e) { }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            // Tìm form Login cũ đang ẩn
            var loginForm = Application.OpenForms.OfType<Login>().FirstOrDefault();
            if (loginForm != null)
            {
                loginForm.ResetForLogout();
                loginForm.Show();
            }
            else
            {
                // Nếu không tìm thấy, tạo mới
                new Login().Show();
            }

            this.Close();
        }

        private void btnViewDetail_Click(object sender, EventArgs e)
        {
           // =========================================================
           // LOGIC 3: XEM CHI TIẾT FEEDBACK (Hiển thị ngay trên dgvProvider)
           // =========================================================

        
            // 1. Validate: Phải chọn quán trước
            if (selectedProviderId == -1)
            {
                MessageBox.Show("Please select a provider from the list first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // 2. Truy vấn lấy Feedback ĐÃ DUYỆT (Approved) của quán đó
                    string query = @"
                SELECT 
                    u.FullName AS [User],
                    f.FbTitle AS [Title],
                    f.FbNote AS [Content],
                    f.FbRating AS [Rating],
                    f.CreatedAt AS [Date]
                FROM Feedbacks f
                JOIN Users u ON f.UserID = u.UserId
                WHERE f.ProviderID = @pid 
                  AND f.FbStatus = 'Approved' 
                ORDER BY f.CreatedAt DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@pid", selectedProviderId);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        // 3. Thay thế dữ liệu trên GridView hiện tại
                        dgvProvider.DataSource = dt;

                        // 4. Format lại cột cho đẹp (vì cột đã thay đổi so với Provider)
                        if (dgvProvider.Columns.Contains("Rating"))
                        {
                            dgvProvider.Columns["Rating"].Width = 50;
                            dgvProvider.Columns["Rating"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }
                        if (dgvProvider.Columns.Contains("Content"))
                        {
                            dgvProvider.Columns["Content"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Cột nội dung tự giãn
                        }

                        // 5. Thông báo nếu không có dữ liệu
                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("This provider has no approved reviews yet.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        // SAU KHI ĐỔ DỮ LIỆU THÀNH CÔNG -> BẬT CỜ LÊN
                        isViewDetailMode = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    

}