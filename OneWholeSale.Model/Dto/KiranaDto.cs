using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneWholeSale.Model.Dto
{
    public class KiranaDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string KiranaCode { get; set; }
        public int PartnerId { get; set; }
        public DateTime Addon { get; set; }
        public string AddBy { get; set; }
        public DateTime? ModOn { get; set; }
        public string? ModBy { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Delete { get; set; }
    }
}
