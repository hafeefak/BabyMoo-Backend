﻿namespace BabyMoo.DTOs.Address
{
    public class CreateAddressDto
    {
        public string FullName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PinCode { get; set; }
        public string Country { get; set; }
    }
}
