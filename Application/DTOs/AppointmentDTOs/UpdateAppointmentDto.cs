using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.AppointmentDTOs
{
    public class UpdateAppointmentDto
    {
        public int AppointmentId { get; set; }
        public int TechnicianId { get; set; }
        public int? TechnicianAssistantId { get; set; }
        public int? DriverId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public byte VisitType { get; set; }
        public string? Notes { get; set; }
    }
}
