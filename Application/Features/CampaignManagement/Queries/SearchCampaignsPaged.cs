using Application.Common;
using Application.DTOs.CampaignDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CampaignManagement.Queries;

public class SearchCampaignsPaged
{
    private readonly ICampaignRepository _repository;

    public SearchCampaignsPaged(ICampaignRepository repository)
    {
        _repository = repository;
    }

    public PagedResult<CampaignSummariesDto> Handle(string searchWord, int pageNumber, int PageSize)
    {
        if (pageNumber <= 0)
            pageNumber = 1;

        if (PageSize <= 0 || PageSize > 100)
            PageSize = 8;

        if (string.IsNullOrWhiteSpace(searchWord))
            searchWord = "";

        return _repository.SearchCampaignsPaged(searchWord,pageNumber, PageSize);

    }
}
