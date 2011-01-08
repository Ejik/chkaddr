using System;
using System.Data;
using ACOT.ChkAddrModule.Constants;
using ACOT.ChkAddrModule.Interface.BusinessEntities;
using ACOT.ChkAddrModule.Interface.Services;
using ACOT.ChkAddrModule.Services;
using ACOT.Infrastructure.Interface;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.ObjectBuilder;


namespace ACOT.ChkAddrModule.Views.AddrElementsSelectionView
{
    public partial class AddrElementsSelectionViewPresenter : ACOT.Infrastructure.Interface.Presenter<IAddrElementsSelectionView>
    {
        private ACOT.ChkAddrModule.Interface.Services.ICheckAddressService _chkAddrSrv;
        private ACOT.ChkAddrModule.Interface.Services.IMdbService _mdbSrv;

        [EventPublication(ACOT.ChkAddrModule.Interface.Constants.EventTopicNames.CurrentAddressInfoUpdate, PublicationScope.Global)]
        public event EventHandler UpdateAddressInfo;

        [InjectionConstructor]
        public AddrElementsSelectionViewPresenter(
            [ServiceDependency] ACOT.ChkAddrModule.Interface.Services.ICheckAddressService chkAddrSrv,
            [ServiceDependency] ACOT.ChkAddrModule.Interface.Services.IMdbService mdbSrv)
        {
            _chkAddrSrv = chkAddrSrv;
            _mdbSrv = mdbSrv;
        }

        /// <summary>
        /// This method is a placeholder that will be called by the view when it has been loaded.
        /// </summary>
        public override void OnViewReady()
        {
            View.Status.Text = "Не выбран элемент адреса";
            base.OnViewReady();
        }

        /// <summary>
        /// Close the view
        /// </summary>
        public override void OnCloseView()
        {
            base.CloseView();
        }


        [EventSubscription(EventTopicNames.SearchName, ThreadOption.UserInterface)]
        public void OnSearch(object sender, EventArgs<string> e)
        {
            DataTable dt = View.BindingSrc.DataSource as DataTable;
            if (dt != null)
            {
                string column = View.WorkArea.Columns[0].DataPropertyName;
                string query = column + " LIKE '" + e.Data.ToString() + "%'";
                DataRow[] drs = dt.Select(query);

                if (drs.Length != 0)
                    View.BindingSrc.Position = View.BindingSrc.Find(column, drs[0].ItemArray[0]);
            }
        }

        internal void OnSelectRow(System.Windows.Forms.DataGridViewRow dataGridViewRow)
        {
            string value = value = dataGridViewRow.Cells[0].Value.ToString() + " " + dataGridViewRow.Cells[1].Value.ToString();
            if (View.WorkArea.Columns[0].DataPropertyName == "ID")
                value = dataGridViewRow.Cells[0].Value.ToString();

            if (UpdateAddressInfo != null)
                UpdateAddressInfo(this, new EventArgs<string>(value));
        }

        // Когда пользователь нажимает кнопку редактирования элементов,
        // запускается выбор подразделений для редактирования.
        [EventSubscription(EventTopicNames.AddrElementsSelectionViewShow, ThreadOption.UserInterface)]
        public void AddrElementsSelectionViewShow(object sender, EventArgs<object[]> e)
        {
            object[] o = e.Data;

            //_view.SetPosition((int)o[1]);

            string filter = _chkAddrSrv.Model.CurrentCode;
            string table = _chkAddrSrv.Model.CurrentItem ==
                           ACOT.ChkAddrModule.Interface.BusinessEntities.Address.AddressItems.Ulica
                               ? "STREET"
                               : "KLADR";


            string query;
            System.Data.DataTable dt;

            if (_chkAddrSrv.Model.CurrentItem == Address.AddressItems.KodReg)
            {
                query = "select * from REGIONS order by ID";

                View.WorkArea.Columns[0].DataPropertyName = "ID";
                View.WorkArea.Columns[0].HeaderText = "Код";
                View.WorkArea.Columns[1].DataPropertyName = "NAME";
                View.WorkArea.Columns[1].HeaderText = "Наименовение";
                View.WorkArea.Columns[2].DataPropertyName = "SOCR";
                View.WorkArea.Columns[2].HeaderText = "Сокр.";
                dt = _mdbSrv.RegionsTable;
            }
            else
            {
                string denyCode = "";
                if (_chkAddrSrv.Model.CurrentItem == Address.AddressItems.Raion)
                    denyCode = filter.Replace("%", "000");
                if (_chkAddrSrv.Model.CurrentItem == Address.AddressItems.Gorod)
                    denyCode = filter.Replace("%", "000");
                if (_chkAddrSrv.Model.CurrentItem == Address.AddressItems.NasPunkt)
                    denyCode = filter.Replace("%", "00000");
                //if (_chkAddrSrv.Model.CurrentItem == Address.AddressItems.NasPunkt)
                //    denyCode = filter.Replace("%", "00000");

                query = "select * from " + table + " where code like '" + filter + "' and code<>'" + denyCode + "' order by NAME";
                View.WorkArea.Columns[0].DataPropertyName = "NAME";
                View.WorkArea.Columns[0].HeaderText = "Наименование";
                View.WorkArea.Columns[1].DataPropertyName = "SOCR";
                View.WorkArea.Columns[1].HeaderText = "Скор.";
                View.WorkArea.Columns[2].DataPropertyName = "INDEX";
                View.WorkArea.Columns[2].HeaderText = "Индекс";
                dt = _mdbSrv.SQL(query);
            }

            View.BindingSrc.DataSource = dt;

            if (dt.Rows.Count == 0)
                View.Status.Text = "Нет объектов для отображения. ";
            else 
                View.Status.Text = dt.Rows.Count.ToString() + " объектов";

            WorkItem.Activate();
        }


        [EventSubscription(ACOT.ChkAddrModule.Constants.EventTopicNames.CurrentElementRequest, ThreadOption.UserInterface)]
        public void OnForceUpdate(object sender, EventArgs e)
        {
            OnSelectRow(View.WorkArea.CurrentRow);
        }

        [EventSubscription(ACOT.ChkAddrModule.Constants.EventTopicNames.AddrGridSelect, ThreadOption.UserInterface)]
        public void OnGridViewSelect(object sender, EventArgs e)
        {
            View.WorkArea.Select();
            View.WorkArea.Focus();
        }
    }
}