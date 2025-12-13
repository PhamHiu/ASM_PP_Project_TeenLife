namespace ASM_PP
{
    partial class Student
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
            this.lbSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lbCategory = new System.Windows.Forms.Label();
            this.cbCategory = new System.Windows.Forms.ComboBox();
            this.dgvProvider = new System.Windows.Forms.DataGridView();
            this.txt_Recommend_complaint = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRecommend = new System.Windows.Forms.Button();
            this.btncomplaint = new System.Windows.Forms.Button();
            this.Checklistboxrating = new System.Windows.Forms.CheckedListBox();
            this.lbRecomment_Complaint = new System.Windows.Forms.Label();
            this.txtProviderName = new System.Windows.Forms.TextBox();
            this.ProviderName = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // lbSearch
            // 
            this.lbSearch.AutoSize = true;
            this.lbSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSearch.Location = new System.Drawing.Point(225, 129);
            this.lbSearch.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbSearch.Name = "lbSearch";
            this.lbSearch.Size = new System.Drawing.Size(53, 17);
            this.lbSearch.TabIndex = 0;
            this.lbSearch.Text = "Search";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(282, 129);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(153, 20);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lbCategory
            // 
            this.lbCategory.AutoSize = true;
            this.lbCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCategory.Location = new System.Drawing.Point(12, 94);
            this.lbCategory.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbCategory.Name = "lbCategory";
            this.lbCategory.Size = new System.Drawing.Size(65, 17);
            this.lbCategory.TabIndex = 2;
            this.lbCategory.Text = "Category";
            // 
            // cbCategory
            // 
            this.cbCategory.FormattingEnabled = true;
            this.cbCategory.Location = new System.Drawing.Point(117, 95);
            this.cbCategory.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbCategory.Name = "cbCategory";
            this.cbCategory.Size = new System.Drawing.Size(153, 21);
            this.cbCategory.TabIndex = 3;
            this.cbCategory.SelectedIndexChanged += new System.EventHandler(this.cbCategory_SelectedIndexChanged);
            // 
            // dgvProvider
            // 
            this.dgvProvider.AllowUserToOrderColumns = true;
            this.dgvProvider.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProvider.Location = new System.Drawing.Point(26, 154);
            this.dgvProvider.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgvProvider.Name = "dgvProvider";
            this.dgvProvider.RowHeadersWidth = 51;
            this.dgvProvider.RowTemplate.Height = 24;
            this.dgvProvider.Size = new System.Drawing.Size(610, 197);
            this.dgvProvider.TabIndex = 4;
            this.dgvProvider.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProvider_CellClick);
            // 
            // txt_Recommend_complaint
            // 
            this.txt_Recommend_complaint.AcceptsReturn = true;
            this.txt_Recommend_complaint.Location = new System.Drawing.Point(336, 17);
            this.txt_Recommend_complaint.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txt_Recommend_complaint.Multiline = true;
            this.txt_Recommend_complaint.Name = "txt_Recommend_complaint";
            this.txt_Recommend_complaint.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_Recommend_complaint.Size = new System.Drawing.Size(175, 99);
            this.txt_Recommend_complaint.TabIndex = 5;
            this.txt_Recommend_complaint.TextChanged += new System.EventHandler(this.txt_Recommend_complaint_TextChanged);
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(117, 55);
            this.txtAddress.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(153, 20);
            this.txtAddress.TabIndex = 7;
            this.txtAddress.TextChanged += new System.EventHandler(this.txtAddress_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 55);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "Address";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btnRecommend
            // 
            this.btnRecommend.Location = new System.Drawing.Point(128, 366);
            this.btnRecommend.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnRecommend.Name = "btnRecommend";
            this.btnRecommend.Size = new System.Drawing.Size(76, 32);
            this.btnRecommend.TabIndex = 9;
            this.btnRecommend.Text = "Recommend";
            this.btnRecommend.UseVisualStyleBackColor = true;
            this.btnRecommend.Click += new System.EventHandler(this.btnRecommend_Click);
            // 
            // btncomplaint
            // 
            this.btncomplaint.Location = new System.Drawing.Point(289, 365);
            this.btncomplaint.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btncomplaint.Name = "btncomplaint";
            this.btncomplaint.Size = new System.Drawing.Size(82, 32);
            this.btncomplaint.TabIndex = 10;
            this.btncomplaint.Text = "Complaint";
            this.btncomplaint.UseVisualStyleBackColor = true;
            this.btncomplaint.Click += new System.EventHandler(this.btncomplaint_Click);
            // 
            // Checklistboxrating
            // 
            this.Checklistboxrating.Cursor = System.Windows.Forms.Cursors.Cross;
            this.Checklistboxrating.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Checklistboxrating.FormattingEnabled = true;
            this.Checklistboxrating.Items.AddRange(new object[] {
            "⭐ ",
            "⭐ ⭐",
            "⭐ ⭐⭐ ",
            "⭐ ⭐⭐ ⭐",
            "⭐ ⭐⭐ ⭐⭐ "});
            this.Checklistboxrating.Location = new System.Drawing.Point(546, 17);
            this.Checklistboxrating.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Checklistboxrating.Name = "Checklistboxrating";
            this.Checklistboxrating.Size = new System.Drawing.Size(90, 99);
            this.Checklistboxrating.TabIndex = 11;
            this.Checklistboxrating.SelectedIndexChanged += new System.EventHandler(this.Checklistboxrating_SelectedIndexChanged);
            // 
            // lbRecomment_Complaint
            // 
            this.lbRecomment_Complaint.AutoSize = true;
            this.lbRecomment_Complaint.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F);
            this.lbRecomment_Complaint.Location = new System.Drawing.Point(286, 15);
            this.lbRecomment_Complaint.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbRecomment_Complaint.Name = "lbRecomment_Complaint";
            this.lbRecomment_Complaint.Size = new System.Drawing.Size(46, 17);
            this.lbRecomment_Complaint.TabIndex = 12;
            this.lbRecomment_Complaint.Text = "Note: ";
            this.lbRecomment_Complaint.Click += new System.EventHandler(this.lbRecomment_Complaint_Click);
            // 
            // txtProviderName
            // 
            this.txtProviderName.Location = new System.Drawing.Point(117, 15);
            this.txtProviderName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtProviderName.Name = "txtProviderName";
            this.txtProviderName.Size = new System.Drawing.Size(153, 20);
            this.txtProviderName.TabIndex = 13;
            // 
            // ProviderName
            // 
            this.ProviderName.AutoSize = true;
            this.ProviderName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F);
            this.ProviderName.Location = new System.Drawing.Point(13, 17);
            this.ProviderName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ProviderName.Name = "ProviderName";
            this.ProviderName.Size = new System.Drawing.Size(102, 17);
            this.ProviderName.TabIndex = 14;
            this.ProviderName.Text = "Provider Name";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(466, 365);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 32);
            this.btnRefresh.TabIndex = 17;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // Student
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 408);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.ProviderName);
            this.Controls.Add(this.txtProviderName);
            this.Controls.Add(this.lbRecomment_Complaint);
            this.Controls.Add(this.Checklistboxrating);
            this.Controls.Add(this.btncomplaint);
            this.Controls.Add(this.btnRecommend);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.txt_Recommend_complaint);
            this.Controls.Add(this.dgvProvider);
            this.Controls.Add(this.cbCategory);
            this.Controls.Add(this.lbCategory);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.lbSearch);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Student";
            this.Text = "Student";
            this.Load += new System.EventHandler(this.Student_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lbCategory;
        private System.Windows.Forms.ComboBox cbCategory;
        private System.Windows.Forms.DataGridView dgvProvider;
        private System.Windows.Forms.TextBox txt_Recommend_complaint;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRecommend;
        private System.Windows.Forms.Button btncomplaint;
        private System.Windows.Forms.CheckedListBox Checklistboxrating;
        private System.Windows.Forms.Label lbRecomment_Complaint;
        private System.Windows.Forms.TextBox txtProviderName;
        private System.Windows.Forms.Label ProviderName;
        private System.Windows.Forms.Button btnRefresh;
    }
}