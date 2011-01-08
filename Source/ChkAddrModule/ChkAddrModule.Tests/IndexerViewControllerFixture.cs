using ACOT.ChkAddrModule.Tests.Support;
using ACOT.Infrastructure.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.CompositeUI;

namespace ACOT.ChkAddrModule.Tests
{
    /// <summary>
    /// Summary description for IndexerViewPresenterFixture
    /// </summary>
    [TestClass]
    public class IndexerViewControllerFixture
    {
        private static WindowWorkspaceExtended workspace;
        private static WorkItem rootWorkitem;

        private TestContext testContextInstance;

        public IndexerViewControllerFixture()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            rootWorkitem = new TestableRootWorkItem();
            workspace = new WindowWorkspaceExtended();
            rootWorkitem.Workspaces.Add(workspace);
        }

        //
        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            workspace = null;
        }

        #endregion

        [TestMethod]
        public void IndexerViewOpenClose()
        {
            const string id = "indexer";
            ChkAddrModule.Views.IndexerView.IndexerView view = 
                rootWorkitem.Items.AddNew<ChkAddrModule.Views.IndexerView.IndexerView>(id);
            object view2 = rootWorkitem.Items.Get(id);
            Assert.AreEqual(view, view2);
        }
    }
}