using Application.DTOs.Treasury;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.TreasuryManagement;

public class GetBalanceHandler
{
    private readonly ITreasuryRepository _treasuryRepository;

    public GetBalanceHandler(ITreasuryRepository treasuryRepository)
    {
        _treasuryRepository = treasuryRepository;
    }

    public BalanceDTOs Handle()
    {
        return _treasuryRepository.GetTreasuryOverview();
    }
}
