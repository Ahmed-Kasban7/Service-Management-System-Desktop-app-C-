using Application.Common;
using Application.DTOs.CampaignDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CampaignManagement.Queries;

public class GetCampaignDetailsHandler
{
    private readonly ICampaignRepository _repository;

    public GetCampaignDetailsHandler(ICampaignRepository repository)
    {
        _repository = repository;
    }

    public CampaignDetailsDto Handle(int Id)
    {
        if (Id <= 0)
            throw new ArgumentException("رقم الحملة غير صالح");

        return _repository.GetCampaignDetails(Id);

    }
}
