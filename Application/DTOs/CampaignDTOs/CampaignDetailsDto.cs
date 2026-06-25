using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.CampaignDTOs;

public record CampaignDetailsDto(string CampaignName , string SourceName, DateOnly StartDate , DateOnly EndDate , int Discount , string? Note , decimal CampaignCost);