using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneWholeSale.Model.Entity.SalesPerson
{
   public  class Vw_SalesPerson
    {
        [Key]
      public int Id { get; set; }
       public string Name { get; set; }

        public string Address { get; set; }

        public string Country { get; set; }
        public string State { get; set; }
        public int District { get; set; }
        public DateTime AddOn { get; set; }

        public bool? IsActive { get; set; }
        public string SalesPersonCode { get; set; }
        public string DistrictName { get; set; }
    }
}
