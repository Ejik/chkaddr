using System;
using System.Collections.Generic;
using System.Text;

namespace ACOT.ChkAddrModule.Views.AddrElementsSelectionView
{
    public interface IAddrElementsSelectionView
    {
        System.Windows.Forms.Form ParentFm { get; }
        void SetPosition(int left);
        ACOT.ChkAddrModule.Interface.DataGridViewEx WorkArea { get; }
        System.Windows.Forms.BindingSource BindingSrc { get; set; }

        System.Windows.Forms.ToolStripStatusLabel Status { get; }
    }
}
