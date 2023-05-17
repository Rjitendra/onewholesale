namespace OneWholeSale.Model.Entity.FullFillCenter
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("MapSalesPersonToFC", Schema = "dbo")]
    public class MapSalesPersonToFC
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int FcId { get; set; }
        public int SalesPersonId { get; set; }
    }
}
