using Application.DTOs.CampaignDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CampaignManagement.Command;

public class UpdateCampaignHandler
{
    private readonly ICampaignRepository _campaignRepository;

    public UpdateCampaignHandler(ICampaignRepository campaignRepository)
    {
        _campaignRepository = campaignRepository;
    }

    public bool Handle(UpdateCampaignDto campaign)
    {
        if (campaign.CampaignId <= 0 || string.IsNullOrWhiteSpace(campaign.CampaignName))
            throw new ArgumentException("بيانات الحملة غير صالحة.");

        return _campaignRepository.UpdateCampaign(campaign);
    }
}