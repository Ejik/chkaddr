using System.Data;

namespace ACOT.ChkAddrModule.Interface.Services
{
    public interface IMdbService
    {
        System.Data.DataTable SQL(string query);
        DataTable KladrTable { get;}
        DataTable StreetTable { get; }
        DataTable RegionsTable { get; }
        void IndexingRun();
        //void IndexingRun(System.Windows.Forms.UserControl view);
        void Dispose();
    }
}