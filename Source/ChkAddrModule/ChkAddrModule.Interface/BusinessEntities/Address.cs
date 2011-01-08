﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ACOT.ChkAddrModule.Interface.BusinessEntities
{
    public class Address
    {   
        public Address()
        {
            
        }

        public string TBN { get; set; }
        public string Index { get; set; }
        public string KodStran { get; set; }
        public string KodReg { get; set; }
        public string Raion { get; set; }
        public string Gorod { get; set; }
        public string NasPunkt { get; set; }
        public string Ulica { get; set; }
        public string Dom { get; set; }
        public string Korp { get; set; }
        public string Kvart { get; set; }

        public enum AddressItems
        {
            None,
            KodReg,
            Raion,
            Gorod,
            NasPunkt,
            Ulica
        } ;

    }
}
