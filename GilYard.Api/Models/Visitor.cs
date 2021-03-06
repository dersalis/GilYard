using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GilYard.Api.Models
{
    public class Visitor
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana.")]
        [StringLength(100, ErrorMessage = "Nazwa nie moze być dłuzsza niz 100 znaków.")]
        public string Name { get; set; }

        [StringLength(20, ErrorMessage = "Telefon nie moze być dłuzszy niz 20 znaków.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Numer rejestracyjny jest wymagany.")]
        [StringLength(20, ErrorMessage = "Numer rejestracyjny nie moze być dłuzszy niz 20 znaków.")]
        public string CarPlate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ArriveDate { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime? LeaveDate { get; set; }

        // [Required(ErrorMessage = "Osobą odpowiedzalna jest wymagana")]
        // [ForeignKey("User")]
        public int GuardianId { get; set; }
        public User Guardian { get; set; }

        // [NotMapped]
        // public string UserName { get { return User.Name ?? ""; } }
        
    }
}