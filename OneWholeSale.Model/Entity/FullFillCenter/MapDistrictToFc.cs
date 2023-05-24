

namespace OneWholeSale.Model.Entity.FullFillCenter
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("MapDistrictToFc", Schema = "dbo")]
    public class MapDistrictToFc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int FcId { get; set; }
        public int DistrictId { get; set; }
    }

}
