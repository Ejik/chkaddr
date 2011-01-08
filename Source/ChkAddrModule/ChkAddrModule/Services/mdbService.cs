using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ACOT.ChkAddrModule.Interface.Services;
using ACOT.ChkAddrModule.Views.IndexerView;
using ADODB;
using ADOX;
using JRO;
using Microsoft.Practices.CompositeUI;
using DataTypeEnum=ADOX.DataTypeEnum;

namespace ACOT.ChkAddrModule.Services
{
    [Service(typeof (IMdbService))]
    public class mdbService : IMdbService, IDisposable
    {
        #region Delegates

        public delegate void AsyncOperationInvoker(AsyncOperation operation, bool isCreateNewDbFile);

        #endregion

        private static Connection adodbConnection;
        private static Catalog dbCatalog;

        private DataSet _ds;
        private AsyncOperation _operation;
        private IIndexerView _view;
        private WorkItem _workitem;
        private string connString;
        private OleDbDataAdapter dbAdapter;
        private OleDbConnection dbConnection;
        private bool isNewDBCreated;
        private string mdb;

        /// <summary>
        /// Конструктор
        /// </summary>
        public mdbService([ServiceDependency] WorkItem workitem)
        {
            mdb = Environment.CurrentDirectory + "\\kladr.mdb";
            _workitem = workitem;
            //IndexingRun();
        }

        public OleDbDataAdapter DbDataAdapter
        {
            get
            {
                if (dbAdapter == null) FillDataSet();
                return dbAdapter;
            }
        }

        #region IMdbService Members

        public DataTable KladrTable
        {
            get { return _ds.Tables["KLADR"]; }
        }

        public DataTable StreetTable
        {
            get { return _ds.Tables["STREET"]; }
        }

        public DataTable RegionsTable
        {
            get { return _ds.Tables["REGIONS"]; }
        }

        public void IndexingRun() // UserControl view)
        {
            //_view = view as IIndexerView;
            //_view = _workitem.Items.AddNew<IndexerView>(ViewNames.IndexerLayout);
            //_workitem.Workspaces[WorkspaceNames.ModalWindows].Show(_view);

            // Создаем пустой файл базы, если он не существует
            if (TestMdbFile())
            {
                if (File.Exists(mdb))
                    File.Delete(mdb);

                _view = new IndexerView();
                _view.ShowView();

                string taskID = "IndexerThread";
                AsyncOperation ao = AsyncOperationManager.CreateOperation(taskID);
                new AsyncOperationInvoker(IndexerThread).BeginInvoke(ao, true, null, null);
            }
            else
            {
                PerfomConnection();
            }
        }

        public DataTable SQL(string query)
        {
            DataSet dsTmp = new DataSet();
            dbAdapter = new OleDbDataAdapter(query, dbConnection);
            dbAdapter.Fill(dsTmp);
            return dsTmp.Tables[0];
        }

        public void Dispose()
        {
            CloseConnection();
            if (isNewDBCreated)
                CompactDataBase();
        }

        #endregion

        private void PerfomConnection()
        {
            OpenConnection();
            _ds = new DataSet("KLADR");

            // Очень долгое заполнение данными 
            FillDataSet();
        }

        private void IndexerThread(AsyncOperation operation, bool isCreateNewDbFile)
        {
            _operation = operation;
            if (isCreateNewDbFile)
                CreateBlankAccessDatabase();

            SendOrPostCallback callUpdate = delegate(object o) { if (_view != null) _view.CloseView(); };
            operation.Post(callUpdate, null);
            PerfomConnection();
        }

        private bool TestMdbFile()
        {
            FileInfo fi = new FileInfo(mdb);
            if (!fi.Exists)
                return true;

            if (fi.Length < 50000000)
                return true;

            return false;
        }

