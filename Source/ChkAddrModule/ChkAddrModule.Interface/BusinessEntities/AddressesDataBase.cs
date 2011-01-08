using System;
using System.Collections.Generic;
using System.Text;

namespace ACOT.ChkAddrModule.Interface.BusinessEntities
{
    public class AddressDatabase
    {
        private List<Address> _adrDB;

        public AddressDatabase()
        {
            _adrDB = new List<Address>();
        }
    }
}
