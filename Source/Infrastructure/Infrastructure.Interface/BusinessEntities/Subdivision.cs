using System;
using System.Collections.Generic;
using System.Text;

namespace ChkAddr.Infrastructure.Interface.BusinessEntities
{
    public class Subdivision
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int WorkersCount { get; set; }

        public Subdivision()
        {
            Id = 0; 
            Name = "";
            WorkersCount = 0;
        }
    }
}
