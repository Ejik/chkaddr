namespace ACOT.CommonDialogsModule.Dialogs.ChoiceSubDivizion
{
    partial class ChoiceSubDivisionDialog
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
            this._listView = new System.Windows.Forms.ListView();
            this._CheckColumn = new System.Windows.Forms.ColumnHeader();
            this._MainColumn = new System.Windows.Forms.ColumnHeader();
            this._secondColumn = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // _listView
            // 
            this._listView.AllowColumnReorder = true;
            this._listView.CheckBoxes = true;
            this._listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._CheckColumn,
            this._MainColumn,
            this._secondColumn});
            this._listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._listView.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._listView.ForeColor = System.Drawing.SystemColors.WindowText;
            this._listView.FullRowSelect = true;
            this._listView.GridLines = true;
            this._listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this._listView.HideSelection = false;
            this._listView.Location = new System.Drawing.Point(0, 0);
            this._listView.Margin = new System.Windows.Forms.Padding(4);
            this._listView.Name = "_listView";
            this._listView.OwnerDraw = true;
            this._listView.Size = new System.Drawing.Size(527, 569);
            this._listView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this._listView.TabIndex = 1;
            this._listView.UseCompatibleStateImageBehavior = false;
            this._listView.View = System.Windows.Forms.View.Details;
            this._listView.VirtualMode = true;
            this._listView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this._listView_MouseDoubleClick);
            this._listView.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this._listView_DrawColumnHeader);
            this._listView.MouseClick += new System.Windows.Forms.MouseEventHandler(this._listView_MouseClick);
            this._listView.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this._listView_DrawItem);
            this._listView.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this._listView_RetrieveVirtualItem);
            this._listView.CacheVirtualItems += new System.Windows.Forms.CacheVirtualItemsEventHandler(this._listView_CacheVirtualItems);
            // 
            // _CheckColumn
            // 
            this._CheckColumn.Text = "";
            this._CheckColumn.Width = 50;
            // 
            // _MainColumn
            // 
            this._MainColumn.Text = "Наименование";
            this._MainColumn.Width = 396;
            // 
            // _secondColumn
            // 
            this._secondColumn.Text = "";
            this._secondColumn.Width = 64;
            // 
            // ChoiceSubDivisionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 569);
            this.Controls.Add(this._listView);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChoiceSubDivisionDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ChoiceSubDivisionDialog";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ChoiceSubDivisionDialog_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChoiceSubDivisionDialog_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ChoiceSubDivisionDialog_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView _listView;
        private System.Windows.Forms.ColumnHeader _CheckColumn;
        private System.Windows.Forms.ColumnHeader _MainColumn;
        private System.Windows.Forms.ColumnHeader _secondColumn;
    }
}