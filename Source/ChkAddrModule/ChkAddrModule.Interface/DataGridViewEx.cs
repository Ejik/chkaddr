using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ACOT.ChkAddrModule.Interface
{
    public class DataGridViewEx : DataGridView
    {
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (base.CurrentCell != null)
            {
                if (keyData == Keys.Enter)
                {
                    //SendKeys.Send("{TAB}");
                    return false;
                }

                if (keyData == Keys.Down)
                    if (base.CurrentCell.RowIndex == (base.RowCount - 1))
                    {
                        base.CurrentCell = base.Rows[0].Cells[1];
                        return true;
                    }

                if (keyData == Keys.Up)
                    if (base.CurrentCell.RowIndex == 0)
                    {
                        base.CurrentCell = base.Rows[base.RowCount - 1].Cells[1];
                        return true;
                    }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
