using System;

namespace GilYard.Api.Models
{
    public class ReportMyVisitors
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime? ArriveDate { get; set; }
        public DateTime? LeaveDate { get; set; }
    }
}