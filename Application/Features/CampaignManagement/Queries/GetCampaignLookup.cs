using Application.Common;
using Application.DTOs.CampaignDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CampaignManagement.Queries;

public class GetCampaignLookupHandler
{
    private readonly ICampaignRepository _repository;

    public GetCampaignLookupHandler(ICampaignRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<CampaignLookup> Handle(int sourceId)
    {
        if (sourceId <= 0)
            throw new ArgumentException("رقم المصدر غير صالح");

        return _repository.GetCampaignsBySourceId(sourceId);

    }
}
