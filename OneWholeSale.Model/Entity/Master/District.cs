using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneWholeSale.Model.Entity.Master
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("District", Schema = "dbo")]
    public class District
    {
        [Key]
        public int Id { get; set; }
        public int StateId { get; set; }
        public string DistrictName { get; set; }
        public DateTime? Addon { get; set; }
        public int Addby { get; set; }
        public DateTime? ModOn { get; set; }
        public int Modby { get; set; }
        public bool? Is_Active { get; set; }
        public bool? Is_Delete { get; set; }
    }
}
