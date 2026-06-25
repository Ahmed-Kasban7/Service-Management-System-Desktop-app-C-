using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.VisitDTOs
{
    public class VisitDetailsDto
    {
        public DateTime ScheduledDate { get; set; }
        public int VisitType { get; set; }
        public string VisitTypeName => VisitType switch
        {
            0=>"كشف" ,
            1=> "إصلاح",
            2=> "تركيب قطع غيار",
            3=> "سحب",
            4=> "تسليم",
            5=> "متابعة"

        };

        public string TechnicianName { get; set; }
        public string? AssistantName { get; set; }
        public string?DriverName { get; set; }
        public string? PaidByEmployeeName { get; set; }


        public string Diagnosis { get; set; }
        public string? ActionsTaken { get; set; }
        public string? Notes { get; set; }

        public decimal TotalCostToCustomer { get; set; }
        public decimal TransportationCost { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal RemainingAmount { get; set; }
        public decimal PartsTransportationCost { get; set; }
        public byte TransportationBearer { get; set; }

        public List<VisitSparePartDto> SpareParts { get; set; } = new();

        public decimal TotalPartsCost => SpareParts?.Sum(s => s.TotalPrice) ?? 0;
    }

    public class VisitSparePartDto
    {
        public string PartName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
