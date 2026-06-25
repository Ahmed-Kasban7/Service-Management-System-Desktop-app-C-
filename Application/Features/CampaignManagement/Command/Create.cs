using Application.DTOs.CampaignDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Application.Features.CampaignManagement.Command;
public class CreateCampaignHandler
{
    private readonly ICampaignRepository _repository;
    public event Action CampaignCreated;
    public CreateCampaignHandler(ICampaignRepository repository)
    {
        _repository = repository;
    }
    public int Handle(CreateCampaignDto newCampaign)
    {
        int newCampaignId = _repository.Create(newCampaign);

        if (newCampaignId > 0)
        {
            CampaignCreated?.Invoke();
        }

        return newCampaignId;
    }
}