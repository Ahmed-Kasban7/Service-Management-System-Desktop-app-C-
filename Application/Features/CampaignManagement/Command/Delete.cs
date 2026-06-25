using Application.DTOs.CampaignDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CampaignManagement.Command;

public class DeleteCampaignHandler
{

    private readonly ICampaignRepository _repository;
    public DeleteCampaignHandler(ICampaignRepository repository)
    {
        _repository = repository;
    }
    public bool Handle(int id )
    {
        return _repository.DeleteCampaign( id );
    }
}
