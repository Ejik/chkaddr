//===============================================================================
// Microsoft patterns & practices
// Smart Client Software Factory
//===============================================================================
// Copyright  Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================

using System;
using System.Windows.Forms;
using ACOT.Infrastructure.Interface.Constants;
using Microsoft.Practices.CompositeUI.WinForms;

namespace ACOT.Infrastructure.Interface
{
    public class WindowWorkspaceExtended : WindowWorkspace
    {
        private IWin32Window _owner;

        public delegate EventHandler Closed(object sender, EventArgs e);

        public delegate FormClosingEventHandler Closing(object sender, FormClosingEventArgs e);

        public event EventHandler OnKeyDown;

        public event EventHandler OnClosed;

        public event FormClosingEventHandler OnClosing;

        public WindowWorkspaceExtended()
        {
        }

        public WindowWorkspaceExtended(IWin32Window owner) : base(owner)
        {
            _owner = owner;
        }

        protected override void OnShow(Control smartPart,
                                       Microsoft.Practices.CompositeUI.WinForms.WindowSmartPartInfo smartPartInfo)
        {
            GetOrCreateForm(smartPart);
            OnApplySmartPartInfo(smartPart, smartPartInfo);
            base.OnShow(smartPart, smartPartInfo);
        }

        protected override void OnClose(Control smartPart)
        {
            Form host = Windows[smartPart];
            host.Hide();
            base.OnClose(smartPart);
        }

        protected override void OnApplySmartPartInfo(Control smartPart,
                                                     Microsoft.Practices.CompositeUI.WinForms.WindowSmartPartInfo
                                                         smartPartInfo)
        {
            base.OnApplySmartPartInfo(smartPart, smartPartInfo);
            if (smartPartInfo is WindowSmartPartInfo)
            {
                WindowSmartPartInfo spi = (WindowSmartPartInfo) smartPartInfo;
                if (spi.Keys.ContainsKey(WindowWorkspaceSetting.AcceptButton))
                    Windows[smartPart].AcceptButton = (IButtonControl) spi.Keys[WindowWorkspaceSetting.AcceptButton];
                if (spi.Keys.ContainsKey(WindowWorkspaceSetting.CancelButton))
                    Windows[smartPart].CancelButton = (IButtonControl) spi.Keys[WindowWorkspaceSetting.CancelButton];
                if (spi.Keys.ContainsKey(WindowWorkspaceSetting.FormBorderStyle))
                    Windows[smartPart].FormBorderStyle =
                        (FormBorderStyle) spi.Keys[WindowWorkspaceSetting.FormBorderStyle];
                if (spi.Keys.ContainsKey(WindowWorkspaceSetting.AutoSize))
                    Windows[smartPart].AutoSize = (bool) spi.Keys[WindowWorkspaceSetting.AutoSize];
                if (spi.Keys.ContainsKey(WindowWorkspaceSetting.FormShowIcon))
                    Windows[smartPart].ShowIcon = (bool) spi.Keys[WindowWorkspaceSetting.FormShowIcon];
                if (spi.Keys.ContainsKey(WindowWorkspaceSetting.FormShowInTaskbar))
                    Windows[smartPart].ShowInTaskbar = (bool) spi.Keys[WindowWorkspaceSetting.FormShowInTaskbar];
                if (spi.Keys.ContainsKey(WindowWorkspaceSetting.StartPosition))
                    Windows[smartPart].StartPosition =
                        (FormStartPosition) spi.Keys[WindowWorkspaceSetting.StartPosition];
                if (spi.Keys.ContainsKey(WindowWorkspaceSetting.KeyPreview))
                    Windows[smartPart].KeyPreview = (bool) spi.Keys[WindowWorkspaceSetting.KeyPreview];
                if (spi.Keys.ContainsKey(WindowWorkspaceSetting.Dock))
                    Windows[smartPart].Dock = (DockStyle) spi.Keys[WindowWorkspaceSetting.Dock];
                if (spi.Keys.ContainsKey(WindowWorkspaceSetting.KeyDown))
                    Windows[smartPart].KeyDown += (KeyEventHandler) spi.Keys[WindowWorkspaceSetting.KeyDown];
            }
        }

        protected new Form GetOrCreateForm(Control control)
        {
            bool resizeRequired = !Windows.ContainsKey(control);
            Form form = base.GetOrCreateForm(control);
            form.ShowInTaskbar = (_owner == null);
            if (resizeRequired) 
                form.ClientSize = control.Size;
            form.Closed += new EventHandler(Form_Closed);
            form.FormClosing += new FormClosingEventHandler(Form_Closing);
            return form;
        }

        protected void Form_Closed(object sender, EventArgs e)
        {
            if (OnClosed != null)
                OnClosed(sender, e);
        }

        protected void Form_Closing(object sender, FormClosingEventArgs e)
        {
            if (OnClosing != null)
                OnClosing(sender, e);
        }
    }
}