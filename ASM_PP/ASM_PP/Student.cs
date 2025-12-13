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
    public partial class Student : Form
    {
        SqlConnection conn;
        private const string connectionString = "server = AQUA\\SQLEXPRESS; database = PP_Location_DATA; Integrated Security=True";

        private int userId;
        public Student(int id)
        {
            InitializeComponent();
            userId = id;
            conn = new SqlConnection(connectionString);

        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void Student_Load(object sender, EventArgs e)
        {
            LoadCategory();
            LoadProviders("");
            LoadAreas();
        }
        private void LoadCategory()
        {
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

        private void LoadAreas()
        {
            // if you have a cbArea control, populate it similarly
            // leave empty if not used
        }
        private int selectedProviderId = -1;
        private void LoadProviders(string keyword)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT 
                    p.Name,
                    c.CategoryName AS Category,
                    p.Address,
                    p.Phone,
                    ISNULL(
                        (SELECT AVG(Rating) FROM ProviderRatings WHERE ProviderId = p.ProviderId),
                        0
                    ) AS Rating,
                    p.Status
                FROM Providers p
                JOIN ProviderCategories c ON p.CategoryId = c.CategoryId
                WHERE 
                    p.Name LIKE @k 
                    OR c.CategoryName LIKE @k 
                    OR p.Address LIKE @k";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@k", "%" + keyword + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvProvider.DataSource = dt;

                // hide ProviderId column from user
                if (dgvProvider.Columns.Contains("ProviderId"))
                    dgvProvider.Columns["ProviderId"].Visible = false;
            }
        }



        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProviders(txtSearch.Text.Trim());
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Checklistboxrating_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txt_Recommend_complaint_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvProvider_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dgvProvider_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore header clicks
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvProvider.Rows[e.RowIndex];

            // Get ProviderId if present
            selectedProviderId = -1;
            if (dgvProvider.Columns.Contains("ProviderId"))
            {
                var idObj = row.Cells["ProviderId"].Value;
                int id;
                if (idObj != null && int.TryParse(idObj.ToString(), out id))
                    selectedProviderId = id;
            }
            // Safely read values (fall back to cell index if column name differs)
            string providerName = string.Empty;
            string categoryName = string.Empty;
            string address = string.Empty;

            // Try by column name first, then by indices
            if (dgvProvider.Columns.Contains("Name"))
                providerName = row.Cells["Name"].Value?.ToString() ?? string.Empty;
            else if (row.Cells.Count > 0)
                providerName = row.Cells[0].Value?.ToString() ?? string.Empty;

            if (dgvProvider.Columns.Contains("Category"))
                categoryName = row.Cells["Category"].Value?.ToString() ?? string.Empty;
            else if (row.Cells.Count > 1)
                categoryName = row.Cells[1].Value?.ToString() ?? string.Empty;

            if (dgvProvider.Columns.Contains("Address"))
                address = row.Cells["Address"].Value?.ToString() ?? string.Empty;
            else if (row.Cells.Count > 2)
                address = row.Cells[2].Value?.ToString() ?? string.Empty;

            // Populate controls
            txtProviderName.Text = providerName;
            txtAddress.Text = address;

            // Select matching category in cbCategory (DataBound to DataTable)
            bool matched = false;
            for (int i = 0; i < cbCategory.Items.Count; i++)
            {
                var drv = cbCategory.Items[i] as DataRowView;
                if (drv != null)
                {
                    var cat = drv["CategoryName"]?.ToString() ?? string.Empty;
                    if (string.Equals(cat, categoryName, StringComparison.OrdinalIgnoreCase))
                    {
                        cbCategory.SelectedIndex = i;
                        matched = true;
                        break;
                    }
                }
            }

            if (!matched)
                cbCategory.SelectedIndex = -1;

            // Allow using Recommend button to add rating for the selected provider
            btnRecommend.Enabled = true;
        }
        private void lbRecomment_Complaint_Click(object sender, EventArgs e)
        {

        }
        private void btnRecommend_Click(object sender, EventArgs e)
        {
            // If a provider is selected in the grid, add a rating
            if (selectedProviderId > 0)
            {
                // Get checked star (CheckedListBox uses zero-based indices for checked items)
                if (Checklistboxrating.CheckedIndices.Count == 0)
                {
                    MessageBox.Show("Bạn phải chọn số sao (1-5) để đánh giá.");
                    return;
                }

                int rating = Checklistboxrating.CheckedIndices[0] + 1; // 1..5
                string comment = txt_Recommend_complaint.Text.Trim(); // optional comment

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string insert = @"INSERT INTO ProviderRatings (ProviderId, UserId, Rating, Comment, CreatedAt)
                              VALUES (@pid, @uid, @rating, @comment, @created)";
                    using (SqlCommand cmd = new SqlCommand(insert, conn))
                    {
                        cmd.Parameters.AddWithValue("@pid", selectedProviderId);
                        cmd.Parameters.AddWithValue("@uid", userId);
                        cmd.Parameters.AddWithValue("@rating", rating);
                        cmd.Parameters.AddWithValue("@comment", string.IsNullOrEmpty(comment) ? (object)DBNull.Value : comment);
                        cmd.Parameters.AddWithValue("@created", DateTime.Now);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Đã gửi đánh giá thành công!");

                // refresh displayed providers to update avg rating
                LoadProviders(txtSearch.Text.Trim());

                // optionally clear rating selection and fields
                for (int i = 0; i < Checklistboxrating.Items.Count; i++)
                    Checklistboxrating.SetItemChecked(i, false);
                txt_Recommend_complaint.Clear();
                selectedProviderId = -1;
                dgvProvider.ClearSelection();
                btnRecommend.Enabled = true;
                return;
            }

            // If no provider is selected, fall back to original Recommend (insert Feedback)
            if (txtAddress.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập địa chỉ!");
                return;
            }
            if (cbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Bạn phải chọn danh mục!");
                return;
            }
            if (txt_Recommend_complaint.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập nội dung!");
                return;
            }

            string address = txtAddress.Text.Trim();
            int categoryId = Convert.ToInt32(cbCategory.SelectedValue);
            string content = txt_Recommend_complaint.Text.Trim();
            string title = "Đề xuất địa điểm mới";
            string providerName = txtProviderName.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Feedback 
                        (UserId, Type, Title, Content, 
                         SuggestedProviderName, SuggestedCategoryId, SuggestedAddress)
                         VALUES 
                        (@uid, 'Recommendation', @title, @content, @name, @catId, @address)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@content", content);
                cmd.Parameters.AddWithValue("@name", providerName);
                cmd.Parameters.AddWithValue("@catId", categoryId);
                cmd.Parameters.AddWithValue("@address", address);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            ClearFields();
            MessageBox.Show("Gửi Recommend thành công!");
        }
        

        private void btncomplaint_Click(object sender, EventArgs e)
        {
            if (txt_Recommend_complaint.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập nội dung!");
                return;
            }

            string content = txt_Recommend_complaint.Text.Trim();
            string title = "Complaint về địa điểm";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Feedback (UserId, Type, Title, Content)
                         VALUES (@uid, 'Complaint', @title, @content)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@content", content);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            ClearFields();
            MessageBox.Show("Gửi Complaint thành công!");
            

        }

        private void label1_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
        private void ClearFields()
        {

            txtAddress.Clear();
            txtProviderName.Clear();
            txtSearch.Clear();
            txt_Recommend_complaint.Clear();
            cbCategory.SelectedIndex = -1;
            LoadProviders("");
            btnRecommend.Enabled = true;
            if (Checklistboxrating != null)
            {
                Checklistboxrating.BeginUpdate();
                for (int i = 0; i < Checklistboxrating.Items.Count; i++)
                {
                    Checklistboxrating.SetItemChecked(i, false);
                }
                Checklistboxrating.ClearSelected();
                Checklistboxrating.EndUpdate();

                // Force a visual refresh
                Checklistboxrating.Refresh();
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearFields();
            

        }
    }
}
