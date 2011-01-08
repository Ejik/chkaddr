//----------------------------------------------------------------------------------------
// patterns & practices - Smart Client Software Factory - Guidance Package
//
// This file was generated by the "Add View" recipe.
//
// A presenter calls methods of a view to update the information that the view displays. 
// The view exposes its methods through an interface definition, and the presenter contains
// a reference to the view interface. This allows you to test the presenter with different 
// implementations of a view (for example, a mock view).
//
// For more information see:
// ms-help://MS.VSCC.v80/MS.VSIPCC.v80/ms.practices.scsf.2007may/SCSF/html/02-09-010-ModelViewPresenter_MVP.htm
//
// Latest version of this Guidance Package: http://go.microsoft.com/fwlink/?LinkId=62182
//----------------------------------------------------------------------------------------

using System;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ACOT.ChkAddrModule.Constants;
using ACOT.ChkAddrModule.Interface.BusinessEntities;
using ACOT.ChkAddrModule.Interface.Services;
using ACOT.Infrastructure.Interface;
using ACOT.Infrastructure.Interface.Services;
using ACOT.Services.WorkersService;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;

namespace ACOT.ChkAddrModule.Views.EditorView
{
    public partial class EditorViewPresenter : Presenter<IEditorView>
    {
        private Address _address;
        private readonly ICheckAddressService _chkAddrSrv;
        private bool _stopChecking;
        private Control _fakeDataGridView;
        ErrorProvider _errProvider;

        public TextBox _currentElement;
        public List<Control> _tabOrder;
        //public int _activeControlIndex;

        [DllImport("user32.dll")]
        private static extern byte LoadKeyboardLayout(string hkl, int Flags);

        [DllImport("user32.dll")]
        private static extern byte ActivateKeyboardLayout(string hkl, int Flags);
        
        [InjectionConstructor]
        public EditorViewPresenter([ServiceDependency] ICheckAddressService chkAddrSrv,
                                   [ServiceDependency] IMdbService mdbSrv)
        {
            _chkAddrSrv = chkAddrSrv;
            _mdbService = mdbSrv;
        }

        [ServiceDependency]
        public IWorkersService WorkersService { get; set; }

        private IMdbService _mdbService { get; set; }

        #region Event publications

        [EventPublication(EventTopicNames.DatasetUpdate, PublicationScope.Global)]
        public event EventHandler OnDataSetChange;

        [EventPublication(EventTopicNames.AddrElementsSelectionViewShow, PublicationScope.Descendants)]
        public event EventHandler OnAddrSelectionOpen;

        [EventPublication(EventTopicNames.SearchName, PublicationScope.Descendants)]
        public event EventHandler OnSearchName;

        [EventPublication(EventTopicNames.CurrentElementRequest, PublicationScope.Global)]
        public event EventHandler OnCurrentElementRequest;

        [EventPublication(EventTopicNames.EditorViewClose, PublicationScope.Global)]
        public event EventHandler OnEditorViewClose;

        [EventPublication(EventTopicNames.AddrGridSelect, PublicationScope.Global)]
        public event EventHandler OnAddrGridSelect;

        #endregion

        /// <summary>
        /// This method is a placeholder that will be called by the view when it has been loaded.
        /// </summary>
        public override void OnViewReady()
        {
            // ������������� ������� �������� ����������

            byte s = LoadKeyboardLayout("00000419", 0);
            s = ActivateKeyboardLayout("00000419", 0);

            _errProvider = new ErrorProvider();
            _errProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            _stopChecking = true;
            SetRegionName(View.KodRegion.Text);
            _currentElement = View.Raion;
            View.ParentFm.FormClosing += ParentFm_Closing;
            SetUpTabOrder();
            base.OnViewReady();
        }

        /// <summary>
        /// Close the view
        /// </summary>
        public override void OnCloseView()
        {
            IPageFlowNavigationController pageController =
                WorkItem.Services.Get<IPageFlowNavigationController>();

            const string viewID = "EditorView";
            pageController.RemoveView(viewID);

            base.CloseView();
        }

        [EventSubscription(EventTopicNames.CheckCurrentAddressRun, ThreadOption.UserInterface)]
        public void CheckCurrentAddressRun(object sender, EventArgs e)
        {
            CheckCurrentAddressInfo();
            if (_currentElement != null)
                actionBtnClick(_currentElement);
        }

