using System.ComponentModel;
using System.Threading;
using ACOT.ChkAddrModule.Interface.Services;
using ACOT.ChkAddrModule.Services;
using ACOT.ChkAddrModule.Views.IndexerView;

namespace ACOT.ChkAddrModule.Tests.Mocks
{
    public class MockMdbService : IMdbService
    {
        private IIndexerView _view;

        #region IMdbService Members

        public System.Data.DataTable SQL(string query)
        {
            throw new System.NotImplementedException();
        }

        public System.Data.DataTable KladrTable
        {
            get { throw new System.NotImplementedException(); }
        }

        public System.Data.DataTable StreetTable
        {
            get { throw new System.NotImplementedException(); }
        }

        public System.Data.DataTable RegionsTable
        {
            get { throw new System.NotImplementedException(); }
        }

        public void IndexingRun()
        //public void IndexingRun(System.Windows.Forms.UserControl view)
        {
            //_view = view as IIndexerView;

            string taskID = "IndexerThread";
            AsyncOperation ao = AsyncOperationManager.CreateOperation(taskID);
            new mdbService.AsyncOperationInvoker(IndexerThread).BeginInvoke(ao, true, null, null);
        }

        private void IndexerThread(AsyncOperation operation, bool isCreateNewDbFile)
        {
            for (int i = 0; i < 100000; i++)
            {
                
            }

            SendOrPostCallback callUpdate = delegate(object o)
                                                {
                                                    _view.CloseView();
                                                };
            operation.Post(callUpdate, null); ;
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}