﻿namespace BabyMoo.DTOs.Address
{
    public class AddressDto
    {
        public int AddressId { get; set; }
        public string FullName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public int UserId { get; set; }
    }
}
