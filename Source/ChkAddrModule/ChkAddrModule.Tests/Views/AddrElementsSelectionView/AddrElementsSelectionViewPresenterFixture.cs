using ACOT.ChkAddrModule.Interface.Services;
using ACOT.ChkAddrModule.Services;
using ACOT.ChkAddrModule.Tests.Mocks;
using ACOT.Infrastructure.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ACOT.ChkAddrModule.Views.AddrElementsSelectionView;

namespace ACOT.ChkAddrModule.Tests
{
    [TestClass]
    public class AddrElementSelectionViewPresenterFixture
    {
        private ICheckAddressService chkaddrService;
        private IMdbService mdbService;
        private MockAddrElementsSelectionView view;

        [TestInitialize]
        public void SetUp()
        {
            chkaddrService = new MockCheckAddressService();
            mdbService = new MockMdbService();
            view = new MockAddrElementsSelectionView();
        }

        [TestMethod]
        public void CanInitPresenter()
        {
            AddrElementsSelectionViewPresenter presenter = CreatePresenter();
            Assert.IsNotNull(presenter);
        }

        [TestMethod]
        public void OnSearch()
        {
            AddrElementsSelectionViewPresenter presenter = new AddrElementsSelectionViewPresenter(chkaddrService, mdbService);
            presenter.View = view;
            EventArgs<string> args = new EventArgs<string>("ярославска€");

            presenter.OnSearch(presenter, args);
            int result = view.BindingSrc.Position;
            Assert.AreNotEqual(4, result);
        }

        
        [TestCleanup]
        public void TearDown()
        {
            chkaddrService = null;
            mdbService = null;
        }

        private AddrElementsSelectionViewPresenter CreatePresenter()
        {
            return new AddrElementsSelectionViewPresenter(chkaddrService, mdbService);
        }
    }
}