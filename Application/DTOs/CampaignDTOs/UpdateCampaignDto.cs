using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.CampaignDTOs;

public record UpdateCampaignDto(int CampaignId , string CampaignName , DateOnly StartDate , DateOnly EndDate , int SourceId , int Discount , string? Note  );
