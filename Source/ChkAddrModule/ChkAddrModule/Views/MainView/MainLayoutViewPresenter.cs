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
using ACOT.ChkAddrModule.Constants;
using ACOT.ChkAddrModule.WorkItems;
using Microsoft.Practices.CompositeUI.Commands;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI;
using ACOT.Infrastructure.Interface;

namespace ACOT.ChkAddrModule.Views.MainView
{
    public partial class MainLayoutViewPresenter : Presenter<IMainLayoutView>
    {
        private ControlledWorkItem<WorkersListViewController> _workersListWorkItem;

        #region Event publications
        [EventPublication(EventTopicNames.CheckAllBtnIsPressed, PublicationScope.Descendants)]
        public event EventHandler CheckAllBtnIsPressed;

        [EventPublication(EventTopicNames.ManualEditBtnIsPressed, PublicationScope.Descendants)]
        public event EventHandler ManualEditBtnIsPressed;

        [EventPublication(EventTopicNames.ChkAddrModuleClose, PublicationScope.Global)]
        public event EventHandler ChkAddrModuleClose;
        #endregion

        /// <summary>
        /// This method is a placeholder that will be called by the view when it has been loaded.
        /// </summary>
        public override void OnViewReady()
        {
            base.OnViewReady();
        }

        /// <summary>
        /// Close the view
        /// </summary>
        public override void OnCloseView()
        {
            Infrastructure.Interface.Services.IPageFlowNavigationController pageController =
                WorkItem.Services.Get<Infrastructure.Interface.Services.IPageFlowNavigationController>();

            const string viewID = "MainView";
            pageController.RemoveView(viewID);

            if (ChkAddrModuleClose != null)
                ChkAddrModuleClose(this, EventArgs.Empty);
            
            if (_workersListWorkItem != null)
                _workersListWorkItem.Dispose();
            base.CloseView();
        }

        //[CommandHandler(CommandNames.CheckAll)]
        public void OnCheckAll()
        {
            if (_workersListWorkItem == null)
            {
                _workersListWorkItem = WorkItem.WorkItems.AddNew<ControlledWorkItem<WorkersListViewController>>();
                _workersListWorkItem.Controller.Run();
            }
            if (CheckAllBtnIsPressed != null)
                CheckAllBtnIsPressed(this, EventArgs.Empty);
        }

        //[CommandHandler(CommandNames.ManualEdit)]
        public void OnManualEdit()
        {
            if (_workersListWorkItem == null)
            {
                _workersListWorkItem = WorkItem.WorkItems.AddNew<ControlledWorkItem<WorkersListViewController>>();
                _workersListWorkItem.Controller.Run();
            }

            if (ManualEditBtnIsPressed != null)
                ManualEditBtnIsPressed(this, EventArgs.Empty);
        }
    }
}

