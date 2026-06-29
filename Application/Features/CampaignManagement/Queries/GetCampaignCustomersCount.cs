using Application.DTOs.CampaignDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CampaignManagement.Queries;

public class GetCampaignCustomersCount
{
    private readonly ICampaignRepository _repository;

    public GetCampaignCustomersCount(ICampaignRepository repository)
    {
        _repository = repository;
    }

    public int Handle(int Id)
    {
        if (Id <= 0)
            throw new ArgumentException("رقم الحملة غير صالح");

        return _repository.GetCampaignCustomersCount(Id);

    }
}
