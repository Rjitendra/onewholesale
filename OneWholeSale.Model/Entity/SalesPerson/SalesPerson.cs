namespace OneWholeSale.Model.Entity.SalesPerson
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("SalesPerson", Schema = "dbo")]
    public class SalesPerson
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
        public int Country { get; set; }
        public int State { get; set; }
        public int District { get; set; }
        public string SalesPersonCode { get; set; }
        public DateTime AddOn { get; set; }
        public int AddBy { get; set; }
        public DateTime? ModOn { get; set; }
        public int? ModBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
