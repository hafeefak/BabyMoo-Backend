using AutoMapper;
using BabyMoo.Data;
using BabyMoo.DTOs.Address;
using BabyMoo.Models;
using BabyMoo.Middleware;
using Microsoft.EntityFrameworkCore;

namespace BabyMoo.Services.Addresses
{
    public class AddressService : IAddressService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AddressService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AddressDto>> GetAddress(int userId)
        {
            var addresses = await _context.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<AddressDto>>(addresses);
        }

        public async Task<AddressDto> AddAddress(int userId, CreateAddressDto addressDto)
        {
            var address = _mapper.Map<Address>(addressDto);
            address.UserId = userId;

            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();

            return _mapper.Map<AddressDto>(address);
        }

        public async Task<AddressDto> UpdateAddress(int userId, AddressDto addressDto)
        {
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.UserId == userId && a.AddressId == addressDto.AddressId);

            if (address == null)
                throw new NotFoundException("Address not found");

            _mapper.Map(addressDto, address);
            await _context.SaveChangesAsync();

            return _mapper.Map<AddressDto>(address);
        }

        public async Task RemoveAddress(int userId, int addressId)
        {
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.UserId == userId && a.AddressId == addressId);

            if (address == null)
                throw new NotFoundException("Address not found");

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
        }
    }
}
