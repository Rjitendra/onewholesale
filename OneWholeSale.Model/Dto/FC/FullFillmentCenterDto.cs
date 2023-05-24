using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace OneWholeSale.Model.Dto.FC
{
    public class FullFillmentCenterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
      
        public string FCCode { get; set; }
        public DateTime Addon { get; set; }
        public int AddBy { get; set; }
        public DateTime? ModOn { get; set; }
        public string? ModBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public List<MapDistrictToFcc> MapDistrictToFcc { get; set; }
        public List<MapSalesPersonToFCc> MapSalesPersonToFCc { get; set; }
    }
    public class MapDistrictToFcc
    {
        public int Id { get; set; }
        public int FcId { get; set; }
        public int DistrictId { get; set; }
    }
    public class MapSalesPersonToFCc
    {
       
        public int Id { get; set; }
        public int FcId { get; set; }
        public int SalesPersonId { get; set; }
    }
}
