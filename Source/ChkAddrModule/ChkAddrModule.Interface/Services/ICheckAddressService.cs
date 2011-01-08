namespace ACOT.ChkAddrModule.Interface.Services
{
    public interface ICheckAddressService
    {
        ACOT.ChkAddrModule.Interface.BusinessEntities.CurrentAddressInfo Model { get; }

        string Raion { get; }

        int Check(ACOT.ChkAddrModule.Interface.BusinessEntities.Address _address);

        ACOT.ChkAddrModule.Interface.BusinessEntities.Address.AddressItems ErrorItem { get; }
    }
}