        private bool CheckCurrentAddressInfo()
        {
            bool isCorrect = true;

            

            _errProvider.Clear();
            View.RaionErrorPicture.Visible = false;
            View.GorodErrorPicture.Visible = false;
            View.NaspunktErrorPicture.Visible = false;
            View.UlicaErrorPicture.Visible = false;

            _address = new Address();
            _address.KodReg = View.KodRegion.Text;
            _address.Index = View.Index.Text;
            _address.Raion = View.Raion.Text;
            _address.Gorod = View.Gorod.Text;
            _address.NasPunkt = View.NasPunkt.Text;
            _address.Ulica = View.Ulica.Text;

            _chkAddrSrv.Check(_address);

            
            switch (_chkAddrSrv.ErrorItem)
            {
                case Address.AddressItems.Raion:
                    _currentElement = View.Raion;
                    View.Raion.Select();
                    //View.RaionErrorPicture.Visible = true;
                    _errProvider.SetError(View.RaionErrorPicture, "������ � ������");
                    isCorrect = false;
                    break;
                case Address.AddressItems.Gorod:
                    _currentElement = View.Gorod;
                    View.Gorod.Select();
                    //View.GorodErrorPicture.Visible = true;
                    _errProvider.SetError(View.Gorod, "������ � ������");
                    isCorrect = false;
                    break;
                case Address.AddressItems.NasPunkt:
                    _currentElement = View.NasPunkt;
                    View.NasPunkt.Select();
                    //View.NaspunktErrorPicture.Visible = true;
                    _errProvider.SetError(View.NasPunkt, "������ � ���������� ������");
                    isCorrect = false;
                    break;
                case Address.AddressItems.Ulica:
                    _currentElement = View.Ulica;
                    View.Ulica.Select();
                    //View.UlicaErrorPicture.Visible = true;
                    _errProvider.SetError(View.Ulica, "������ � �����");
                    isCorrect = false;
                    break;
            }

            if (_chkAddrSrv.Model.Index != "")
                View.Index.Text = _chkAddrSrv.Model.Index;

            if ((View.Raion.Text == "") && (_chkAddrSrv.Model.Raion != ""))
                View.Raion.Text = _chkAddrSrv.Raion;


            //if (!isCorrect)
            //    actionBtnClick(_currentElement);
            return isCorrect;
        }

        internal void SaveBtnClick()
        {
            if (CheckCurrentAddressInfo())
            {
                View.KodRegion.DataBindings["Text"].WriteValue();
                View.Index.DataBindings["Text"].WriteValue();
                View.Raion.DataBindings["Text"].WriteValue();
                View.Gorod.DataBindings["Text"].WriteValue();
                View.NasPunkt.DataBindings["Text"].WriteValue();
                View.Ulica.DataBindings["Text"].WriteValue();
                View.NasPunkt.DataBindings["Text"].WriteValue();
                View.Dom.DataBindings["Text"].WriteValue();
                View.Korp.DataBindings["Text"].WriteValue();
                View.Kvart.DataBindings["Text"].WriteValue();

                WorkersService.SaveData();

                if (OnDataSetChange != null)
                    OnDataSetChange(this, EventArgs.Empty);
                
                _stopChecking = false;
                View.ParentFm.Close();
            }
        }

        internal void actionBtnClick(object sender)
        {
            GetCurrentElement(sender);

            if (_currentElement == View.KodRegion)
            {
                _chkAddrSrv.Model.CurrentCode = "%00000000000";
                _chkAddrSrv.Model.CurrentItem = Address.AddressItems.KodReg;
            }
            else if (_currentElement == View.Raion)
            {
                _chkAddrSrv.Model.CurrentCode = _chkAddrSrv.Model.KodReg + "%00000000";
                _chkAddrSrv.Model.CurrentItem = Address.AddressItems.Raion;
            }
            else if (_currentElement == View.Gorod)
            {
                _chkAddrSrv.Model.CurrentCode = _chkAddrSrv.Model.Raion == ""
                                                    ? _chkAddrSrv.Model.KodReg + "000%00000"
                                                    : _chkAddrSrv.Model.Raion + "%00000";
                _chkAddrSrv.Model.CurrentItem = Address.AddressItems.Gorod;
            }
            else if (_currentElement == View.NasPunkt)
            {
                if (_chkAddrSrv.Model.Gorod == "")
                {
                    if (_chkAddrSrv.Model.Raion == "")
                    {
                        _chkAddrSrv.Model.CurrentCode = _chkAddrSrv.Model.KodReg + "000000%";
                    }
                    else
                        _chkAddrSrv.Model.CurrentCode = _chkAddrSrv.Model.Raion + "000%";
                }
                else
                    _chkAddrSrv.Model.CurrentCode = _chkAddrSrv.Model.Gorod + "%";

                _chkAddrSrv.Model.CurrentItem = Address.AddressItems.NasPunkt;
            }
            else if (_currentElement == View.Ulica)
            {
                if (_chkAddrSrv.Model.NasPunkt == "")
                {
                    if (_chkAddrSrv.Model.Gorod == "")
                    {
                        if (_chkAddrSrv.Model.Raion == "")
                        {
                            if (_chkAddrSrv.Model.KodReg == "")
                            {
                                _chkAddrSrv.Model.CurrentCode = "";
                            }
                            else
                                _chkAddrSrv.Model.CurrentCode = _chkAddrSrv.Model.KodReg + "%";
                        }
                        else
                            _chkAddrSrv.Model.CurrentCode = _chkAddrSrv.Model.Raion + "%";
                    }
                    else
                        _chkAddrSrv.Model.CurrentCode = _chkAddrSrv.Model.Gorod + "%";
                }
                else
                    _chkAddrSrv.Model.CurrentCode = _chkAddrSrv.Model.NasPunkt + "%";
                _chkAddrSrv.Model.CurrentItem = Address.AddressItems.Ulica;
            }

            object[] args = new object[2] {_currentElement.Text, View.ViewLocation.X + View.ViewWidth};

            if (OnAddrSelectionOpen != null)
                OnAddrSelectionOpen(this, new EventArgs<object[]>(args));
        }

