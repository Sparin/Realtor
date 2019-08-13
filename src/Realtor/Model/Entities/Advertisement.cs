using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Realtor.Model.Entities
{
    public class Advertisement
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }

        public AdvertisementType Type { get; set; }

        [StringLength(100, ErrorMessage = "The short description is too long")]
        public string ShortDescription { get; set; }

        [Range(1, 100, ErrorMessage = "Too many rooms in your apartment. Maximum 100 rooms")]
        public int RoomsCount { get; set; }

        [Column(TypeName = "double(12, 2)")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid price")]
        [Range(0,double.MaxValue)]
        public double Price { get; set; }

        public Customer Author { get; set; }
    }
}
