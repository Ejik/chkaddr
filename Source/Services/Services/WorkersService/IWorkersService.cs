using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ACOT.Services.WorkersService
{
    public interface IWorkersService
    {
        ACOT.Infrastructure.Interface.Data.WorkersDataSet.WorkersDataTable WorkersTable { get; set; }

        BindingSource bingingSource { get; set; }

        int GetIdByTBN(string p);

        void SaveData();
    }
}
