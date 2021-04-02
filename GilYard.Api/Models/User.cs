using System.ComponentModel.DataAnnotations;

namespace GilYard.Api.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana.")]
        [StringLength(100, ErrorMessage = "Nazwa nie moze być dłuzsza niz 100 znaków.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "E-mail jest wymagany.")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu e-mail.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [StringLength(255, ErrorMessage = "Hasło musi być dłuzsze niz 8 znaków, ale krótsze od 255 znaków.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Rola jest wymagana")]
        [StringLength(40, ErrorMessage = "Rola nie moze być dłuzsza niz 40 znaków.")]
        public string Role { get; set; }

        public string Phone { get; set; }
        
        public bool CanSendEmail { get; set; }
        
        public bool CanSendSms { get; set; }
        
        public bool IsActive { get; set; }
        
        
    }
}