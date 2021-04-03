using System;

namespace GilYard.Api.Models
{
    public class VisitorForTable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string CarPlate { get; set; }
        public DateTime? ArriveDate { get; set; }
        public DateTime? LeaveDate { get; set; }
        public int GuardianId { get; set; }
        public string UserName { get; set; }
    }
}