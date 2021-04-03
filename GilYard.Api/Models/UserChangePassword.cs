using System.ComponentModel.DataAnnotations;

namespace GilYard.Api.Models
{
    public class UserChangePassword
    {
        [Required(ErrorMessage = "E-mail jest wymagany.")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu e-mail.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Stare hasło jest wymagane.")]
        [StringLength(255, ErrorMessage = "Stare hasło musi być dłuzsze niz 8 znaków, ale krótsze od 255 znaków.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [StringLength(255, ErrorMessage = "Hasło musi być dłuzsze niz 8 znaków, ale krótsze od 255 znaków.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [StringLength(255, ErrorMessage = "Hasło musi być dłuzsze niz 8 znaków, ale krótsze od 255 znaków.", MinimumLength = 8)]
        [Compare("NewPassword", ErrorMessage = "Hasło i potwierdzenie hasło muszą być równe.")]
        public string ConfirmNewPassword { get; set; }
    }
}