using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ACOT.ChkAddrModule.Interface.BusinessEntities;
using ACOT.ChkAddrModule.Interface.Services;

namespace ACOT.ChkAddrModule.Tests.Mocks
{
    public class MockCheckAddressService : ICheckAddressService
    {
        private CurrentAddressInfo _model;

        public MockCheckAddressService()
        {
            _model = new CurrentAddressInfo();
            _model.Index = "152915";
        }

        #region ICheckAddressService Members

        public ACOT.ChkAddrModule.Interface.BusinessEntities.CurrentAddressInfo Model
        {
            get
            {
                return _model;
            }
        }

        public string Raion
        {
            get { throw new NotImplementedException(); }
        }

        public int Check(ACOT.ChkAddrModule.Interface.BusinessEntities.Address _address)
        {
            return 0;
        }

        public ACOT.ChkAddrModule.Interface.BusinessEntities.Address.AddressItems ErrorItem
        {
            get
            {
                return Address.AddressItems.None;
            }
        }

        #endregion
    }
}
