using AutoMapper;
using BabyMoo.DTOs.Address;
using BabyMoo.Models;
using BabyMoo.Services.Addresses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BabyMoo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly IMapper _mapper;

        public AddressController(IAddressService addressService, IMapper mapper)
        {
            _addressService = addressService;
            _mapper = mapper;
        }

        // ✅ Add Address
        [HttpPost]
        public async Task<IActionResult> AddAddress([FromBody] CreateAddressDto addressDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await _addressService.AddAddress(userId, addressDto);
            return Ok(new ApiResponse<AddressDto>(200, "Address added successfully", result));
        }

        // ✅ Get All Addresses
        [HttpGet]
        public async Task<IActionResult> GetAddresses()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await _addressService.GetAddress(userId);
            return Ok(new ApiResponse<IEnumerable<AddressDto>>(200, "Addresses retrieved", result));
        }

        // ✅ Update Address
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] AddressDto addressDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await _addressService.UpdateAddress(userId, addressDto);

            if (result == null)
                return NotFound(new ApiResponse<AddressDto>(404, "Address not found"));

            return Ok(new ApiResponse<AddressDto>(200, "Address updated successfully", result));
        }

        // ✅ Delete Address
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var success = await _addressService.RemoveAddress(userId, id);

            if (!success)
                return NotFound(new ApiResponse<string>(404, "Address not found"));

            return Ok(new ApiResponse<string>(200, "Address deleted successfully"));
        }
    }
}
