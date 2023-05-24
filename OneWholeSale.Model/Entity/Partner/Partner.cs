
namespace OneWholeSale.Model.Entity.Partner
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("Partner", Schema = "dbo")]
    public class Partner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
       
        public string PartnerCode { get; set; }
        public int FulfillmentCenterId { get; set; }
        

        public DateTime Addon { get; set; }
        public int AddBy { get; set; }
        public DateTime? ModOn { get; set; }
        public string? ModBy { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Delete { get; set; }
        public int AssignedSalesPeronId { get; set; }
        

    }
}
