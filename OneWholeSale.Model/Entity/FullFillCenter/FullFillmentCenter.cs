namespace OneWholeSale.Model.Entity.FullFillCenter
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("FullFillmentCenter", Schema = "dbo")]
    public class FullFillmentCenter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string FCCode { get; set; }
        public DateTime Addon { get; set; }
        public int AddBy { get; set; }
        public DateTime? ModOn { get; set; }
        public string? ModBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }

    }
}
