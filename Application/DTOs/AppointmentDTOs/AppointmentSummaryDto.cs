using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.AppointmentDTOs
{
    public class AppointmentSummaryDto
    {
        public int AppointmentId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string TechnicianName { get; set; }
        public string AssistantName { get; set; }
        public string DriverName { get; set; }
        public string? Notes { get; set; } 

        public string VisitTypeName => VisitType switch
        {
            0 => "كشف",
            1 => "إصلاح",
            2 => "تركيب قطع غيار",
            3 => "سحب",
            4 => "تسليم",
            5 => "متابعة",
            _ => "غير معروف"
        };
        public string AppointmentStateName => AppointmentState switch
        {
            0 => "مجدول",
            1 => "لم يُنجز",
            2 => "مكتمل",
            3 => "ملغي",
            _ => "غير معروف"
        };
        public byte VisitType { get; set; }
        public byte AppointmentState { get; set; }
    }
}
