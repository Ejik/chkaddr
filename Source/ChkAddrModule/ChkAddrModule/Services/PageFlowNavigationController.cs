using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ACOT.ChkAddrModule.Interface.Constants;
using ACOT.Infrastructure.Interface.Services;
using Microsoft.Practices.CompositeUI.EventBroker;

namespace ACOT.ChkAddrModule.Services
{
    public class PageFlowNavigationController : IPageFlowNavigationController
    {
        private Dictionary<string, object> _viewOrder;
        private List<string> _viewIdList;
        private object _currentWorkitem;

        public PageFlowNavigationController()
        {
            _viewOrder = new Dictionary<string, object>();
            _viewIdList = new List<string>();
            _currentWorkitem = null;
        }

        #region IPageFlowNavigationController Members

        public List<string> ViewOrder
        {
            get { return _viewIdList; }
        }

        public object CurrentWorkItem()
        {
            return _currentWorkitem;
        }

        public void AddView(string id, object view)
        {
            foreach (KeyValuePair<string, object> kpv in _viewOrder)
            {
                try
                {
                    (kpv.Value as UserControl).ParentForm.Hide();
                }
                catch(System.Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            _viewOrder.Add(id, view);
            _viewIdList.Add(id);
        }

        public void SetVisibility(string viewID)
        {
            UserControl ctrl = _viewOrder[viewID] as UserControl;
            if (ctrl.ParentForm != null)
                ctrl.ParentForm.Show();
        }

        public void RemoveView(string viewID)
        {
            _viewOrder.Remove(viewID);
            _viewIdList.Remove(viewID);

            if (_viewIdList.Count != 0)
            {
                string curID = _viewIdList[_viewIdList.Count - 1];
                (_viewOrder[curID] as UserControl).ParentForm.Show();
            }
            else
                if (ChkAddrModuleClose != null)
                    ChkAddrModuleClose(this, EventArgs.Empty);

        }

        #endregion

        [EventPublication(EventTopicNames.ChkAddrModuleClose, PublicationScope.Global)]
        public event EventHandler ChkAddrModuleClose;

    }
}