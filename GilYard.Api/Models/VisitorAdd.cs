using System;
using System.ComponentModel.DataAnnotations;

namespace GilYard.Api.Models
{
    public class VisitorAdd
    {
        [Required(ErrorMessage = "Nazwa jest wymagana.")]
        [StringLength(100, ErrorMessage = "Nazwa nie moze być dłuzsza niz 100 znaków.")]
        public string Name { get; set; }

        [StringLength(20, ErrorMessage = "Telefon nie moze być dłuzszy niz 20 znaków.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Numer rejestracyjny jest wymagany.")]
        [StringLength(20, ErrorMessage = "Numer rejestracyjny nie moze być dłuzszy niz 20 znaków.")]
        public string CarPlate { get; set; }

        [Required(ErrorMessage = "Osobą odpowiedzalna jest wymagana")]
        public int GuardianId { get; set; }
    }
}