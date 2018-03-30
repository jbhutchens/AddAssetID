namespace AddAssetIDTable
{
    partial class AddAssetTable
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddAssetTable));
            this.txtServer = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgAssetTable = new System.Windows.Forms.DataGridView();
            this.submitButton = new System.Windows.Forms.Button();
            this.cboDbs = new System.Windows.Forms.ComboBox();
            this.cboTable = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgAssetTable)).BeginInit();
            this.SuspendLayout();
            // 
            // txtServer
            // 
            this.txtServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtServer.Location = new System.Drawing.Point(147, 22);
            this.txtServer.Multiline = false;
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(220, 35);
            this.txtServer.TabIndex = 0;
            this.txtServer.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(29, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "SQL Server:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "SQL Database:";
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(414, 22);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(155, 65);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Get Data";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dgAssetTable
            // 
            this.dgAssetTable.AllowUserToOrderColumns = true;
            this.dgAssetTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgAssetTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgAssetTable.Location = new System.Drawing.Point(13, 159);
            this.dgAssetTable.Name = "dgAssetTable";
            this.dgAssetTable.Size = new System.Drawing.Size(731, 366);
            this.dgAssetTable.TabIndex = 4; 
            this.dgAssetTable.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgAssetTable_CellValueChanged);
            this.dgAssetTable.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgAssetTable_DefaultValuesNeeded); 
            this.dgAssetTable.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgAssetTable_UserDeletedRow);
            // 
            // submitButton
            // 
            this.submitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.submitButton.Location = new System.Drawing.Point(590, 22);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(155, 65);
            this.submitButton.TabIndex = 5;
            this.submitButton.Text = "Save Changes";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // cboDbs
            // 
            this.cboDbs.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboDbs.FormattingEnabled = true;
            this.cboDbs.Location = new System.Drawing.Point(147, 63);
            this.cboDbs.Name = "cboDbs";
            this.cboDbs.Size = new System.Drawing.Size(220, 32);
            this.cboDbs.TabIndex = 1;
            this.cboDbs.Enter += new System.EventHandler(this.cboDbs_Enter);
            // 
            // cboTable
            // 
            this.cboTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTable.FormattingEnabled = true;
            this.cboTable.Location = new System.Drawing.Point(147, 108);
            this.cboTable.Name = "cboTable";
            this.cboTable.Size = new System.Drawing.Size(220, 32);
            this.cboTable.TabIndex = 2; 
            this.cboTable.Enter += new System.EventHandler(this.cboTable_Enter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(36, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 24);
            this.label3.TabIndex = 6;
            this.label3.Text = "SQL Table:";
            // 
            // AddAssetTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 528);
            this.Controls.Add(this.cboTable);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboDbs);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.dgAssetTable);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtServer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddAssetTable";
            this.Text = "Add Asset Table";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddAssetTable_FormClosing);        
            ((System.ComponentModel.ISupportInitialize)(this.dgAssetTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgAssetTable;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.ComboBox cboDbs;
        private System.Windows.Forms.ComboBox cboTable;
        private System.Windows.Forms.Label label3;
    }
}