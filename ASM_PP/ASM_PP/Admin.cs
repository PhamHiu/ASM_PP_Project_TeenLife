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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace ASM_PP
{

    public partial class Admin : Form
    {
        SqlConnection conn;
        private const string connectionString = "server = AQUA\\SQLEXPRESS; database = PP_Location_DATA; Integrated Security=True";

        private int userId;


        public Admin(int userId)
        {
            this.userId = userId;
            InitializeComponent();
            conn = new SqlConnection(connectionString);

        }
        private void Admin_Load(object sender, EventArgs e)
        {
            LoadCategoriesToCombo(cbCategory);
            LoadAreasToCombo(cbArea);
            LoadProviders();
            LoadFeedback();

        }

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
                cb.SelectedIndex = -1; // không chọn gì ban đầu
            }
        }
        private void LoadProviders(string search = "")
        {
            // Calculate average rating from ProviderRatings (handles cases where Providers table doesn't maintain AverageRating)
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
                -- compute average rating from ProviderRatings; NULL if no ratings
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

                // hide ids
                if (dgvProvider.Columns.Contains("ProviderId"))
                    dgvProvider.Columns["ProviderId"].Visible = false;
                if (dgvProvider.Columns.Contains("CategoryId"))
                    dgvProvider.Columns["CategoryId"].Visible = false;
                if (dgvProvider.Columns.Contains("AreaId"))
                    dgvProvider.Columns["AreaId"].Visible = false;

                // Ensure AverageRating is visible and formatted
                if (dgvProvider.Columns.Contains("AverageRating"))
                {
                    dgvProvider.Columns["AverageRating"].HeaderText = "Avg Rating";
                    dgvProvider.Columns["AverageRating"].DefaultCellStyle.Format = "N1";
                    dgvProvider.Columns["AverageRating"].DefaultCellStyle.NullValue = "";
                    dgvProvider.Columns["AverageRating"].Visible = true;
                }
            }

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProviders(txtSearch.Text.Trim());
        }

        private void btn_ViewRecommend_Click(object sender, EventArgs e)
        {
            LoadFeedback("Recommendation");
        }

        private void btn_ViewComplaint_Click(object sender, EventArgs e)
        {
            LoadFeedback("Complaint");
        }

        private void btn_Approved_Click(object sender, EventArgs e)
        {
            if (dgv_Feedback.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn Feedback để duyệt.");
                return;
            }

            int feedbackId = Convert.ToInt32(dgv_Feedback.SelectedRows[0].Cells["FeedbackId"].Value);

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("UPDATE Feedback SET Status = 'Approved', ProcessedBy = @admin WHERE FeedbackId = @id", conn))
            {
                cmd.Parameters.AddWithValue("@admin", userId);
                cmd.Parameters.AddWithValue("@id", feedbackId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Đã duyệt feedback.");
            LoadFeedback();

        }



        private void btnAdd_Click(object sender, EventArgs e)
        {
            // allow user to type a new category/area in the combo and save it automatically
            if (string.IsNullOrWhiteSpace(txt_Provider_Name.Text) || string.IsNullOrWhiteSpace(cbCategory.Text))
            {
                MessageBox.Show("Vui lòng nhập tên provider và nhập/chọn danh mục.");
                return;
            }

            // get or create category id
            int categoryId = GetOrCreateCategoryId(cbCategory.Text.Trim());

            // get or create area id (area is optional)
            int? areaId = null;
            if (!string.IsNullOrWhiteSpace(cbArea.Text))
            {
                areaId = GetOrCreateAreaId(cbArea.Text.Trim());
            }

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(@"
                INSERT INTO Providers (Name, CategoryId, Address, Phone, Description, AreaId, CreatedBy, CreatedAt, Status)
                VALUES (@name,@cat,@addr,@phone,@desc,@area,@createdBy,GETDATE(),'Active')", conn))
            {
                cmd.Parameters.AddWithValue("@name", txt_Provider_Name.Text.Trim());
                cmd.Parameters.AddWithValue("@cat", categoryId);
                cmd.Parameters.AddWithValue("@addr", txtAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());
                cmd.Parameters.AddWithValue("@desc", txtdescription.Text.Trim());
                cmd.Parameters.AddWithValue("@createdBy", userId);
                cmd.Parameters.AddWithValue("@area", areaId.HasValue ? (object)areaId.Value : DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // refresh combos so new items appear and select them
            LoadCategoriesToCombo(cbCategory);
            cbCategory.SelectedValue = categoryId;
            if (areaId.HasValue)
            {
                LoadAreasToCombo(cbArea);
                cbArea.SelectedValue = areaId.Value;
            }

            MessageBox.Show("Thêm Provider thành công.");
            LoadProviders();

        }

        /// <summary>
        /// Returns existing CategoryId for name or inserts and returns new id.
        /// </summary>
        private int GetOrCreateCategoryId(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName)) throw new ArgumentException(nameof(categoryName));

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT CategoryId FROM ProviderCategories WHERE CategoryName = @name", conn))
            {
                cmd.Parameters.AddWithValue("@name", categoryName);
                conn.Open();
                var obj = cmd.ExecuteScalar();
                if (obj != null && obj != DBNull.Value)
                    return Convert.ToInt32(obj);
            }

            // not found => insert
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("INSERT INTO ProviderCategories (CategoryName) VALUES (@name); SELECT CAST(SCOPE_IDENTITY() AS INT);", conn))
            {
                cmd.Parameters.AddWithValue("@name", categoryName);
                conn.Open();
                var id = cmd.ExecuteScalar();
                return Convert.ToInt32(id);
            }
        }

        /// <summary>
        /// Returns existing AreaId for name or inserts and returns new id.
        /// </summary>
        private int GetOrCreateAreaId(string areaName)
        {
            if (string.IsNullOrWhiteSpace(areaName)) throw new ArgumentException(nameof(areaName));

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT AreaId FROM Areas WHERE AreaName = @name", conn))
            {
                cmd.Parameters.AddWithValue("@name", areaName);
                conn.Open();
                var obj = cmd.ExecuteScalar();
                if (obj != null && obj != DBNull.Value)
                    return Convert.ToInt32(obj);
            }

            // not found => insert
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("INSERT INTO Areas (AreaName) VALUES (@name); SELECT CAST(SCOPE_IDENTITY() AS INT);", conn))
            {
                cmd.Parameters.AddWithValue("@name", areaName);
                conn.Open();
                var id = cmd.ExecuteScalar();
                return Convert.ToInt32(id);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

            if (dgvProvider.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn Provider để sửa.");
                return;
            }

            int providerId = Convert.ToInt32(dgvProvider.SelectedRows[0].Cells["ProviderId"].Value);
            int categoryId = Convert.ToInt32(cbCategory.SelectedValue);


            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(@"
                UPDATE Providers SET Name=@name, CategoryId=@cat, Address=@addr, Phone=@phone, Description=@desc, AreaId=@area
                WHERE ProviderId=@id", conn))
            {
                cmd.Parameters.AddWithValue("@name", txt_Provider_Name.Text.Trim());
                cmd.Parameters.AddWithValue("@cat", categoryId);
                cmd.Parameters.AddWithValue("@addr", txtAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());
                cmd.Parameters.AddWithValue("@desc", txtdescription.Text.Trim());

                cmd.Parameters.AddWithValue("@id", providerId);
                cmd.Parameters.AddWithValue("@area",
                cbArea.SelectedIndex == -1 ? DBNull.Value : cbArea.SelectedValue);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Cập nhật thành công.");
            LoadProviders();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txt_Provider_Name.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
            txtdescription.Clear();

            cbCategory.SelectedIndex = -1;
            cbArea.SelectedIndex = -1;
            // Clear the search box so LoadProviders shows all entries without filter
            txtSearch.Clear();
            LoadProviders();
        }

        private void txtdescription_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnDelete_Provider_Click_1(object sender, EventArgs e)
        {

            if (dgvProvider.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn Provider để xóa.");
                return;
            }

            int providerId = Convert.ToInt32(dgvProvider.SelectedRows[0].Cells["ProviderId"].Value);
            var confirm = MessageBox.Show("Bạn chắc chắn muốn xóa nhà cung cấp này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes) return;

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("DELETE FROM Providers WHERE ProviderId = @id", conn))
            {
                cmd.Parameters.AddWithValue("@id", providerId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Xóa thành công.");
            LoadProviders();
        }

        private void btnDelete_Feedback_Click(object sender, EventArgs e)
        {
            if (dgv_Feedback.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn feedback để xóa.");
                return;
            }

            int feedbackId = Convert.ToInt32(dgv_Feedback.SelectedRows[0].Cells["FeedbackId"].Value);
            var confirm = MessageBox.Show("Xác nhận xóa feedback?", "Xác nhận", MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes) return;

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("DELETE FROM Feedback WHERE FeedbackId = @id", conn))
            {
                cmd.Parameters.AddWithValue("@id", feedbackId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Đã xóa feedback.");
            LoadFeedback();

        }
        private void LoadFeedback(string filter = "")
        {
            // This query returns feedback rows and, if the student also left a rating in ProviderRatings,
            // pulls the most recent rating by that student for that provider.
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(@"
                SELECT 
                    f.FeedbackId,
                    f.ProviderId,
                    COALESCE(p.Name, f.SuggestedProviderName) AS ProviderName,
                    f.UserId,
                    u.FullName AS Sender,
                    f.Type,
                    f.Title,
                    f.Content,
                    f.Status,
                    f.CreatedAt,
                    f.ProcessedBy,
                    -- if provider and user exist, pick the most recent rating this user gave that provider (nullable)
                    (SELECT TOP 1 pr.Rating 
                     FROM ProviderRatings pr 
                     WHERE pr.ProviderId = f.ProviderId AND pr.UserId = f.UserId 
                     ORDER BY pr.CreatedAt DESC) AS Rating
                FROM Feedback f
                LEFT JOIN Providers p ON f.ProviderId = p.ProviderId
                LEFT JOIN Users u ON f.UserId = u.UserId
                WHERE (@filter IS NULL OR f.Type = @filter)
                ORDER BY f.CreatedAt DESC", conn))
            {
                // pass null when no filter to allow the SQL to match the IS NULL check
                cmd.Parameters.AddWithValue("@filter", string.IsNullOrEmpty(filter) ? (object)DBNull.Value : filter);

                var dt = new DataTable();
                using (var da = new SqlDataAdapter(cmd)) da.Fill(dt);
                dgv_Feedback.DataSource = dt;

                // hide technical id columns
                if (dgv_Feedback.Columns.Contains("FeedbackId")) dgv_Feedback.Columns["FeedbackId"].Visible = false;
                if (dgv_Feedback.Columns.Contains("ProviderId")) dgv_Feedback.Columns["ProviderId"].Visible = false;
                if (dgv_Feedback.Columns.Contains("UserId")) dgv_Feedback.Columns["UserId"].Visible = false;
                if (dgv_Feedback.Columns.Contains("ProcessedBy")) dgv_Feedback.Columns["ProcessedBy"].Visible = false;

                // set friendly headers / formatting
                if (dgv_Feedback.Columns.Contains("ProviderName")) dgv_Feedback.Columns["ProviderName"].HeaderText = "Provider";
                if (dgv_Feedback.Columns.Contains("Sender")) dgv_Feedback.Columns["Sender"].HeaderText = "From";
                if (dgv_Feedback.Columns.Contains("CreatedAt")) dgv_Feedback.Columns["CreatedAt"].HeaderText = "Date";
                if (dgv_Feedback.Columns.Contains("Rating"))
                {
                    dgv_Feedback.Columns["Rating"].HeaderText = "Rating";
                    dgv_Feedback.Columns["Rating"].DefaultCellStyle.NullValue = "";
                }
            }
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lbArea_Click(object sender, EventArgs e)
        {

        }

        private void cbArea_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dgvProvider_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // ignore header clicks
            if (e.RowIndex < 0) return;

            var row = dgvProvider.Rows[e.RowIndex];

            txt_Provider_Name.Text = Convert.ToString(row.Cells["Name"].Value ?? "");
            txtAddress.Text = Convert.ToString(row.Cells["Address"].Value ?? "");
            txtPhone.Text = Convert.ToString(row.Cells["Phone"].Value ?? "");
            txtdescription.Text = Convert.ToString(row.Cells["Description"].Value ?? "");

            // Category (may be hidden column)
            var catVal = row.Cells["CategoryId"].Value;
            if (catVal == null || catVal == DBNull.Value)
                cbCategory.SelectedIndex = -1;
            else
            {
                int catId;
                if (int.TryParse(catVal.ToString(), out catId))
                    cbCategory.SelectedValue = catId;
                else
                    cbCategory.SelectedIndex = -1;
            }

            // Area (may be null)
            var areaVal = row.Cells["AreaId"].Value;
            if (areaVal == null || areaVal == DBNull.Value)
                cbArea.SelectedIndex = -1;
            else
            {
                int areaId;
                if (int.TryParse(areaVal.ToString(), out areaId))
                    cbArea.SelectedValue = areaId;
                else
                    cbArea.SelectedIndex = -1;
            }

            // ensure the clicked row is selected
            row.Selected = true;
        }

        private void dgv_Feedback_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            // Try to reuse any existing hidden Login form (Login hid itself on successful login).
            var existingLogin = Application.OpenForms.OfType<Login>().FirstOrDefault();
            if (existingLogin != null)
            {
                // Ensure UI operations happen on the Login form's thread
                try
                {
                    existingLogin.ResetForLogout();
                    existingLogin.Show();
                    existingLogin.BringToFront();
                }
                catch
                {
                    // fallback: create a fresh login if anything goes wrong
                    var login = new Login();
                    login.ResetForLogout();
                    login.Show();
                }
            }
            else
            {
                // No existing Login instance found — create a fresh one.
                var login = new Login();
                login.ResetForLogout();
                login.Show();
            }

            // Close this Admin form to return to login screen
            this.Close();
        }
    }
}
