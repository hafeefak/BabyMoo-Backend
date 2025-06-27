using BabyMoo.DTOs.Address;

namespace BabyMoo.Services.Addresses
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressDto>?> GetAddress(int userId);
        Task<AddressDto> AddAddress(int userId, CreateAddressDto addressDto);
        Task<bool> RemoveAddress(int userId, int addressId);
        Task<AddressDto> UpdateAddress(int userId, AddressDto addressDto);
    }
}
