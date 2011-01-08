using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ACOT.ChkAddrModule.Interface;
using ACOT.ChkAddrModule.Views.AddrElementsSelectionView;
using System.Data;

namespace ACOT.ChkAddrModule.Tests.Mocks
{
    class MockAddrElementsSelectionView : Control, IAddrElementsSelectionView
    {
        private object _dataSource;
        private BindingSource _bindingSource;
        private DataTable _table;
        private DataGridViewEx _dataGridViewEx;

        public MockAddrElementsSelectionView()
        {
            _table = new DataTable("MockTable");
            DataColumn column;
            DataRow row;

            // Create new DataColumn, set DataType, ColumnName
            // and add to DataTable.    
            column = new DataColumn("NAME", Type.GetType("System.String"));
            _table.Columns.Add(column);

            column = new DataColumn("INDEX", Type.GetType("System.String"));
            _table.Columns.Add(column);

            object[] str1 =
                            {
                                "Ярославская 1",
                                "12345"
                            };

            _table.Rows.Add(str1);

            object[] str2 = {
                                "Ярославская 2",
                                "56786"
                            };
            _table.Rows.Add(str2);

            object[] str3 = {
                                "Ярославская",
                                "56786"
                            };
            _table.Rows.Add(str3);
            
            
            _bindingSource = new BindingSource(_table, "");

            _dataGridViewEx = new DataGridViewEx();
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            _dataGridViewEx.Columns.Add("NAME", "Наименование");
            _dataGridViewEx.Columns.Add("INDEX", "Индекс");

            _dataGridViewEx.Columns[0].DataPropertyName = "NAME";
            _dataGridViewEx.Columns[1].DataPropertyName = "INDEX";

            _dataGridViewEx.DataSource = _bindingSource;
            _bindingSource.DataSource = _table;
        }

        #region IAddrElementsSelectionView Members

        public Form ParentFm
        {
            get { throw new NotImplementedException(); }
        }

        public void SetPosition(int left)
        {
            throw new NotImplementedException();
        }

        public ACOT.ChkAddrModule.Interface.DataGridViewEx WorkArea
        {
            get { return _dataGridViewEx; }
        }

        public BindingSource BindingSrc
        {
            get
            {
                return _bindingSource;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ToolStripStatusLabel Status
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
