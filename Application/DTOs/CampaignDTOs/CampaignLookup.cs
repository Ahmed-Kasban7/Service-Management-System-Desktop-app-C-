using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.CampaignDTOs;

public record CampaignLookup(int CampaignId , string CampaignName , int Discount);
