using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Realtor.Model.Entities
{
    public class Phone
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }

        [Phone]
        public string Number { get; set; }

        public Customer Customer { get; set; }
    }
}
