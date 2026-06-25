using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.CampaignDTOs;

public record CampaignSummariesDto(int Id,string CampaignName , string SourceName , DateOnly StartDate , DateOnly EndDate);
