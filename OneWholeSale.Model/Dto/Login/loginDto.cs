namespace OneWholeSale.Model.Dto.Login
{
    using System.ComponentModel.DataAnnotations;
    public class loginDto
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
