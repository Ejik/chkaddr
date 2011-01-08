//----------------------------------------------------------------------------------------
// patterns & practices - Smart Client Software Factory - Guidance Package
//
// This file was generated by the "Add View" recipe.
//
// A presenter calls methods of a view to update the information that the view displays. 
// The view exposes its methods through an interface definition, and the presenter contains
// a reference to the view interface. This allows you to test the presenter with different 
// implementations of a view (for example, a mock view).
//
// For more information see:
// ms-help://MS.VSCC.v80/MS.VSIPCC.v80/ms.practices.scsf.2007may/SCSF/html/02-09-010-ModelViewPresenter_MVP.htm
//
// Latest version of this Guidance Package: http://go.microsoft.com/fwlink/?LinkId=62182
//----------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using ACOT.ChkAddrModule.Constants;
using ACOT.ChkAddrModule.Interface.BusinessEntities;
using ACOT.ChkAddrModule.Interface.Services;
using ACOT.Infrastructure.Interface.Data;
using ACOT.Services.WorkersService;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI;
using ACOT.Infrastructure.Interface;
using System.ComponentModel;
using ACOT.ChkAddrModule.WorkItems;

namespace ACOT.ChkAddrModule.Views.ListView
{
    public partial class WorkersListViewPresenter : Presenter<IWorkersListView>
    {
        private ACOT.ChkAddrModule.Views.ListView.ProgressThreadState _threadMode;

        private ControlledWorkItem<EditorViewController> _editorWorkitem;
        
        public IWorkersService _workersService;

        public ICheckAddressService _chkAddrService { get; set; }

        private ListViewItem[] myCache; //array to cache items for the virtual list
        private int firstItem; //stores the index of the first item in the cache


        [EventPublication(EventTopicNames.EditorViewShow, PublicationScope.Descendants)]
        public event EventHandler CellDblClick;

        [EventPublication(EventTopicNames.ChkAddrModuleClose, PublicationScope.Global)]
        public event EventHandler ChkAddrModuleClose;

        public delegate void AsyncOperationInvoker(AsyncOperation operation);

        [InjectionConstructor]
        public WorkersListViewPresenter([ServiceDependency] ICheckAddressService chkAddrService)
        {
            _chkAddrService = chkAddrService;
        }

        /// <summary>
        /// This method is a placeholder that will be called by the view when it has been loaded.
        /// </summary>
        public override void OnViewReady()
        {
            _workersService = WorkItem.Services.Get<IWorkersService>();
            if (_workersService == null)
                _workersService = WorkItem.Services.AddNew<WorkersService, IWorkersService>();

            View.ParentFm.FormClosing += delegate(object sender, FormClosingEventArgs e)
                                             {
                                                 if (_threadMode == ProgressThreadState.Stop)
                                                     e.Cancel = false;
                                                 else
                                                 {
                                                     _threadMode = ProgressThreadState.Stop;
                                                     //e.Cancel = true;
                                                 }
                                             };
            base.OnViewReady();
        }

        /// <summary>
        /// Close the view
        /// </summary>
        public override void OnCloseView()
        {
            _threadMode = ProgressThreadState.Stop;

            ACOT.Infrastructure.Interface.Services.IPageFlowNavigationController pageController =
                WorkItem.Services.Get<ACOT.Infrastructure.Interface.Services.IPageFlowNavigationController>();

            const string viewID = "WorkersView";
            pageController.RemoveView(viewID);
            if (!pageController.ViewOrder.Contains("MainView"))
                if (ChkAddrModuleClose != null)
                    ChkAddrModuleClose(this, EventArgs.Empty);

            if (_editorWorkitem != null)
                _editorWorkitem.Dispose();

            base.CloseView();
        }

        internal void CellMouseDoubleClick(DataGridViewCellMouseEventArgs e)
        {
            int param = Convert.ToInt32(View.DataGridLayoutView.Rows[e.RowIndex].Cells[0].Value);
            FireCellDblClickEvent(param);
        }

        internal void EnterPress()
        {
            int param = Convert.ToInt32(View.DataGridLayoutView.SelectedRows[0].Cells[0].Value);
            FireCellDblClickEvent(param);
        }

        private void FireCellDblClickEvent(int param)
        {
            if (_editorWorkitem == null)
            {
                _editorWorkitem = WorkItem.WorkItems.AddNew<ControlledWorkItem<EditorViewController>>();
                _editorWorkitem.Controller.Run();
            }

            if (CellDblClick != null)
                CellDblClick(this, new EventArgs<int>(param));
        }

