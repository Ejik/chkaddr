using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ACOT.ChkAddrModule.Interface.Services;
using Microsoft.Practices.CompositeUI;

namespace ACOT.ChkAddrModule.Services
{
    public class CmdLineService : ICmdLineService
    {
        public string CmdLineParameter { get; set; }
        public bool isAddrpFileExist { get; set; }
        public string TBN { get; set; }

        public CmdLineService()
        {
            string[] pars = Environment.GetCommandLineArgs();
            if (pars.Length > 1)
                CmdLineParameter = pars[1];
            else
                CmdLineParameter = "";

            string adrp = Environment.CurrentDirectory + "\\ADRP_TBN";
            if (System.IO.File.Exists(adrp))
            {
                isAddrpFileExist = true;
                StreamReader tr = new StreamReader(adrp);
                TBN = tr.ReadLine();
                tr.Close();
            }
            else
                isAddrpFileExist = false;
        }
    }

    
}
