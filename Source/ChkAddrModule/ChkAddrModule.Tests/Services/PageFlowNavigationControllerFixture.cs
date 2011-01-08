using System;
using System.Drawing;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ACOT.ChkAddrModule.Services;
using ACOT.Infrastructure.Interface.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ACOT.ChkAddrModule.Tests.Mocks;
using System.Windows.Forms;

namespace ACOT.ChkAddrModule.Tests.Services
{
    /// <summary>
    /// Summary description for PageFlowNavigationControllerFixture
    /// </summary>
    [TestClass]
    public class PageFlowNavigationControllerFixture
    {
        private Form form1;
        private Form form2;
        private Form form3;
        private MockView view1;
        private MockView view2;
        private MockView view3;

        public PageFlowNavigationControllerFixture()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
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
             form1 = new Form();
             form2 = new Form();
             form3 = new Form();
             
             view1 = new MockView("View1");
             view2 = new MockView("View2");
             view3 = new MockView("View3");

             view1.Parent = form1;
             view2.Parent = form2;
             view3.Parent = form3;

             form1.Location = new Point(0, 0);
             form2.Location = new Point(50, 50);
             form3.Location = new Point(100, 100);
         }
        
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void AddViewToPageFlowOrder()
        {
            IPageFlowNavigationController controller = new PageFlowNavigationController();
            controller.AddView("View1", view1);
            controller.AddView("View2", view2);
            controller.AddView("View3", view3);

            string testviewID = controller.ViewOrder[1].ToString();

            Assert.AreEqual("View2", testviewID);
        }

        [TestMethod]
        public void RemoveViewFromPageFlowOrder()
        {
            const string id = "View1";
            MockView view = new MockView(id);

            IPageFlowNavigationController controller = new PageFlowNavigationController();
            controller.AddView(id, view);

            controller.RemoveView(id);

            Assert.AreEqual(controller.ViewOrder.Count, 0);
        }

        [TestMethod]
        public void ShowNextViewFromPageFlowOrder()
        {
            view1.ParentForm.Show();
            view2.ParentForm.Show();
            view3.ParentForm.Show();
            IPageFlowNavigationController controller = new PageFlowNavigationController();
            controller.AddView("View1", view1);
            controller.AddView("View2", view2);
            controller.AddView("View3", view3);

            Assert.IsFalse(view1.ParentForm.Visible);
            Assert.IsFalse(view2.ParentForm.Visible);
            Assert.IsTrue(view3.ParentForm.Visible);

            controller.RemoveView("View3");
            Assert.IsFalse(view1.ParentForm.Visible);
            Assert.IsTrue(view2.ParentForm.Visible);
        }
    }
}
