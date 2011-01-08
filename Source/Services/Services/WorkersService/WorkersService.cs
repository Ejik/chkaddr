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
                byte[] int8600 = new byte[8600];

                while (fs.Read(int8600, 0, 8600) > 0)
                {
                    WorkersDataSet.WorkersRow row = _workersTable.NewWorkersRow();
                    int tna = BitConverter.ToInt32(int8600, 0);
                    if (tna != 0)
                    {
                        row.TBN = tna.ToString("D5");
                        // FIO
                        string buf = Encoding.UTF8.GetString(
                            Encoding.Convert(Encoding.GetEncoding(866), Encoding.UTF8, int8600, 4, 44));
                        row.NAME = buf.Trim();

                        // kodstran
                        buf = Encoding.UTF8.GetString(
                            Encoding.Convert(Encoding.GetEncoding(866), Encoding.UTF8, int8600, 48, 3));
                        row.KODSTRAN = buf.Trim();

                        // kodreg
                        buf = Encoding.UTF8.GetString(
                            Encoding.Convert(Encoding.GetEncoding(866), Encoding.UTF8, int8600, 51, 2));
                        row.KODREG = buf.Trim();

                        // index
                        buf = Encoding.UTF8.GetString(
                            Encoding.Convert(Encoding.GetEncoding(866), Encoding.UTF8, int8600, 53, 6));
                        row.INDEX = buf.Trim();

                        // gorod
                        buf = Encoding.UTF8.GetString(
                            Encoding.Convert(Encoding.GetEncoding(866), Encoding.UTF8, int8600, 59, 40));
                        row.GOROD = buf.Trim();

                        // n_punkt
                        buf = Encoding.UTF8.GetString(
                            Encoding.Convert(Encoding.GetEncoding(866), Encoding.UTF8, int8600, 99, 40));
                        row.NPUNKT = buf.Trim();

                        // raion
                        buf = Encoding.UTF8.GetString(
                            Encoding.Convert(Encoding.GetEncoding(866), Encoding.UTF8, int8600, 139, 25));
                        row.RAION = buf.Trim();

                        // ulica
                        buf = Encoding.UTF8.GetString(
                            Encoding.Convert(Encoding.GetEncoding(866), Encoding.UTF8, int8600, 164, 40));
                        row.ULICA = buf.Trim();

                        // dom
                        buf = Encoding.UTF8.GetString(
                            Encoding.Convert(Encoding.GetEncoding(866), Encoding.UTF8, int8600, 204, 7));
                        row.DOM = buf.Trim();

                        // korp
                        buf = Encoding.UTF8.GetString(
                            Encoding.Convert(Encoding.GetEncoding(866), Encoding.UTF8, int8600, 211, 2));
                        row.KORPUS = buf.Trim();

                        // kvart
                        buf = Encoding.UTF8.GetString(
                            Encoding.Convert(Encoding.GetEncoding(866), Encoding.UTF8, int8600, 213, 5));
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
                shift *= 8600;
                shift -= 8600;
                fs.Seek(shift, SeekOrigin.Begin);
                byte[] int8600 = new byte[8600];

                // ТБН
                byte[] b = BitConverter.GetBytes(Convert.ToInt32(row.TBN));
                int i = 0;
                for(i = 0; i < b.Length; i++)
                    int8600[i] = b[i];

                AllocateDataInBuffer(row.NAME, 4, 44, int8600);
                AllocateDataInBuffer(row.KODSTRAN, 48, 3, int8600);
                AllocateDataInBuffer(row.KODREG, 51, 2, int8600);
                AllocateDataInBuffer(row.INDEX, 53, 6, int8600);
                AllocateDataInBuffer(row.GOROD, 59, 40, int8600);
                AllocateDataInBuffer(row.NPUNKT, 99, 40, int8600);
                AllocateDataInBuffer(row.RAION, 139, 25, int8600);
                AllocateDataInBuffer(row.ULICA, 164, 40, int8600);
                AllocateDataInBuffer(row.DOM, 204, 7, int8600);
                AllocateDataInBuffer(row.KORPUS, 211, 2, int8600);
                AllocateDataInBuffer(row.KVART, 213, 5, int8600);


                fs.Write(int8600, 0, 219);
            }
        }

        private void AllocateDataInBuffer(string buf, int idx, int bufLen, byte[] int8600)
        {
            for (int i = 0; i < bufLen; i++)
            {
                if (i < buf.Length)
                {
                    char[] cArr = buf.ToCharArray(0, buf.Length);
                    byte[] bArr = Encoding.UTF8.GetBytes(cArr);
                    bArr = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(866), bArr);
                    int8600[idx + i] = bArr[i];
                }
                else
                    int8600[idx + i] = (byte)32;
            }
        }

        #endregion
    }
}
