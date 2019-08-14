using Realtor.Model.DTO.Phone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Realtor.Model.DTO.Customer
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public IEnumerable<PhoneDto> Phones { get; set; }
    }
}
