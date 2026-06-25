using Application.Common;
using Application.DTOs.CampaignDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface  ICampaignRepository
    {
        int Create(CreateCampaignDto newCampaign);
        PagedResult<CampaignSummariesDto> GetPagedCampaignSummaries(int pageNumber, int pageSize);
        PagedResult<CampaignSummariesDto> SearchCampaignsPaged(string searchWord, int pageNumber, int pageSize);
         CampaignDetailsDto GetCampaignDetails(int id);
        bool DeleteCampaign(int campaignId);
        bool UpdateCampaign(UpdateCampaignDto campaign);
        IEnumerable<CampaignLookup> GetCampaignsBySourceId(int sourceId);




    }
}
