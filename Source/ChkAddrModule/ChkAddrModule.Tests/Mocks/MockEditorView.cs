using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ACOT.ChkAddrModule.Views.EditorView;

namespace ACOT.ChkAddrModule.Tests.Mocks
{
    class MockEditorView : Form, IEditorView
    {
        private TextBox _currentElement;
        private TextBox _kodRegion;
        private TextBox _raion;
        private PictureBox _raionErrorPicture;
        private PictureBox _gorodErrorPicture;
        private PictureBox _naspunterrorpicture;
        private PictureBox _ulicaErrorPicture;
        private TextBox _index;
        private TextBox _gorod;
        private TextBox _naspunkt;
        private TextBox _ulica;
        private DataGridView _dataGridView;

        public MockEditorView()
        {
            _currentElement = new TextBox();
            _kodRegion = new TextBox();
            _kodRegion.Parent = this;
            _raion = new TextBox();
            _raion.Parent = this;
            _index = new TextBox();
            _index.Parent = this;
            _gorod = new TextBox();
            _gorod.Parent = this;
            _naspunkt = new TextBox();
            _naspunkt.Parent = this;
            _ulica = new TextBox();
            _ulica.Parent = this;

            _dataGridView = new DataGridView();
            _dataGridView.Parent = this;

            Dom = new TextBox();
            Dom.Parent = this;
            Korp = new TextBox();
            Korp.Parent = this;
            Kvart = new TextBox();
            Kvart.Parent = this;


            _raionErrorPicture = new PictureBox();
            _gorodErrorPicture = new PictureBox();
            _naspunterrorpicture = new PictureBox();
            _ulicaErrorPicture = new PictureBox();
        }

        #region IEditorView Members

        public BindingSource bindingSource
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public TextBox FullAddress
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public TextBox KodRegion
        {
            get
            {
                return _kodRegion;
            }
            set
            {
                _kodRegion = value;
            }
        }

        public Label RegionLabel
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public TextBox Index
        {
            get
            {
                return _index;
            }
        }

        public TextBox Raion
        {
            get
            {
                return _raion;
            }
            set
            {
                _raion = value;
            }
        }

        public TextBox Gorod
        {
            get
            {
                return _gorod;
            }
        }

        public TextBox NasPunkt
        {
            get
            {
               return _naspunkt;
            }
        }

        public TextBox Ulica
        {
            get
            {
                return _ulica;
            }
        }

        public TextBox Dom { get; set; }

        public TextBox Korp { get; set; }

        public TextBox Kvart { get; set; }

        public PictureBox RaionErrorPicture
        {
            get { return _raionErrorPicture; }
        }

        public PictureBox GorodErrorPicture
        {
            get { return _gorodErrorPicture; }
        }

        public PictureBox NaspunktErrorPicture
        {
            get { return _naspunterrorpicture; }
        }

        public PictureBox UlicaErrorPicture
        {
            get { return _ulicaErrorPicture; }
        }

        public System.Drawing.Point ViewLocation
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Title
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int ViewWidth
        {
            get { throw new NotImplementedException(); }
        }

        public Form ParentFm
        {
            get { throw new NotImplementedException(); }
        }

        public Microsoft.Practices.CompositeUI.SmartParts.IWorkspace rightWorkSpace
        {
            get { throw new NotImplementedException(); }
        }

        public int UserCtrlWidth
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsAddressCorrect
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void SetCenterPosition()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
