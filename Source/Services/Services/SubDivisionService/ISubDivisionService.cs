using System;
using System.Collections.Generic;
using System.Text;

namespace ACOT.Services.SubDivisionService
{
    public interface ISubDivisionService
    {
        List<ACOT.Infrastructure.Interface.BusinessEntities.Subdivision> GetSubDivisions();
    }
}
