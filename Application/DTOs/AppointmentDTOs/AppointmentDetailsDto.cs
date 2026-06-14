using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.AppointmentDTOs
{
    public class AppointmentDetailsDto
    {
        public int AppointmentId { get; set; }
        public int OrderId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public byte VisitType { get; set; }
        public byte AppointmentState { get; set; }
        public string? Notes { get; set; }
        public int TechnicianId { get; set; }
        public int? TechnicianAssistantId { get; set; }
        public int? DriverId { get; set; }
    }
}
