using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.CampaignDTOs;

public class CreateCampaignDto
{
    public string CampaignName { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int SourceId { get; set; }
    public int Discount { get; set; } = 0;

    public decimal CampaignCost { get; set; } = 0;
    public string? Note { get; set; }

}
