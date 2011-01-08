using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ACOT.Infrastructure.Interface.BusinessEntities;
using System.Drawing.Drawing2D;

namespace ACOT.CommonDialogsModule.Dialogs.ChoiceSubDivizion
{
    public partial class ChoiceSubDivisionDialog : Form
    {
        private List<Subdivision> _data;
        private ListViewItem[] myCache; //array to cache items for the virtual list
        private int firstItem; //stores the index of the first item in the cache

        public List<Subdivision> Data
        {
            get { return _data; }
            set { this._data = value; OnDataChange(); }
        }

        public ListView DialogListView
        {
            get { return this._listView; }
        }

        private void OnDataChange()
        {
            this._listView.Items.Clear();
            this._listView.CheckBoxes = true;


            int i = 0;
            while(i < this._data.Count)
            {
                if (_data[i].WorkersCount == 0)
                    _data.Remove(_data[i--]);
                else
                    i++;
            }

            this._listView.VirtualListSize = _data.Count;
        }
        
        public ChoiceSubDivisionDialog()
        {
            InitializeComponent();
        }

        private void ChoiceSubDivisionDialog_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape: this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    break;
                case Keys.Enter: 
                    this.DialogResult = System.Windows.Forms.DialogResult.OK; 
                    break;
                case Keys.F10:
                    bool isNoChecked = true;
                    for (int i = 0; i < this._listView.VirtualListSize; i++)
                        if (this._listView.Items[i].Checked)
                        {
                            isNoChecked = false;
                            break;
                        }
                    if (isNoChecked)
                    {
                        for (int i = 0; i < this._listView.VirtualListSize; i++)
                            this._listView.Items[i].Checked = true;
                    }
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    break;
                case Keys.Insert:
                    this._listView.FocusedItem.Checked = !this._listView.FocusedItem.Checked;
                    int idx = this._listView.FocusedItem.Index + 1;
                    ListViewItem oldItem = this._listView.FocusedItem;
                    if (idx < this._listView.Items.Count)
                        this._listView.FocusedItem = this._listView.Items[idx];
                    else
                        this._listView.FocusedItem = this._listView.Items[0];
                    oldItem.Selected = false;
                    this._listView.FocusedItem.Selected = true;

                    this._listView.Invalidate();
                    break;
            }
        }

        private void ChoiceSubDivisionDialog_Load(object sender, EventArgs e)
        {
            this._listView.SelectedIndices.Add(0);
        }

        private void ChoiceSubDivisionDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            int count = 0;
            for (int i =0; i < this._listView.VirtualListSize; i++)
                if (this._listView.Items[i].Checked)
                    count++; 

            if (count == 0)
            {
                this._listView.FocusedItem.Checked = true;
            }
        }

    #region Функции View
        private void _listView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            if (e.Item.Checked)
                e.Item.ForeColor = Color.Blue;
            else
                e.Item.ForeColor = Color.Black;
            e.DrawDefault = true;
            if (!e.Item.Checked)
            {
                e.Item.Checked = true;
                e.Item.Checked = false;
            }
            
        }

        private void _listView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            //e.Graphics.FillRectangle(Brushes.WhiteSmoke, e.Bounds);
            using (LinearGradientBrush brush =  new LinearGradientBrush(e.Bounds, Color.White, Color.WhiteSmoke, LinearGradientMode.Horizontal))
            {
                Rectangle newBounds = new Rectangle(e.Bounds.Left, e.Bounds.Top, e.Bounds.Width, e.Bounds.Height);
                newBounds.Height += 10;
                e.Graphics.FillRectangle(brush, newBounds);
            }

            //e.DrawBackground();

            //using (Font headerFont = new Font(e.Font.FontFamily, 13, FontStyle.Bold))
            //Helvetica
            using (Font headerFont = new Font("Georgia", 14, FontStyle.Bold))
            {
                e.Graphics.DrawString(e.Header.Text, headerFont,
                    Brushes.Black, e.Bounds);
            }
            //e.DrawDefault = true;
        }

        private void _listView_MouseClick(object sender, MouseEventArgs e)
        {
            ListView lv = (ListView)sender;
            ListViewItem lvi = lv.GetItemAt(e.X, e.Y);
            if (lvi != null)
            {
                if (e.X < (lvi.Bounds.Left + 16))
                {
                    lvi.Checked = !lvi.Checked;
                    lv.Invalidate(lvi.Bounds);
                }
            }
        }

        private void _listView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView lv = (ListView)sender;
            ListViewItem lvi = lv.GetItemAt(e.X, e.Y);
            if (lvi != null)
                lv.Invalidate(lvi.Bounds);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void _listView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (myCache != null && e.ItemIndex >= firstItem && e.ItemIndex < firstItem + myCache.Length)
            {
                //A cache hit, so get the ListViewItem from the cache instead of making a new one.
                e.Item = myCache[e.ItemIndex - firstItem];
            }
            else
            {
                //A cache miss, so create a new ListViewItem and pass it back.
                e.Item = new ListViewItem(this._data[e.ItemIndex].Id.ToString());
                e.Item.SubItems.Add(this._data[e.ItemIndex].Name);
                e.Item.SubItems.Add(this._data[e.ItemIndex].WorkersCount.ToString());
            }
        }

        private void _listView_CacheVirtualItems(object sender, CacheVirtualItemsEventArgs e)
        {
            if (myCache != null && e.StartIndex >= firstItem && e.EndIndex <= firstItem + myCache.Length)
            {
                //If the newly requested cache is a subset of the old cache, 
                //no need to rebuild everything, so do nothing.
                return;
            }

            //Now we need to rebuild the cache.
            firstItem = e.StartIndex;
            int length = e.EndIndex - e.StartIndex + 1; //indexes are inclusive
            myCache = new ListViewItem[length];

            //Fill the cache with the appropriate ListViewItems.
            for (int i = 0; i < length; i++)
            {
                myCache[i] = new ListViewItem(this._data[i].Id.ToString());
                myCache[i].SubItems.Add(this._data[i].Name);
                myCache[i].SubItems.Add(this._data[i].WorkersCount.ToString());
            }
        }
    #endregion
    }
}