        internal void OnTextChanged(object sender)
        {
            TextBox textBox = GetCurrentElement(sender);
            string value = textBox.Text;
            //if (value == "")
            //    CheckCurrentAddressInfo();

            if (OnSearchName != null)
                OnSearchName(this, new EventArgs<string>(value));
        }

        internal void SetRegionName(string p)
        {
            DataRow[] dr = _mdbService.RegionsTable.Select("ID='" + p + "'");
            if (dr.Length != 0)
                View.RegionLabel.Text = dr[0].ItemArray[1] + " " + dr[0].ItemArray[2];

            //CheckCurrentAddressInfo();
        }

        [EventSubscription(EventTopicNames.CurrentAddressInfoUpdate, ThreadOption.UserInterface)]
        public void AddrElementUpdate(object sender, EventArgs<string> e)
        {
            _currentElement.Text = e.Data;
            if (_currentElement == View.KodRegion)
                SetRegionName(View.KodRegion.Text);

            CheckCurrentAddressInfo();
        }

        internal void OnTextBoxLeave(object sender)
        {
            TextBox textBox = GetCurrentElement(sender);
            if (textBox == View.KodRegion)
            {
                SetRegionName(textBox.Text);
            }
            else
                if (textBox.Text == "")
                    CheckCurrentAddressInfo();
        }

        internal void OnEnterKeyDown(object sender)
        {
            GetCurrentElement(sender);
            if (OnCurrentElementRequest != null)
                OnCurrentElementRequest(this, EventArgs.Empty);
        }

        private TextBox GetCurrentElement(object sender)
        {
            TextBox curTb = (TextBox) sender;
            switch (curTb.Name)
            {
                case "_kodRegTb":
                    _currentElement = View.KodRegion;
                    break;
                case "_raionTb":
                    _currentElement = View.Raion;
                    break;
                case "_gorodTb":
                    _currentElement = View.Gorod;
                    break;
                case "_naspunktTb":
                    _currentElement = View.NasPunkt;
                    break;
                case "_ulicaTb":
                    _currentElement = View.Ulica;
                    break;
            }
            return _currentElement;
        }

        public void CloseViewByCloseBtnClick()
        {
            _stopChecking = false;
            //if (OnEditorViewClose != null)
            //    OnEditorViewClose(this, new EventArgs<bool>(_stopChecking));
            View.ParentFm.Close();
        }

        void ParentFm_Closing(object sender, FormClosingEventArgs e)
        {
            if (OnEditorViewClose != null)
                OnEditorViewClose(this, new EventArgs<bool>(_stopChecking));
        }

        public void SetUpTabOrder()
        {
            _tabOrder = new List<Control>();
            Control[] controls = {View.KodRegion, _fakeDataGridView, View.Index, View.Raion, _fakeDataGridView, View.Gorod, _fakeDataGridView, View.NasPunkt, _fakeDataGridView, View.Ulica, _fakeDataGridView, View.Dom, View.Korp, View.Kvart};
            _tabOrder.AddRange(controls);

            //_activeControlIndex = 0;
            //_tabOrder[_activeControlIndex].Select();
        }

        public void SelectNextControl(Control control, bool forward)
        {
            int currentControlIndex = 0;
            foreach (Control ctrl in _tabOrder)
            {
                if (ctrl == control)
                {
                    break;
                }
                currentControlIndex++;
            }

            if (forward)
            {
                if (currentControlIndex > _tabOrder.Count - 2)
                    currentControlIndex = 0;
                else
                    currentControlIndex++;
            }
            else
            {
                if (currentControlIndex < 1)
                    currentControlIndex = _tabOrder.Count - 1;
                else
                    currentControlIndex--; 
            }

            if (_tabOrder[currentControlIndex] == null)
            {
                if (OnAddrGridSelect != null)
                    OnAddrGridSelect(this, EventArgs.Empty);
            }
            else
                _tabOrder[currentControlIndex].Select();
        }
    }
}