        public void OpenConnection()
        {
            try
            {
                dbConnection = new OleDbConnection(GetConnectionString(mdb));
                dbConnection.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("Не могу создать соединение.", ex);
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (dbConnection != null)
                {
                    dbConnection.Close();
                    dbConnection = null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Не могу закрыть соединение.", ex);
            }
        }

        private void FillDataBase()
        {
            try
            {
                // Полключаемся к файлу БД
                OleDbConnection mdbCon = new OleDbConnection(GetConnectionString(mdb));
                mdbCon.Open();

                string sql = "INSERT INTO KLADR ( NAME, SOCR, CODE, [INDEX] ) " +
                             "SELECT NAME, SOCR, CODE, [INDEX] FROM [dBASE III;DATABASE=" +
                             Environment.CurrentDirectory + "\\].[kladr.dbf];";

                OleDbCommand cmd = new OleDbCommand(sql, mdbCon);
                cmd.ExecuteNonQuery();

                sql = "INSERT INTO STREET ( NAME, SOCR, CODE, [INDEX] ) " +
                      "SELECT NAME, SOCR, CODE, [INDEX] FROM [dBASE III;DATABASE=" + Environment.CurrentDirectory +
                      "\\].[street.dbf];";
                cmd.CommandText = sql;

                cmd.ExecuteNonQuery();
                mdbCon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании индексов ");
            }
        }

        public static string GetConnectionString(string ADatabasePath)
        {
            StringBuilder connText = new StringBuilder();
            connText.Append("Provider=Microsoft.Jet.OLEDB.4.0;");
            connText.Append("Data Source=\"").Append(ADatabasePath).Append("\";");
            connText.Append("Jet OLEDB:Engine Type=5;");
            connText.Append("Jet OLEDB:Encrypt Database=True;");
            connText.Append("User ID=\"Admin\";");

            return connText.ToString();
        }

        public static Catalog OpenAdoxConnection(string connectionString)
        {
            try
            {
                adodbConnection = new ConnectionClass();
                adodbConnection.Open(connectionString, null, null, 0);
                dbCatalog = new CatalogClass();
                dbCatalog.ActiveConnection = adodbConnection;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to open connection", ex);
            }
            return dbCatalog;
        }

        public static void CloseAdoxConnection()
        {
            try
            {
                if (adodbConnection != null)
                {
                    dbCatalog = null;
                    adodbConnection.Close();
                    adodbConnection = null;
                }
            }
            catch
            {
                //suppress exceptions 
            }
        }

        private void FillDataSet()
        {
            string query;
            query = "select Left(CODE, 2) as ID, NAME, SOCR from KLADR where CODE like '%00000000000'";
            dbAdapter = new OleDbDataAdapter(query, dbConnection);
            dbAdapter.SelectCommand.CommandText = query;
            dbAdapter.Fill(_ds, "REGIONS");
        }

        #region Создание нового файла базы данных с пустыми таблицами kladr и street

        private void CreateBlankAccessDatabase()
        {
            // Проверяем наличие индексов у файлов dbf
            bool isKladrIndexExist = DropDbfIndex(Environment.CurrentDirectory + "\\kladr.dbf");

            bool isStreetIndexExist = DropDbfIndex(Environment.CurrentDirectory + "\\street.dbf");


            SendOrPostCallback callUpdate = delegate(object o)
                                                {
                                                    if (_view != null)
                                                    {
                                                        if (o != null)
                                                            _view.ProgressLabel.Text = o.ToString();
                                                        _view.Progress.PerformStep();
                                                    }
                                                };


            connString = GetConnectionString(mdb);

            Catalog dbCatalog = new Catalog();
            dbCatalog.Create(connString);

            // Create instance of Table object.                    
            Table kladrTable = new Table();
            Table streetTable = new Table();

            _operation.Post(callUpdate, "Создается таблица KLADR");

            kladrTable.Name = "KLADR";
            kladrTable.Columns.Append("NAME", DataTypeEnum.adVarWChar, 40);
            kladrTable.Columns.Append("SOCR", DataTypeEnum.adVarWChar, 10);
            kladrTable.Columns.Append("CODE", DataTypeEnum.adVarWChar, 13);
            kladrTable.Columns.Append("INDEX", DataTypeEnum.adVarWChar, 6);
            kladrTable.Columns[3].Attributes = ColumnAttributesEnum.adColNullable;
            dbCatalog.Tables.Append((object) kladrTable);

            _operation.Post(callUpdate, "Создается таблица STREET");

            streetTable.Name = "STREET";
            streetTable.Columns.Append("NAME", DataTypeEnum.adVarWChar, 40);
            streetTable.Columns.Append("SOCR", DataTypeEnum.adVarWChar, 10);
            streetTable.Columns.Append("CODE", DataTypeEnum.adVarWChar, 17);
            streetTable.Columns.Append("INDEX", DataTypeEnum.adVarWChar, 6);
            streetTable.Columns[3].Attributes = ColumnAttributesEnum.adColNullable;
            dbCatalog.Tables.Append((object) streetTable);

            _operation.Post(callUpdate, "Обработка...");
            FillDataBase();

            _operation.Post(callUpdate, "Индексируется KLADR");

            Index idx = new Index();
            idx.Name = "IDXCODE";
            idx.Unique = true;
            idx.PrimaryKey = true;
            idx.Columns.Append("CODE", DataTypeEnum.adVarWChar, 13);
            kladrTable.Indexes.Append((object) idx, null);

            _operation.Post(callUpdate, null);

            idx = new Index();
            idx.Name = "IDXNAME";
            idx.Unique = false;
            idx.PrimaryKey = false;
            idx.Columns.Append("NAME", DataTypeEnum.adVarWChar, 40);
            kladrTable.Indexes.Append((object) idx, null);

            _operation.Post(callUpdate, "Индексируется STREET");

            idx = new Index();
            idx.Name = "IDXCODE";
            idx.Unique = true;
            idx.PrimaryKey = true;
            idx.Columns.Append("CODE", DataTypeEnum.adVarWChar, 17);
            streetTable.Indexes.Append((object) idx, null);

            _operation.Post(callUpdate, null);
            idx = new Index();
            idx.Name = "IDXNAME";
            idx.Unique = false;
            idx.PrimaryKey = false;
            idx.Columns.Append("NAME", DataTypeEnum.adVarWChar, 13);
            streetTable.Indexes.Append((object) idx, null);

            dbCatalog = null;

            isNewDBCreated = true;
            //if (isKladrIndexExist)
            //    RestoreDbfIndex(Environment.CurrentDirectory + "\\kladr.dbf");
            //if (isStreetIndexExist)
            //    RestoreDbfIndex(Environment.CurrentDirectory + "\\street.dbf");
        }

        private void RestoreDbfIndex(string pathToDbfFile)
        {
            using (FileStream fs = File.OpenWrite(pathToDbfFile))
            {
                fs.Seek(28, SeekOrigin.Begin);
                byte[] buf = {1};
                fs.Write(buf, 0, 1);
                fs.Close();
            }
        }

        private bool DropDbfIndex(string pathToDbfFile)
        {
            bool isIndexExist = false;
            using (FileStream fs = File.Open(pathToDbfFile, FileMode.Open))
            {
                DateTime lastWTime = File.GetLastWriteTime(pathToDbfFile);
                fs.Seek(28, SeekOrigin.Begin);
                byte[] buf = {0};
                fs.Read(buf, 0, 1);
                if (buf[0] == 1)
                    isIndexExist = true;
                buf[0] = 0;
                fs.Seek(28, SeekOrigin.Begin);
                fs.Write(buf, 0, 1);
                fs.Close();
                File.SetLastWriteTime(pathToDbfFile, lastWTime);
            }
            return isIndexExist;
        }

        private void CompactDataBase()
        {
            string src = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mdb;
            string dest = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mdb + "~";

            JetEngine jet = new JetEngine();
            jet.CompactDatabase(src, dest);

            File.Delete(mdb);
            Directory.Move(mdb + "~", mdb);
        }

        #endregion
    }
}