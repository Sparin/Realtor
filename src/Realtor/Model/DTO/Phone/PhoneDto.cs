using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Realtor.Model.DTO.Phone
{
    public class PhoneDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }

        public string Number { get; set; }
    }
}
