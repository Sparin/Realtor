using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Realtor.Model.Entities
{
    public class Customer : IdentityUser<int>
    {
        public IEnumerable<Advertisement> Advertisements { get; set; }
        public IEnumerable<Phone> Phones { get; set; }
    }
}
