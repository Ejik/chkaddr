using System;
using Microsoft.Practices.CompositeUI;
using ACOT.ChkAddrModule.Interface.Services;
using ACOT.ChkAddrModule.Interface.BusinessEntities;
using System.Data;


namespace ACOT.ChkAddrModule.Services
{
    //[Service(typeof(ICheckAddressService), AddOnDemand = true)]
    public class CheckAddressService : ICheckAddressService
    {
        private CurrentAddressInfo _model;
        private Address _address;

        [ServiceDependency]
        public IMdbService _mdbService { get; set; }


        /// <summary>
        /// Конструктор
        /// </summary>
        public CheckAddressService()
        {
            _model = new CurrentAddressInfo();
        }

        #region ICheckAddressService Members

        public CurrentAddressInfo Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
            }
        }

        public int Check(Address address)
        {
            _address = address;

            string curCode = "00";

            if (address.KodReg == "")
            {
                _model.KodReg = "0000000000000";
                curCode = "00";
            }
            else
            {
                _model.KodReg = address.KodReg;
                curCode = address.KodReg;
            }

            if ((address.Raion == "") && (address.Gorod == "") && (address.NasPunkt == "") &&
                (address.Ulica == ""))
            {
                ErrorItem = Address.AddressItems.None;
                return 0;
            }

            // Проверяем район
            if (address.Raion != "")
            {
                _model.Raion = CheckItem(address.Raion, curCode, "KLADR");
                if (_model.Raion != "")
                {
                    _model.Raion = _model.Raion.Remove(5);
                    curCode = _model.Raion;
                }
                else
                {
                    ErrorItem = Address.AddressItems.Raion;
                    return 1;
                }
            }
            else
                _model.Raion = "";

            // Проверяем город
            if (address.Gorod != "")
            {
                _model.Gorod = CheckItem(address.Gorod, curCode, "KLADR");
                if (_model.Gorod != "")
                {
                    _model.Gorod = _model.Gorod.Remove(8);
                    curCode = _model.Gorod;
                }
                else
                {
                    ErrorItem = Address.AddressItems.Gorod;
                    return 1;
                }
            }
            else
                _model.Gorod = "";

            // Проверяем населенный пункт
            if (address.NasPunkt != "")
            {
                _model.NasPunkt = CheckItem(address.NasPunkt, curCode, "KLADR");
                if (_model.NasPunkt != "")
                {
                    _model.NasPunkt = _model.NasPunkt.Remove(11);
                    curCode = _model.NasPunkt;
                }
                else
                {
                    ErrorItem = Address.AddressItems.NasPunkt;
                    return 1;
                }
            }
            else
                _model.NasPunkt = "";

            // Проверяем улицу
            if (address.Ulica != "")
            {
                _model.Ulica = CheckItem(address.Ulica, curCode, "STREET");
                if (_model.Ulica != "")
                {
                    _model.Ulica = _model.Ulica.Remove(13);
                    curCode = _model.Ulica;
                }
                else
                {
                    ErrorItem = Address.AddressItems.Ulica;
                    return 1;
                }
            }
            else
                _model.Ulica = "";

            _model.CurrentCode = curCode;

            AutoUpdateAddress();

            ErrorItem = Address.AddressItems.None;
            return 0;
        }

        public string Raion { get; set; }

        public Address.AddressItems ErrorItem { get; set; }

        #endregion

        private void AutoUpdateAddress()
        {
            // Устанавливаем принудительно район
            if (_model.Raion == "")
            {
                string query = "";
                if (_model.Gorod != "")
                    query = "select * from KLADR where CODE='" + _model.Gorod.Remove(5) + "00000000' AND CODE<>'" + _model.Gorod.Remove(2) + "00000000000'";
                
                if (_model.NasPunkt != "")
                    query = "select * from KLADR where CODE='" + _model.NasPunkt.Remove(5) + "00000000' AND CODE<>'" + _model.NasPunkt.Remove(2) + "00000000000'" ;

                

                if (query != "")
                {
                    DataTable dt = _mdbService.SQL(query);

                    if (dt.Rows.Count != 0)
                    {
                        Raion = dt.Rows[0].ItemArray[0].ToString() + " " + dt.Rows[0].ItemArray[1].ToString();
                        _model.Raion = dt.Rows[0].ItemArray[2].ToString();
                    }
                }
            }
        }

        private string CheckItem(string name, string code, string table)
        {
            string query = "select * from " + table + " where UCase(NAME + ' ' + SOCR) = '" + name.ToUpper() + "' and CODE like '" + code + "%'";
            DataTable dt = _mdbService.SQL(query);

            if (dt.Rows.Count != 0)
            {
                _model.Index = dt.Rows[0].ItemArray[3].ToString(); // Устанавливаем принудительно индекс
                return dt.Rows[0].ItemArray[2].ToString();
            }
            else
                return "";
        }
    }
}