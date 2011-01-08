using System;
using ACOT.ChkAddrModule.Interface.Services;
using ACOT.ChkAddrModule.Services;
using ACOT.ChkAddrModule.Tests.Mocks;
using ACOT.ChkAddrModule.Views.EditorView;
using ACOT.Infrastructure.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.CompositeUI.EventBroker;
using ACOT.ChkAddrModule.Interface.Constants;
using System.Windows.Forms;

namespace ACOT.ChkAddrModule.Tests
{
    [TestClass]
    public class EditorViewPresenterFixture
    {
        private ICheckAddressService chkaddrService;
        private IMdbService mdbService;
        private MockEditorView view;

        [EventPublication(EventTopicNames.CurrentAddressInfoUpdate, PublicationScope.WorkItem)]
        public event EventHandler AddressInfoUpdate;

        [TestInitialize]
        public void SetUp()
        {
            chkaddrService = new MockCheckAddressService();
            mdbService = new MockMdbService();
            view = new MockEditorView();
        }

        [TestMethod]
        public void CanInitPresenter()
        {
            EditorViewPresenter presenter = CreateEditorViewPresenter();
            Assert.IsNotNull(presenter);
        }

        [TestMethod]
        public void OnUpdateFoundName()
        {
            EditorViewPresenter presenter = new EditorViewPresenter(chkaddrService, mdbService);
            presenter.View = view;
            presenter._currentElement = view.Raion;
            EventArgs<string> args = new EventArgs<string>("ярославска€");
            presenter.AddrElementUpdate(presenter, args);
            string result = view.Raion.Text;
            Assert.AreEqual("ярославска€", result, true, result);
        }
        
        [TestMethod]
        public void SetFocusOnNextControl()
        {
            EditorViewPresenter presenter = CreateEditorViewPresenter();
            presenter.View = view;
            presenter.SetUpTabOrder();

            presenter.SelectNextControl(view.KodRegion, true);
            Assert.IsNull(view.ActiveControl);

            presenter.SelectNextControl(view.KodRegion, false);
            Assert.AreEqual(view.Kvart, view.ActiveControl);
        }

        [TestCleanup]
        public void TearDown()
        {
            chkaddrService = null;
            mdbService = null;
        }

        private EditorViewPresenter CreateEditorViewPresenter()
        {
            return new EditorViewPresenter(chkaddrService, mdbService);
        }
    }
}