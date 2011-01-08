using System;
using System.Collections.Generic;
using System.Text;

namespace ACOT.ChkAddrModule.Interface.Services
{
    public interface ICmdLineService
    {
        string CmdLineParameter { get; set; }
        string TBN { get; set; }
        bool isAddrpFileExist { get; set; }
    }
}
