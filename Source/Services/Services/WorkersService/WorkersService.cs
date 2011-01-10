using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data;
using ACOT.Infrastructure.Interface.Data;
using Microsoft.Practices.CompositeUI;

namespace ACOT.Services.WorkersService
{
    public class AcotRecord {
        public const int recordSize = 12600;
    }

    public class WorkersService : IWorkersService
    {
        private readonly WorkersDataSet _workersDataSet;
        private WorkersDataSet.WorkersDataTable _workersTable;
        private string spraw;
        private string adres;

        public WorkersDataSet.WorkersDataTable WorkersTable
        {
            get { return _workersTable; }
            set { _workersTable = value; }
        }

        public WorkersService()
        {
            this._workersDataSet = new WorkersDataSet();
            this._workersTable = this._workersDataSet.Workers;
            this.bingingSource = new BindingSource();
            this.bingingSource.DataSource = this._workersTable;
            GetAllWorkers();
        }

        private void GetAllWorkers()
        {
            ACOT.Services.OrgnameService.IOrgnameService orgnameService = new ACOT.Services.OrgnameService.OrgnameService();
            string orgname = orgnameService.GetOrgname();
            spraw = Environment.CurrentDirectory + "\\SPRAW." + orgname;
            adres = Environment.CurrentDirectory + "\\ADRES." + orgname;

            using (FileStream fs = new FileStream(adres, FileMode.Open, FileAccess.ReadWrite))
            {
                fs.Seek(0, SeekOrigin.Begin);
                byte[] recordSize = new byte[AcotRecord.recordSize];

                while (fs.Read(recordSize, 0, AcotRecord.recordSize) > 0)
                {
                    WorkersDataSet.WorkersRow row = _workersTable.NewWorkersRow();
                    int tna = BitConverter.ToInt32(recordSize, 0);
                    if (tna != 0)
                    {
                        row.TBN = tna.ToString("D5");
                        // FIO
                        string buf = Encoding.UTF8.GetString(
                            Encoding.Convert(Encoding.GetEncoding(866), Encoding.UTF8, recordSize, 4, 44));
                        row.NAME = buf.Trim();

                        // kodstran
                        buf = Encoding.UTF8.GetString(
                            Encoding.Convert(Encoding.GetEncoding(866), Encoding.UTF8, recordSize, 48, 3));
                        row.KODSTRAN = buf.Trim();

                        // kodreg
                        buf = Encoding.UTF8.GetString(
                            Encoding.Convert(Encoding.GetEncoding(866), Encoding.UTF8, recordSize, 51, 2));
                        row.KODREG = buf.Trim();

                        // index
                        buf = Encoding.UTF8.GetString(
                            Encoding.Convert(Encoding.GetEncoding(866), Encoding.UTF8, recordSize, 53, 6));
                        row.INDEX = buf.Trim();

                        // gorod
                        buf = Encoding.UTF8.GetString(
                            Encoding.Convert(Encoding.GetEncoding(866), Encoding.UTF8, recordSize, 59, 40));
                        row.GOROD = buf.Trim();

                        // n_punkt
                        buf = Encoding.UTF8.GetString(
                            Encoding.Convert(Encoding.GetEncoding(866), Encoding.UTF8, recordSize, 99, 40));
                        row.NPUNKT = buf.Trim();

                        // raion
                        buf = Encoding.UTF8.GetString(
                            Encoding.Convert(Encoding.GetEncoding(866), Encoding.UTF8, recordSize, 139, 25));
                        row.RAION = buf.Trim();

                        // ulica
                        buf = Encoding.UTF8.GetString(
                            Encoding.Convert(Encoding.GetEncoding(866), Encoding.UTF8, recordSize, 164, 40));
                        row.ULICA = buf.Trim();

                        // dom
                        buf = Encoding.UTF8.GetString(
                            Encoding.Convert(Encoding.GetEncoding(866), Encoding.UTF8, recordSize, 204, 7));
                        row.DOM = buf.Trim();

                        // korp
                        buf = Encoding.UTF8.GetString(
                            Encoding.Convert(Encoding.GetEncoding(866), Encoding.UTF8, recordSize, 211, 2));
                        row.KORPUS = buf.Trim();

                        // kvart
                        buf = Encoding.UTF8.GetString(
                            Encoding.Convert(Encoding.GetEncoding(866), Encoding.UTF8, recordSize, 213, 5));
                        row.KVART = buf.Trim();

                        _workersTable.AddWorkersRow(row);
                    }
                    //else
                    //    row.TBN = "00000";
                }
            }
        }

        #region IWorkersService Members

        public BindingSource bingingSource { get; set; }

        public int GetIdByTBN(string p)
        {
            DataRow[] drs = this._workersTable.Select("TBN = '" + p + "'");
            if (drs.Length != 0)
            {
                bingingSource.Filter = "TBN=" + drs[0].ItemArray[1].ToString(); 
                return Convert.ToInt32(drs[0].ItemArray[0]);
            }
            return 0;
        }

        public void SaveData()
        {
            using (FileStream fs = new FileStream(adres, FileMode.Open, FileAccess.ReadWrite))
            {
                WorkersDataSet.WorkersRow row = (this.bingingSource.Current as DataRowView).Row as WorkersDataSet.WorkersRow;
                int shift = row.ID;
                shift *= AcotRecord.recordSize;
                shift -= AcotRecord.recordSize;
                fs.Seek(shift, SeekOrigin.Begin);
                byte[] recordSize = new byte[AcotRecord.recordSize];

                // ТБН
                byte[] b = BitConverter.GetBytes(Convert.ToInt32(row.TBN));
                int i = 0;
                for(i = 0; i < b.Length; i++)
                    recordSize[i] = b[i];

                AllocateDataInBuffer(row.NAME, 4, 44, recordSize);
                AllocateDataInBuffer(row.KODSTRAN, 48, 3, recordSize);
                AllocateDataInBuffer(row.KODREG, 51, 2, recordSize);
                AllocateDataInBuffer(row.INDEX, 53, 6, recordSize);
                AllocateDataInBuffer(row.GOROD, 59, 40, recordSize);
                AllocateDataInBuffer(row.NPUNKT, 99, 40, recordSize);
                AllocateDataInBuffer(row.RAION, 139, 25, recordSize);
                AllocateDataInBuffer(row.ULICA, 164, 40, recordSize);
                AllocateDataInBuffer(row.DOM, 204, 7, recordSize);
                AllocateDataInBuffer(row.KORPUS, 211, 2, recordSize);
                AllocateDataInBuffer(row.KVART, 213, 5, recordSize);


                fs.Write(recordSize, 0, 219);
            }
        }

        private void AllocateDataInBuffer(string buf, int idx, int bufLen, byte[] recordSize)
        {
            for (int i = 0; i < bufLen; i++)
            {
                if (i < buf.Length)
                {
                    char[] cArr = buf.ToCharArray(0, buf.Length);
                    byte[] bArr = Encoding.UTF8.GetBytes(cArr);
                    bArr = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(866), bArr);
                    recordSize[idx + i] = bArr[i];
                }
                else
                    recordSize[idx + i] = (byte)32;
            }
        }

        #endregion
    }
}
