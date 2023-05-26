

namespace OneWholeSale.Model.Entity.Kirana
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("Kirana", Schema = "dbo")]
    public  class Kirana
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
