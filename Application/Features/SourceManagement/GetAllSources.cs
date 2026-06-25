using Application.DTOs.SourceDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SourceManagement;

public class GetAllSourcesHandler
{
    private readonly ISourceRepository _sourceRepository;

    public GetAllSourcesHandler(ISourceRepository sourceRepository)
    {
        _sourceRepository = sourceRepository;
    }

    public IEnumerable<SourceDto> Handle()
    {
        return _sourceRepository.GetAllSources();
    }
}
