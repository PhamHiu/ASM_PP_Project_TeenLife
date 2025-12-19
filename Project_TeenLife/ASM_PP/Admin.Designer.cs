namespace ASM_PP
{
    partial class Admin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Provider_Management = new System.Windows.Forms.TabControl();
            this.Provider = new System.Windows.Forms.TabPage();
            this.btnLogout = new System.Windows.Forms.Button();
            this.cbArea = new System.Windows.Forms.ComboBox();
            this.lbArea = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnDelete_Provider = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.dgvProvider = new System.Windows.Forms.DataGridView();
            this.lbdescription = new System.Windows.Forms.Label();
            this.lbPhone = new System.Windows.Forms.Label();
            this.lbAddress = new System.Windows.Forms.Label();
            this.lbCategory = new System.Windows.Forms.Label();
            this.lbProvider_Name = new System.Windows.Forms.Label();
            this.lbSearch = new System.Windows.Forms.Label();
            this.cbCategory = new System.Windows.Forms.ComboBox();
            this.txtdescription = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txt_Provider_Name = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.Feedback = new System.Windows.Forms.TabPage();
            this.txtSearchFb = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Delete_Feedback = new System.Windows.Forms.Button();
            this.btn_Approved = new System.Windows.Forms.Button();
            this.btn_ViewComplaint = new System.Windows.Forms.Button();
            this.btn_ViewRecommend = new System.Windows.Forms.Button();
            this.dgv_Feedback = new System.Windows.Forms.DataGridView();
            this.Provider_Management.SuspendLayout();
            this.Provider.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProvider)).BeginInit();
            this.Feedback.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Feedback)).BeginInit();
            this.SuspendLayout();
            // 
            // Provider_Management
            // 
            this.Provider_Management.Controls.Add(this.Provider);
            this.Provider_Management.Controls.Add(this.Feedback);
            this.Provider_Management.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Provider_Management.Location = new System.Drawing.Point(0, 0);
            this.Provider_Management.Margin = new System.Windows.Forms.Padding(2);
            this.Provider_Management.Name = "Provider_Management";
            this.Provider_Management.SelectedIndex = 0;
            this.Provider_Management.Size = new System.Drawing.Size(616, 393);
            this.Provider_Management.TabIndex = 0;
            // 
            // Provider
            // 
            this.Provider.Controls.Add(this.btnLogout);
            this.Provider.Controls.Add(this.cbArea);
            this.Provider.Controls.Add(this.lbArea);
            this.Provider.Controls.Add(this.btnRefresh);
            this.Provider.Controls.Add(this.btnDelete_Provider);
            this.Provider.Controls.Add(this.btnEdit);
            this.Provider.Controls.Add(this.btnAdd);
            this.Provider.Controls.Add(this.dgvProvider);
            this.Provider.Controls.Add(this.lbdescription);
            this.Provider.Controls.Add(this.lbPhone);
            this.Provider.Controls.Add(this.lbAddress);
            this.Provider.Controls.Add(this.lbCategory);
            this.Provider.Controls.Add(this.lbProvider_Name);
            this.Provider.Controls.Add(this.lbSearch);
            this.Provider.Controls.Add(this.cbCategory);
            this.Provider.Controls.Add(this.txtdescription);
            this.Provider.Controls.Add(this.txtPhone);
            this.Provider.Controls.Add(this.txt_Provider_Name);
            this.Provider.Controls.Add(this.txtAddress);
            this.Provider.Controls.Add(this.txtSearch);
            this.Provider.Location = new System.Drawing.Point(4, 22);
            this.Provider.Margin = new System.Windows.Forms.Padding(2);
            this.Provider.Name = "Provider";
            this.Provider.Padding = new System.Windows.Forms.Padding(2);
            this.Provider.Size = new System.Drawing.Size(608, 367);
            this.Provider.TabIndex = 0;
            this.Provider.Text = "Provider";
            this.Provider.UseVisualStyleBackColor = true;
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(489, 328);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(2);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(60, 32);
            this.btnLogout.TabIndex = 19;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // cbArea
            // 
            this.cbArea.FormattingEnabled = true;
            this.cbArea.Location = new System.Drawing.Point(91, 114);
            this.cbArea.Margin = new System.Windows.Forms.Padding(2);
            this.cbArea.Name = "cbArea";
            this.cbArea.Size = new System.Drawing.Size(156, 21);
            this.cbArea.TabIndex = 18;
            this.cbArea.SelectedIndexChanged += new System.EventHandler(this.cbArea_SelectedIndexChanged);
            // 
            // lbArea
            // 
            this.lbArea.AutoSize = true;
            this.lbArea.Location = new System.Drawing.Point(13, 120);
            this.lbArea.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbArea.Name = "lbArea";
            this.lbArea.Size = new System.Drawing.Size(29, 13);
            this.lbArea.TabIndex = 17;
            this.lbArea.Text = "Area";
            this.lbArea.Click += new System.EventHandler(this.lbArea_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(386, 328);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(60, 32);
            this.btnRefresh.TabIndex = 16;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnDelete_Provider
            // 
            this.btnDelete_Provider.Location = new System.Drawing.Point(270, 328);
            this.btnDelete_Provider.Margin = new System.Windows.Forms.Padding(2);
            this.btnDelete_Provider.Name = "btnDelete_Provider";
            this.btnDelete_Provider.Size = new System.Drawing.Size(60, 32);
            this.btnDelete_Provider.TabIndex = 15;
            this.btnDelete_Provider.Text = "Delete";
            this.btnDelete_Provider.UseVisualStyleBackColor = true;
            this.btnDelete_Provider.Click += new System.EventHandler(this.btnDelete_Provider_Click_1);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(154, 328);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(2);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(60, 32);
            this.btnEdit.TabIndex = 14;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(44, 328);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(60, 32);
            this.btnAdd.TabIndex = 13;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dgvProvider
            // 
            this.dgvProvider.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProvider.Location = new System.Drawing.Point(2, 139);
            this.dgvProvider.Margin = new System.Windows.Forms.Padding(2);
            this.dgvProvider.Name = "dgvProvider";
            this.dgvProvider.RowHeadersWidth = 51;
            this.dgvProvider.RowTemplate.Height = 24;
            this.dgvProvider.Size = new System.Drawing.Size(604, 185);
            this.dgvProvider.TabIndex = 12;
            this.dgvProvider.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProvider_CellClick);
            // 
            // lbdescription
            // 
            this.lbdescription.AutoSize = true;
            this.lbdescription.Location = new System.Drawing.Point(298, 83);
            this.lbdescription.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbdescription.Name = "lbdescription";
            this.lbdescription.Size = new System.Drawing.Size(60, 13);
            this.lbdescription.TabIndex = 11;
            this.lbdescription.Text = "Description";
            // 
            // lbPhone
            // 
            this.lbPhone.AutoSize = true;
            this.lbPhone.Location = new System.Drawing.Point(296, 51);
            this.lbPhone.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbPhone.Name = "lbPhone";
            this.lbPhone.Size = new System.Drawing.Size(38, 13);
            this.lbPhone.TabIndex = 10;
            this.lbPhone.Text = "Phone";
            // 
            // lbAddress
            // 
            this.lbAddress.AutoSize = true;
            this.lbAddress.Location = new System.Drawing.Point(296, 20);
            this.lbAddress.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbAddress.Name = "lbAddress";
            this.lbAddress.Size = new System.Drawing.Size(45, 13);
            this.lbAddress.TabIndex = 9;
            this.lbAddress.Text = "Address";
            // 
            // lbCategory
            // 
            this.lbCategory.AutoSize = true;
            this.lbCategory.Location = new System.Drawing.Point(13, 86);
            this.lbCategory.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbCategory.Name = "lbCategory";
            this.lbCategory.Size = new System.Drawing.Size(49, 13);
            this.lbCategory.TabIndex = 8;
            this.lbCategory.Text = "Category";
            // 
            // lbProvider_Name
            // 
            this.lbProvider_Name.AutoSize = true;
            this.lbProvider_Name.Location = new System.Drawing.Point(13, 51);
            this.lbProvider_Name.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbProvider_Name.Name = "lbProvider_Name";
            this.lbProvider_Name.Size = new System.Drawing.Size(77, 13);
            this.lbProvider_Name.TabIndex = 7;
            this.lbProvider_Name.Text = "Provider Name";
            // 
            // lbSearch
            // 
            this.lbSearch.AutoSize = true;
            this.lbSearch.Location = new System.Drawing.Point(13, 20);
            this.lbSearch.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbSearch.Name = "lbSearch";
            this.lbSearch.Size = new System.Drawing.Size(41, 13);
            this.lbSearch.TabIndex = 6;
            this.lbSearch.Text = "Search";
            // 
            // cbCategory
            // 
            this.cbCategory.FormattingEnabled = true;
            this.cbCategory.Location = new System.Drawing.Point(91, 79);
            this.cbCategory.Margin = new System.Windows.Forms.Padding(2);
            this.cbCategory.Name = "cbCategory";
            this.cbCategory.Size = new System.Drawing.Size(156, 21);
            this.cbCategory.TabIndex = 5;
            this.cbCategory.SelectedIndexChanged += new System.EventHandler(this.cbCategory_SelectedIndexChanged);
            // 
            // txtdescription
            // 
            this.txtdescription.Location = new System.Drawing.Point(365, 79);
            this.txtdescription.Margin = new System.Windows.Forms.Padding(2);
            this.txtdescription.Name = "txtdescription";
            this.txtdescription.Size = new System.Drawing.Size(156, 20);
            this.txtdescription.TabIndex = 4;
            this.txtdescription.TextChanged += new System.EventHandler(this.txtdescription_TextChanged);
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(365, 47);
            this.txtPhone.Margin = new System.Windows.Forms.Padding(2);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(156, 20);
            this.txtPhone.TabIndex = 3;
            this.txtPhone.TextChanged += new System.EventHandler(this.txtPhone_TextChanged);
            // 
            // txt_Provider_Name
            // 
            this.txt_Provider_Name.Location = new System.Drawing.Point(91, 46);
            this.txt_Provider_Name.Margin = new System.Windows.Forms.Padding(2);
            this.txt_Provider_Name.Name = "txt_Provider_Name";
            this.txt_Provider_Name.Size = new System.Drawing.Size(156, 20);
            this.txt_Provider_Name.TabIndex = 2;
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(365, 15);
            this.txtAddress.Margin = new System.Windows.Forms.Padding(2);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(156, 20);
            this.txtAddress.TabIndex = 1;
            this.txtAddress.TextChanged += new System.EventHandler(this.txtAddress_TextChanged);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(91, 15);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(2);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(156, 20);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // Feedback
            // 
            this.Feedback.Controls.Add(this.txtSearchFb);
            this.Feedback.Controls.Add(this.label1);
            this.Feedback.Controls.Add(this.btn_Delete_Feedback);
            this.Feedback.Controls.Add(this.btn_Approved);
            this.Feedback.Controls.Add(this.btn_ViewComplaint);
            this.Feedback.Controls.Add(this.btn_ViewRecommend);
            this.Feedback.Controls.Add(this.dgv_Feedback);
            this.Feedback.Location = new System.Drawing.Point(4, 22);
            this.Feedback.Margin = new System.Windows.Forms.Padding(2);
            this.Feedback.Name = "Feedback";
            this.Feedback.Padding = new System.Windows.Forms.Padding(2);
            this.Feedback.Size = new System.Drawing.Size(608, 367);
            this.Feedback.TabIndex = 1;
            this.Feedback.Text = "Feedback";
            this.Feedback.UseVisualStyleBackColor = true;
            // 
            // txtSearchFb
            // 
            this.txtSearchFb.Location = new System.Drawing.Point(234, 16);
            this.txtSearchFb.Margin = new System.Windows.Forms.Padding(2);
            this.txtSearchFb.Name = "txtSearchFb";
            this.txtSearchFb.Size = new System.Drawing.Size(156, 20);
            this.txtSearchFb.TabIndex = 9;
            this.txtSearchFb.TextChanged += new System.EventHandler(this.txtSearchFb_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(188, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Search";
            // 
            // btn_Delete_Feedback
            // 
            this.btn_Delete_Feedback.Location = new System.Drawing.Point(469, 328);
            this.btn_Delete_Feedback.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Delete_Feedback.Name = "btn_Delete_Feedback";
            this.btn_Delete_Feedback.Size = new System.Drawing.Size(114, 32);
            this.btn_Delete_Feedback.TabIndex = 4;
            this.btn_Delete_Feedback.Text = "Delete";
            this.btn_Delete_Feedback.UseVisualStyleBackColor = true;
            this.btn_Delete_Feedback.Click += new System.EventHandler(this.btnDelete_Feedback_Click);
            // 
            // btn_Approved
            // 
            this.btn_Approved.Location = new System.Drawing.Point(319, 328);
            this.btn_Approved.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Approved.Name = "btn_Approved";
            this.btn_Approved.Size = new System.Drawing.Size(103, 32);
            this.btn_Approved.TabIndex = 3;
            this.btn_Approved.Text = "Approved";
            this.btn_Approved.UseVisualStyleBackColor = true;
            this.btn_Approved.Click += new System.EventHandler(this.btn_Approved_Click);
            // 
            // btn_ViewComplaint
            // 
            this.btn_ViewComplaint.Location = new System.Drawing.Point(167, 328);
            this.btn_ViewComplaint.Margin = new System.Windows.Forms.Padding(2);
            this.btn_ViewComplaint.Name = "btn_ViewComplaint";
            this.btn_ViewComplaint.Size = new System.Drawing.Size(96, 32);
            this.btn_ViewComplaint.TabIndex = 2;
            this.btn_ViewComplaint.Text = "View Complaint";
            this.btn_ViewComplaint.UseVisualStyleBackColor = true;
            this.btn_ViewComplaint.Click += new System.EventHandler(this.btn_ViewComplaint_Click);
            // 
            // btn_ViewRecommend
            // 
            this.btn_ViewRecommend.Location = new System.Drawing.Point(24, 328);
            this.btn_ViewRecommend.Margin = new System.Windows.Forms.Padding(2);
            this.btn_ViewRecommend.Name = "btn_ViewRecommend";
            this.btn_ViewRecommend.Size = new System.Drawing.Size(101, 32);
            this.btn_ViewRecommend.TabIndex = 1;
            this.btn_ViewRecommend.Text = "View Recommend";
            this.btn_ViewRecommend.UseVisualStyleBackColor = true;
            this.btn_ViewRecommend.Click += new System.EventHandler(this.btn_ViewRecommend_Click);
            // 
            // dgv_Feedback
            // 
            this.dgv_Feedback.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_Feedback.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Feedback.Location = new System.Drawing.Point(4, 49);
            this.dgv_Feedback.Margin = new System.Windows.Forms.Padding(2);
            this.dgv_Feedback.Name = "dgv_Feedback";
            this.dgv_Feedback.RowHeadersWidth = 51;
            this.dgv_Feedback.RowTemplate.Height = 24;
            this.dgv_Feedback.Size = new System.Drawing.Size(597, 275);
            this.dgv_Feedback.TabIndex = 0;
            this.dgv_Feedback.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_Feedback_CellClick);
            // 
            // Admin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 393);
            this.Controls.Add(this.Provider_Management);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Admin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Admin";
            this.Load += new System.EventHandler(this.Admin_Load);
            this.Provider_Management.ResumeLayout(false);
            this.Provider.ResumeLayout(false);
            this.Provider.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProvider)).EndInit();
            this.Feedback.ResumeLayout(false);
            this.Feedback.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Feedback)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl Provider_Management;
        private System.Windows.Forms.TabPage Provider;
        private System.Windows.Forms.TabPage Feedback;
        private System.Windows.Forms.Button btn_Delete_Feedback;
        private System.Windows.Forms.Button btn_Approved;
        private System.Windows.Forms.Button btn_ViewComplaint;
        private System.Windows.Forms.Button btn_ViewRecommend;
        private System.Windows.Forms.DataGridView dgv_Feedback;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lbSearch;
        private System.Windows.Forms.ComboBox cbCategory;
        private System.Windows.Forms.TextBox txtdescription;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txt_Provider_Name;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lbProvider_Name;
        private System.Windows.Forms.Label lbCategory;
        private System.Windows.Forms.Label lbAddress;
        private System.Windows.Forms.Label lbdescription;
        private System.Windows.Forms.Label lbPhone;
        private System.Windows.Forms.DataGridView dgvProvider;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnDelete_Provider;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ComboBox cbArea;
        private System.Windows.Forms.Label lbArea;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSearchFb;
    }
}