        internal void CheckAllAddresses()
        {
            if (View.ChkAddrBtn.Text == "�������� ����")
            {
                View.ChkAddrBtn.Text = "��������";
                string taskID = "ChkThread";
                AsyncOperation ao = AsyncOperationManager.CreateOperation(taskID);
                new AsyncOperationInvoker(ChkThread).BeginInvoke(ao, null, null);
            }
            else
            {
                View.ChkAddrBtn.Text = "�������� ����";
                _threadMode = ProgressThreadState.Stop;
            }
        }

        private void ChkThread(AsyncOperation operation)
        {
            WorkersDataSet.WorkersDataTable dt = _workersService.bingingSource.DataSource as WorkersDataSet.WorkersDataTable;
            string filter = _workersService.bingingSource.Filter;
            DataRow[] dr = dt.Select(filter);

            _threadMode = ProgressThreadState.Run;
            int i = 0;

            while ((i < dr.Length) && (_threadMode != ProgressThreadState.Stop))
            {
                Address addr = new Address();

                ACOT.Infrastructure.Interface.Data.WorkersDataSet.WorkersRow currentRow =
                    dr[i] as ACOT.Infrastructure.Interface.Data.WorkersDataSet.WorkersRow;

                addr.KodReg = currentRow.KODREG;
                addr.Raion = currentRow.RAION;
                addr.Gorod = currentRow.GOROD;
                addr.NasPunkt = currentRow.NPUNKT;
                addr.Ulica = currentRow.ULICA;

                _chkAddrService.Check(addr);
                if (_chkAddrService.ErrorItem != Address.AddressItems.None)
                {
                    // � ���� ����� ������ ����� ���������� � ������ �������� � UI.
                    SendOrPostCallback editorCallback = delegate(object state)
                                                            {
                                                                int param =
                                                                    Convert.ToInt32(
                                                                        View.DataGridLayoutView.SelectedRows[0].Cells[0]
                                                                            .Value);
                                                                FireCellDblClickEvent(param);
                                                            };
                    operation.Post(editorCallback, null);
                    _threadMode = ProgressThreadState.Pause;
                }

                // � ���� ����� ������ ����� ���������� � ������ �������� � UI.
                SendOrPostCallback callback = delegate(object state)
                {
                    if (View != null)
                    {
                        DataGridViewRow rowView = View.DataGridLayoutView.Rows[(int) state];
                        rowView.Selected = true;
                        View.DataGridLayoutView.FirstDisplayedScrollingRowIndex = (int) state;
                        _workersService.bingingSource.Position = (int) state + 1;
                    }
                };

                while (_threadMode == ProgressThreadState.Pause)
                {
                    
                }
                operation.Post(callback, i);
                i++;
            }

            SendOrPostCallback callUpdate = delegate(object o)
                                                {
                                                    View.ChkAddrBtn.Text = "�������� ����";
                                                    MessageBox.Show("�������� �������", "������ chkaddr");
                                                    OnCloseView();
                                                    if (ChkAddrModuleClose != null)
                                                        ChkAddrModuleClose(this, EventArgs.Empty);
                                                };
            if (_threadMode != ProgressThreadState.Stop)
                operation.Post(callUpdate, null);
        }

        [EventSubscription(EventTopicNames.EditorViewClose, ThreadOption.UserInterface)]
        public void OnEditorViewClose(object sender, EventArgs<bool> e)
        {
            if (_threadMode == ProgressThreadState.Pause)
                if ((bool)e.Data)
                {
                    _threadMode = ProgressThreadState.Stop;
                    MessageBox.Show("�������� �������", "������ chkaddr");
                    OnCloseView();
                    if (ChkAddrModuleClose != null)
                        ChkAddrModuleClose(this, EventArgs.Empty);
                }
                else
                    _threadMode = ProgressThreadState.Run;
                    

            //if (_threadMode == EMode.Pause)
            //    if (MessageBox.Show("������ ������ ���������� ��������?", "������ chkaddr", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //        _threadMode = EMode.Run;
            //    else
            //        _threadMode = EMode.Stop;

            
        }

        public void TryToCloseView()
        {
            if (_threadMode == ProgressThreadState.Stop)
                OnCloseView();
        }
    }
}
