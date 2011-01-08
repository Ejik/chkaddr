using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ACOT.ChkAddrModule.Interface;
using ACOT.Infrastructure.Interface;
using ACOT.Infrastructure.Interface.Constants;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace ACOT.ChkAddrModule.Views.AddrElementsSelectionView
{
    public partial class AddrElementsSelectionView : UserControl, IAddrElementsSelectionView// ISmartPartInfoProvider
    {
        public AddrElementsSelectionView()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            _presenter.OnViewReady();
            base.OnLoad(e);
        }

        #region ISmartPartInfoProvider Members

        //public ISmartPartInfo GetSmartPartInfo(Type smartPartInfoType)
        //{
        //    ISmartPartInfo spi;
        //    if (smartPartInfoType.IsAssignableFrom(typeof(WindowSmartPartInfo)))
        //    {
        //        WindowSmartPartInfo LayoutView = new WindowSmartPartInfo();
        //        LayoutView.MaximizeBox = false;
        //        LayoutView.MinimizeBox = false;

        //        LayoutView.Keys[WindowWorkspaceSetting.FormShowIcon] = false;
        //        //LayoutView.Keys[WindowWorkspaceSetting.AutoSize] = true;
        //        LayoutView.Keys[WindowWorkspaceSetting.TopMost] = true;
        //        LayoutView.Keys[WindowWorkspaceSetting.KeyPreview] = true;
        //        LayoutView.Keys[WindowWorkspaceSetting.KeyDown] = new KeyEventHandler(View_KeyDown);
        //        spi = LayoutView;
        //    }
        //    else
        //    {
        //        spi = Activator.CreateInstance(smartPartInfoType) as ISmartPartInfo;
        //    }

        //    //spi.Description = Properties.Resources.FindCustomerResultsViewDescription;
        //    spi.Title = "Адресный классификатор";

        //    return spi;
        //}

        private void View_KeyDown(object sender, KeyEventArgs e)
        {
           if (e.KeyData == Keys.Escape)
               _presenter.OnCloseView();
            if (e.KeyData == Keys.Enter)
                _presenter.OnSelectRow(_dataGridViewEx.SelectedRows[0]);
        }

        #endregion

        #region IAddrElementsSelectionView Members

        public Form ParentFm
        {
            get { return this.ParentForm; }
        }
        public void SetPosition(int left)
        {
            this.ParentFm.Location = new Point(left, Screen.PrimaryScreen.Bounds.Height/2 - this.ParentForm.Height/2);
        }

        public DataGridViewEx WorkArea {
            get { return this._dataGridViewEx; }
            set { this._dataGridViewEx = value; }
        }

        public BindingSource BindingSrc 
        { 
            get { return this._bindingSource; }
            set
            {
                _bindingSource = value;
                _dataGridViewEx.DataSource = value;
            }
        }

        public ToolStripStatusLabel Status
        {
            get { return _StatusLabel; }
        }

        #endregion

        private void _dataGridViewEx_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 )
            _presenter.OnSelectRow(_dataGridViewEx.Rows[e.RowIndex]);
        }
    }
}
