using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Practices.CompositeUI;

namespace ACOT.Services.OrgnameService
{
    [Service(typeof(IOrgnameService))]
    public class OrgnameService : IOrgnameService
    {
        private string orgname;

        public OrgnameService()
        {
            orgname = "";               
        }


        #region IOrgnameService Members

        public string GetOrgname()
        {
            string orgn = Environment.CurrentDirectory + "\\ORGNAME";

            if (File.Exists(orgn))
            {
                try
                {
                    StreamReader tr = new StreamReader(orgn);
                    orgname = tr.ReadLine();
                    tr.Close();
                    
                    //System.IO.StreamReader sr = new System.IO.StreamReader(orgn);
                    //int i = 0;
                    //char[] buf = new char[1];
                    //while (!sr.EndOfStream && i <= 2)
                    //{
                    //    sr.Read(buf, 0, 1);
                    //    orgname += buf[0].ToString();
                    //    i++;
                    //}
                    //sr.Close();
                    return this.orgname;
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
            return null;
        }

        #endregion
    }
}
