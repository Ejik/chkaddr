namespace ACOT.ChkAddrModule.Views.AddrElementsSelectionView
{
    partial class AddrElementsSelectionView
    {
        /// <summary>
        /// The presenter used by this view.
        /// </summary>
        private ACOT.ChkAddrModule.Views.AddrElementsSelectionView.AddrElementsSelectionViewPresenter _presenter = null;

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
            if (_presenter != null)
                _presenter.Dispose();    // <===== This fires the cleanup code

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this._dataGridViewEx = new ACOT.ChkAddrModule.Interface.DataGridViewEx();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._statusStrip = new System.Windows.Forms.StatusStrip();
            this._StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this._dataGridViewEx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._bindingSource)).BeginInit();
            this._statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _dataGridViewEx
            // 
            this._dataGridViewEx.AllowUserToAddRows = false;
            this._dataGridViewEx.AllowUserToDeleteRows = false;
            this._dataGridViewEx.AllowUserToResizeRows = false;
            this._dataGridViewEx.AutoGenerateColumns = false;
            this._dataGridViewEx.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._dataGridViewEx.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._dataGridViewEx.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dataGridViewEx.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            this._dataGridViewEx.DataSource = this._bindingSource;
            this._dataGridViewEx.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dataGridViewEx.Location = new System.Drawing.Point(0, 0);
            this._dataGridViewEx.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._dataGridViewEx.MultiSelect = false;
            this._dataGridViewEx.Name = "_dataGridViewEx";
            this._dataGridViewEx.ReadOnly = true;
            this._dataGridViewEx.RowHeadersVisible = false;
            this._dataGridViewEx.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._dataGridViewEx.Size = new System.Drawing.Size(352, 493);
            this._dataGridViewEx.StandardTab = true;
            this._dataGridViewEx.TabIndex = 2;
            this._dataGridViewEx.VirtualMode = true;
            this._dataGridViewEx.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this._dataGridViewEx_CellDoubleClick);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column1.DataPropertyName = "NAME";
            this.Column1.HeaderText = "Наименование";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 122;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column2.DataPropertyName = "SOCR";
            this.Column2.HeaderText = "Сокр.";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 68;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "INDEX";
            this.Column3.HeaderText = "Индекс";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 80;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "CODE";
            this.Column4.HeaderText = "CODE";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Visible = false;
            // 
            // _statusStrip
            // 
            this._statusStrip.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._StatusLabel});
            this._statusStrip.Location = new System.Drawing.Point(0, 493);
            this._statusStrip.Name = "_statusStrip";
            this._statusStrip.Size = new System.Drawing.Size(352, 22);
            this._statusStrip.SizingGrip = false;
            this._statusStrip.TabIndex = 3;
            // 
            // _StatusLabel
            // 
            this._StatusLabel.Name = "_StatusLabel";
            this._StatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // AddrElementsSelectionView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._dataGridViewEx);
            this.Controls.Add(this._statusStrip);
            this.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "AddrElementsSelectionView";
            this.Size = new System.Drawing.Size(352, 515);
            ((System.ComponentModel.ISupportInitialize)(this._dataGridViewEx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._bindingSource)).EndInit();
            this._statusStrip.ResumeLayout(false);
            this._statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ACOT.ChkAddrModule.Interface.DataGridViewEx _dataGridViewEx;
        private System.Windows.Forms.BindingSource _bindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.StatusStrip _statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel _StatusLabel;
    }
}
