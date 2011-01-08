using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ACOT.ChkAddrModule.Tests.Mocks
{
    public partial class MockView : UserControl
    {
        public MockView(string labeltext)
        {
            InitializeComponent();
            label1.Text = labeltext;
        }
    }
}
