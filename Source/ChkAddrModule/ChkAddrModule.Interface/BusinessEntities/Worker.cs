using System;
using System.Collections.Generic;
using System.Text;

namespace ACOT.ChkAddrModule.Interface.BusinessEntities
{
    public class Worker
    {
        private int _tbn;
        private string _name;
        private Address _address;

        public Worker()
        {
            _address = new Address();   
        }
    }
}
