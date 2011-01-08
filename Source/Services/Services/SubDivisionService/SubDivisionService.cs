using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ACOT.Infrastructure.Interface.BusinessEntities;

namespace ACOT.Services.SubDivisionService
{
    public class SubDivisionService : ISubDivisionService
    {

        private string spraw = "";
        private List<Subdivision> subdivisions;
        private int[] subdivisionsCount = null;

        public SubDivisionService()
        {
            subdivisions = new List<Subdivision>();    
        }

        #region ISubDivisionService Members

        public List<Subdivision> GetSubDivisions()
        {
            subdivisions.Clear();
            string orgname = GetOrgnameExtention();

            if (orgname != null)
            {
                spraw = Environment.CurrentDirectory + "\\SPRAW." + orgname;
                int jm = GetLastSubDivisionsNumber();

                using (FileStream fs = new FileStream(spraw, FileMode.Open, FileAccess.Read))
                {
                    int i = 0;
                    byte[] buf2 = new byte[2];
                    byte[] buf20 = new byte[20];

                    while ((i < jm) && (jm > 0))
                    {
                        Subdivision subdiv = new Subdivision();

                        fs.Seek(96 + 2 * i, SeekOrigin.Begin);
                        fs.Read(buf2, 0, 2);
                        subdiv.WorkersCount = BitConverter.ToInt16(buf2, 0);

                        if (subdiv.WorkersCount != 0)
                        {
                            fs.Seek(200 + i * 20, SeekOrigin.Begin);
                            fs.Read(buf20, 0, 20);
                        }

                        subdiv.Id = i++ + 1;
                        subdiv.Name =
                            Encoding.UTF8.GetString(Encoding.Convert(Encoding.GetEncoding(866), Encoding.UTF8, buf20));
                        subdivisions.Add(subdiv);
                    }
                }
                return this.subdivisions;
            }
            return null;
        }

        private int GetLastSubDivisionsNumber()
        {
            byte[] buf2 = new byte[2];
            using (FileStream fs = new FileStream(spraw, FileMode.Open, FileAccess.Read))
            {
                fs.Seek(82, SeekOrigin.Begin);
                fs.Read(buf2, 0, 2);
                fs.Close();
            }
            return BitConverter.ToInt16(buf2, 0);;
        }

        private string GetOrgnameExtention()
        {
            ACOT.Services.OrgnameService.IOrgnameService orgname = new ACOT.Services.OrgnameService.OrgnameService();
            return orgname.GetOrgname();
        }

        public int GetWorkersCount(int subdivisionIndex)
        {
            return this.subdivisionsCount[subdivisionIndex];
        }

        #endregion
    }
}
