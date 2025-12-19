
////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq; // Cần cho hàm FirstOrDefault khi Logout
using System.Windows.Forms;

namespace ASM_PP
{
    public partial class Admin : Form
    {
        // Cấu hình chuỗi kết nối
        SqlConnection conn;
        private const string connectionString = "server = AQUA\\SQLEXPRESS; database = PP_Location_DATA; Integrated Security=True";

        private int userId;

        // Biến để xác định Admin đang xem bảng nào (Complaint hay Recommend)
        private string currentMode = "";
        //Dùng để lưu tạm đánh giá của Recommend chờ khi tạo quán xong
        private int _pendingReRating = 0; // số sao tạm của recomment trong lúc chờ ad tạo mới nhà cung c
        private int _pendingReUserId = 0; // userId tạm của recomment trong lúc chờ ad tạo mới nhà cung cấp
        private string _pendingReNote = "";
        public Admin(int userId)
        {
            this.userId = userId;
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void Admin_Load(object sender, EventArgs e)
        {
            // Tải dữ liệu ban đầu cho Tab Provider
            LoadCategoriesToCombo(cbCategory);
            LoadAreasToCombo(cbArea);
            LoadProviders();

            // Tab Feedback để trống cho đến khi Admin chọn nút xem
        }

        // ====================================================================================
        // PHẦN 1: QUẢN LÝ PROVIDER (NHÀ CUNG CẤP/ĐỊA ĐIỂM)
        // ====================================================================================

        private void LoadCategoriesToCombo(ComboBox cb)
        {
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT CategoryId, CategoryName FROM ProviderCategories", conn))
            {
                var dt = new DataTable();
                using (var da = new SqlDataAdapter(cmd)) da.Fill(dt);
                cb.DisplayMember = "CategoryName";
                cb.ValueMember = "CategoryId";
                cb.DataSource = dt;
                cb.SelectedIndex = -1;
            }
        }

        private void LoadAreasToCombo(ComboBox cb)
        {
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT AreaId, AreaName FROM Areas", conn))
            {
                var dt = new DataTable();
                using (var da = new SqlDataAdapter(cmd)) da.Fill(dt);

                cb.DisplayMember = "AreaName";
                cb.ValueMember = "AreaId";
                cb.DataSource = dt;
                cb.SelectedIndex = -1;
            }
        }

        private void LoadProviders(string search = "")
        {
            // Tính toán điểm trung bình (Rating) trực tiếp từ bảng ProviderRatings
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(@"
            SELECT 
                p.ProviderId,
                p.Name,
                p.CategoryId,
                c.CategoryName,
                p.Address,
                p.Phone,
                p.Description,
                -- Tính trung bình cộng, nếu không có đánh giá nào thì trả về NULL
                NULLIF((
                    SELECT AVG(CAST(Rating AS FLOAT)) 
                    FROM ProviderRatings pr 
                    WHERE pr.ProviderId = p.ProviderId
                ), 0) AS AverageRating,
                p.Status,
                a.AreaName,
                p.AreaId
            FROM Providers p
            JOIN ProviderCategories c ON p.CategoryId = c.CategoryId
            LEFT JOIN Areas a ON p.AreaId = a.AreaId
            WHERE (@search = '' OR p.Name LIKE @k OR c.CategoryName LIKE @k OR p.Address LIKE @k)
             ORDER BY p.Name", conn))
            {
                cmd.Parameters.AddWithValue("@search", search ?? "");
                cmd.Parameters.AddWithValue("@k", "%" + (search ?? "") + "%");

                var dt = new DataTable();
                using (var da = new SqlDataAdapter(cmd)) da.Fill(dt);
                dgvProvider.DataSource = dt;

                // Ẩn các cột ID kỹ thuật
                if (dgvProvider.Columns.Contains("ProviderId")) dgvProvider.Columns["ProviderId"].Visible = false;
                if (dgvProvider.Columns.Contains("CategoryId")) dgvProvider.Columns["CategoryId"].Visible = false;
                if (dgvProvider.Columns.Contains("AreaId")) dgvProvider.Columns["AreaId"].Visible = false;

                // Định dạng hiển thị cột điểm trung bình
                if (dgvProvider.Columns.Contains("AverageRating"))
                {
                    dgvProvider.Columns["AverageRating"].HeaderText = "Avg Rating";
                    dgvProvider.Columns["AverageRating"].DefaultCellStyle.Format = "N1"; // 1 số thập phân
                    dgvProvider.Columns["AverageRating"].DefaultCellStyle.NullValue = "-";
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProviders(txtSearch.Text.Trim());
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_Provider_Name.Text) || string.IsNullOrWhiteSpace(cbCategory.Text))
            {
                MessageBox.Show("Please enter Provider Name and choose a Category.", "Warning");
                return;
            }

            // Lấy ID Category & Area
            int categoryId = GetOrCreateCategoryId(cbCategory.Text.Trim());
            int? areaId = null;
            if (!string.IsNullOrWhiteSpace(cbArea.Text)) areaId = GetOrCreateAreaId(cbArea.Text.Trim());

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // 1. INSERT PROVIDER và lấy ID mới vừa tạo
                    string insertProv = @"
                INSERT INTO Providers (Name, CategoryId, Address, Phone, Description, AreaId, CreatedBy, CreatedAt, Status)
                VALUES (@name, @cat, @addr, @phone, @desc, @area, @createdBy, GETDATE(), 'Active');
                SELECT CAST(SCOPE_IDENTITY() AS INT);"; // Lệnh này trả về ID mới

                    int newProviderId = 0;

                    using (var cmd = new SqlCommand(insertProv, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", txt_Provider_Name.Text.Trim());
                        cmd.Parameters.AddWithValue("@cat", categoryId);
                        cmd.Parameters.AddWithValue("@addr", txtAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@desc", txtdescription.Text.Trim());
                        cmd.Parameters.AddWithValue("@createdBy", userId);
                        cmd.Parameters.AddWithValue("@area", areaId.HasValue ? (object)areaId.Value : DBNull.Value);

                        // Thực thi và lấy ID
                        newProviderId = (int)cmd.ExecuteScalar();
                    }

                    // 2. TỰ ĐỘNG THÊM RATING (Nếu đây là quán được tạo từ Recommend)
                    if (newProviderId > 0 && _pendingReRating > 0)
                    {
                        string insertRating = @"
                    INSERT INTO ProviderRatings (ProviderId, UserId, Rating, Comment, CreatedAt)
                    VALUES (@pid, @uid, @rate, @cmt, GETDATE())";

                        using (var cmd = new SqlCommand(insertRating, conn))
                        {
                            cmd.Parameters.AddWithValue("@pid", newProviderId);
                            cmd.Parameters.AddWithValue("@uid", _pendingReUserId);
                            cmd.Parameters.AddWithValue("@rate", _pendingReRating);
                            cmd.Parameters.AddWithValue("@cmt", _pendingReNote + " (Initial Recommendation)");
                            cmd.ExecuteNonQuery();
                        }

                        // Reset biến tạm sau khi dùng xong
                        ResetPendingVars();
                    }
                }

                // Refresh UI
                LoadCategoriesToCombo(cbCategory);
                cbCategory.SelectedValue = categoryId;
                if (areaId.HasValue)
                {
                    LoadAreasToCombo(cbArea);
                    cbArea.SelectedValue = areaId.Value;
                }

                MessageBox.Show("Provider added successfully (with initial rating if applicable).", "Success");
                LoadProviders();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        // hàm update provider
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvProvider.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a Provider to edit.", "Warning");
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_Provider_Name.Text) || string.IsNullOrWhiteSpace(cbCategory.Text))
            {
                MessageBox.Show("Please enter Provider Name and choose a Category.", "Warning");
                return;
            }

            int providerId = Convert.ToInt32(dgvProvider.SelectedRows[0].Cells["ProviderId"].Value);

            // Resolve (or create) category and area IDs using existing helpers
            int categoryId = GetOrCreateCategoryId(cbCategory.Text.Trim());
            int? areaId = null;
            if (!string.IsNullOrWhiteSpace(cbArea.Text)) areaId = GetOrCreateAreaId(cbArea.Text.Trim());

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(@"
            UPDATE Providers
            SET Name = @name,
                CategoryId = @cat,
                Address = @addr,
                Phone = @phone,
                Description = @desc,
                AreaId = @area
            WHERE ProviderId = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@name", txt_Provider_Name.Text.Trim());
                    cmd.Parameters.AddWithValue("@cat", categoryId);
                    cmd.Parameters.AddWithValue("@addr", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@desc", txtdescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@area", areaId.HasValue ? (object)areaId.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@id", providerId);

                    conn.Open();
                    int affected = cmd.ExecuteNonQuery();

                    if (affected > 0)
                        MessageBox.Show("Provider updated successfully.", "Success");
                    else
                        MessageBox.Show("No provider was updated. It may have been removed.", "Info");
                }

                // Refresh UI
                LoadCategoriesToCombo(cbCategory);
                if (cbCategory.Items.Count > 0) cbCategory.SelectedValue = categoryId;
                if (areaId.HasValue)
                {
                    LoadAreasToCombo(cbArea);
                    cbArea.SelectedValue = areaId.Value;
                }
                LoadProviders();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        // Hàm reset biến tạm
        private void ResetPendingVars()
        {
            _pendingReRating = 0;
            _pendingReUserId = 0;
            _pendingReNote = "";
        }

        private void btnDelete_Provider_Click_1(object sender, EventArgs e)
        {
            if (dgvProvider.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a Provider to delete.", "Warning");
                return;
            }

            int providerId = Convert.ToInt32(dgvProvider.SelectedRows[0].Cells["ProviderId"].Value);
            var confirm = MessageBox.Show("Are you sure you want to delete this Provider?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("DELETE FROM Providers WHERE ProviderId = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", providerId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Provider deleted.", "Success");
                LoadProviders();
            }
            catch (SqlException ex)
            {
                // Thường lỗi do khóa ngoại (có rating hoặc feedback liên quan)
                MessageBox.Show("Cannot delete because data exists related to this provider (Feedback/Ratings).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txt_Provider_Name.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
            txtdescription.Clear();
            cbCategory.SelectedIndex = -1;
            cbArea.SelectedIndex = -1;
            txtSearch.Clear();

            // Reset biến tạm của Recommend
            ResetPendingVars();

            LoadProviders();
        }

        private void dgvProvider_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvProvider.Rows[e.RowIndex];

            txt_Provider_Name.Text = Convert.ToString(row.Cells["Name"].Value ?? "");
            txtAddress.Text = Convert.ToString(row.Cells["Address"].Value ?? "");
            txtPhone.Text = Convert.ToString(row.Cells["Phone"].Value ?? "");
            txtdescription.Text = Convert.ToString(row.Cells["Description"].Value ?? "");

            // Gán Category
            var catVal = row.Cells["CategoryId"].Value;
            if (catVal != null && int.TryParse(catVal.ToString(), out int catId))
                cbCategory.SelectedValue = catId;
            else
                cbCategory.SelectedIndex = -1;

            // Gán Area
            var areaVal = row.Cells["AreaId"].Value;
            if (areaVal != null && int.TryParse(areaVal.ToString(), out int areaId))
                cbArea.SelectedValue = areaId;
            else
                cbArea.SelectedIndex = -1;

            row.Selected = true;
        }

        // --- Helper Methods ---
        private int GetOrCreateCategoryId(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName)) throw new ArgumentException("Category name is required.");
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT CategoryId FROM ProviderCategories WHERE CategoryName = @name", conn))
                {
                    cmd.Parameters.AddWithValue("@name", categoryName);
                    var obj = cmd.ExecuteScalar();
                    if (obj != null) return Convert.ToInt32(obj);
                }
                using (var cmd = new SqlCommand("INSERT INTO ProviderCategories (CategoryName) VALUES (@name); SELECT CAST(SCOPE_IDENTITY() AS INT);", conn))
                {
                    cmd.Parameters.AddWithValue("@name", categoryName);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        private int GetOrCreateAreaId(string areaName)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT AreaId FROM Areas WHERE AreaName = @name", conn))
                {
                    cmd.Parameters.AddWithValue("@name", areaName);
                    var obj = cmd.ExecuteScalar();
                    if (obj != null) return Convert.ToInt32(obj);
                }
                using (var cmd = new SqlCommand("INSERT INTO Areas (AreaName) VALUES (@name); SELECT CAST(SCOPE_IDENTITY() AS INT);", conn))
                {
                    cmd.Parameters.AddWithValue("@name", areaName);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }


        // ====================================================================================
        // PHẦN 2: QUẢN LÝ FEEDBACK (COMPLAINT & RECOMMEND)
        // ====================================================================================

        private void btn_ViewComplaint_Click(object sender, EventArgs e)
        {
            currentMode = "Complaint";
            txtSearchFb.Clear();
            LoadComplaints();
        }

        private void btn_ViewRecommend_Click(object sender, EventArgs e)
        {
            currentMode = "Recommend";
            txtSearchFb.Clear();
            LoadRecommends();
        }

        // Tải bảng Feedbacks (Complain)
        private void LoadComplaints(string search = "")
        {
            using (var conn = new SqlConnection(connectionString))
            {
                // Thêm mệnh đề WHERE để tìm kiếm theo Tên người gửi, Tên quán, Tiêu đề hoặc Nội dung
                string query = @"
            SELECT 
                f.FeedbackID AS ID, 
                u.FullName AS Sender,
                p.Name AS ProviderName,
                f.FbTitle AS Title,
                f.FbNote AS Content,
                f.FbRating AS Rating,
                f.FbStatus AS Status,
                f.CreatedAt
            FROM Feedbacks f
            JOIN Users u ON f.UserID = u.UserId
            JOIN Providers p ON f.ProviderID = p.ProviderId
            WHERE (@k = '' OR u.FullName LIKE @k OR p.Name LIKE @k OR f.FbTitle LIKE @k OR f.FbNote LIKE @k)
            ORDER BY f.CreatedAt DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Nếu search rỗng thì @k = "", ngược lại là %keyword%
                    cmd.Parameters.AddWithValue("@k", string.IsNullOrEmpty(search) ? "" : "%" + search + "%");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgv_Feedback.DataSource = dt;
                }

                // Ẩn cột ID
                if (dgv_Feedback.Columns.Contains("ID")) dgv_Feedback.Columns["ID"].Visible = false;
            }
        }

        // Tải bảng Recommends
        private void LoadRecommends(string search = "")
        {
            using (var conn = new SqlConnection(connectionString))
            {
                // Sửa query: Lấy thẳng cột ReCategoryName
                string query = @"
            SELECT 
                r.RecommendID AS ID,
                u.FullName AS Sender,
                r.ReProvider AS ProviderName,
                r.ReAddress AS Address,
                r.ReCategoryName AS CategoryName, -- Lấy cột text này
                r.ReTitle AS Title,
                r.ReNote AS Content,
                r.ReRating AS Rating,
                r.ReStatus AS Status,
                r.CreatedAt
            FROM Recommends r
            JOIN Users u ON r.UserID = u.UserId
            WHERE (@k = '' OR u.FullName LIKE @k OR r.ReProvider LIKE @k OR r.ReCategoryName LIKE @k)
            ORDER BY r.CreatedAt DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@k", string.IsNullOrEmpty(search) ? "" : "%" + search + "%");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgv_Feedback.DataSource = dt;
                }

                if (dgv_Feedback.Columns.Contains("ID")) dgv_Feedback.Columns["ID"].Visible = false;
            }
        }

        //Duyệt (Approved)
        private void btn_Approved_Click(object sender, EventArgs e)
        {
            if (dgv_Feedback.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to approve.", "Warning");
                return;
            }

            int id = Convert.ToInt32(dgv_Feedback.SelectedRows[0].Cells["ID"].Value);

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // LOGIC 1: COMPLAINT (Quán đã có -> Thêm Rating ngay lập tức)
                    if (currentMode == "Complaint")
                    {
                        // B1: Update trạng thái Feedback
                        using (var cmd = new SqlCommand("UPDATE Feedbacks SET FbStatus = 'Approved' WHERE FeedbackID = @id", conn))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteNonQuery();
                        }

                        // B2: TỰ ĐỘNG THÊM VÀO BẢNG RATING
                        // Copy dữ liệu từ bảng Feedbacks sang ProviderRatings
                        string insertRating = @"
                    INSERT INTO ProviderRatings (ProviderId, UserId, Rating, Comment, CreatedAt)
                    SELECT ProviderID, UserID, FbRating, FbNote, GETDATE()
                    FROM Feedbacks
                    WHERE FeedbackID = @id";

                        using (var cmd = new SqlCommand(insertRating, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Complaint Approved. Rating has been added to the Provider.", "Success");
                        LoadComplaints(txtSearchFb.Text.Trim());
                    }

                    // LOGIC 2: RECOMMEND (Quán chưa có -> Lưu tạm thông tin -> Chuyển sang Tab Provider)
                    else if (currentMode == "Recommend")
                    {
                        // B1: Update trạng thái Recommends
                        using (var cmd = new SqlCommand("UPDATE Recommends SET ReStatus = 'Approved' WHERE RecommendID = @id", conn))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteNonQuery();
                        }

                        // B2: Lấy thông tin từ DB (để chắc chắn lấy đúng UserID và Rating)
                        string getInfo = "SELECT UserID, ReRating, ReNote, ReProvider, ReAddress, ReCategoryName FROM Recommends WHERE RecommendID = @id";
                        using (var cmd = new SqlCommand(getInfo, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    // LƯU VÀO BIẾN TẠM (Chờ bấm nút Add)
                                    _pendingReUserId = Convert.ToInt32(reader["UserID"]);
                                    _pendingReRating = Convert.ToInt32(reader["ReRating"]);
                                    _pendingReNote = reader["ReNote"].ToString();

                                    // Đẩy dữ liệu lên giao diện
                                    txt_Provider_Name.Text = reader["ReProvider"].ToString();
                                    txtAddress.Text = reader["ReAddress"].ToString();
                                    cbCategory.Text = reader["ReCategoryName"].ToString();
                                    txtdescription.Text = _pendingReNote;
                                }
                            }
                        }

                        // B3: Chuyển Tab
                        Provider_Management.SelectedTab = Provider;

                        // Clear các ô khác
                        txtPhone.Clear();
                        cbArea.SelectedIndex = -1;

                        MessageBox.Show("Approved! Please review info and click 'Add' to create the provider.\n(The rating will be applied automatically after creation).", "Info");
                        LoadRecommends(txtSearchFb.Text.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }



        // Xóa Feedback/Recommend
        private void btnDelete_Feedback_Click(object sender, EventArgs e)
        {
            if (dgv_Feedback.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to delete.", "Warning");
                return;
            }

            int id = Convert.ToInt32(dgv_Feedback.SelectedRows[0].Cells["ID"].Value);
            var confirm = MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "";

                    if (currentMode == "Complaint")
                        query = "DELETE FROM Feedbacks WHERE FeedbackID = @id";
                    else if (currentMode == "Recommend")
                        query = "DELETE FROM Recommends WHERE RecommendID = @id";
                    else return;

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Deleted successfully.", "Success");

                // Tải lại dữ liệu NHƯNG giữ nguyên từ khóa tìm kiếm hiện tại
                string currentSearch = txtSearchFb.Text.Trim();
                if (currentMode == "Complaint") LoadComplaints(currentSearch);
                else LoadRecommends(currentSearch);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Sự kiện click grid Feedback (không cần làm gì đặc biệt, nhưng giữ lại để tránh lỗi)
        private void dgv_Feedback_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Có thể thêm logic hiển thị chi tiết ra popup nếu text quá dài
        }

        // ====================================================================================
        // PHẦN 3: HỆ THỐNG (LOGOUT)
        // ====================================================================================

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
        private void txtSearchFb_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearchFb.Text.Trim();

            // Kiểm tra xem đang ở chế độ nào thì load lại bảng đó với từ khóa
            if (currentMode == "Complaint")
            {
                LoadComplaints(keyword);
            }
            else if (currentMode == "Recommend")
            {
                LoadRecommends(keyword);
            }
        }

        // Các event rỗng giữ lại để tránh lỗi Designer
        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e) { }
        private void cbArea_SelectedIndexChanged(object sender, EventArgs e) { }
        private void lbArea_Click(object sender, EventArgs e) { }
        private void txtdescription_TextChanged(object sender, EventArgs e) { }
        private void txtPhone_TextChanged(object sender, EventArgs e) { }
        private void txtAddress_TextChanged(object sender, EventArgs e) { }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
