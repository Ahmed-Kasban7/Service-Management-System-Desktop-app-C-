using Application.Common;
using Application.DTOs.CampaignDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CampaignManagement.Queries;

public class GetPagedCampaignSummariesHandler
{
    private readonly ICampaignRepository _repository;

    public GetPagedCampaignSummariesHandler(ICampaignRepository repository)
    {
        _repository = repository;
    }

    public PagedResult<CampaignSummariesDto> Handle(int pageNumber, int PageSize)
    {
        if (pageNumber <= 0)
            pageNumber = 1;

        if (PageSize <= 0 || PageSize > 100)
            PageSize = 8;

        return _repository.GetPagedCampaignSummaries(pageNumber , PageSize);

    }
}
