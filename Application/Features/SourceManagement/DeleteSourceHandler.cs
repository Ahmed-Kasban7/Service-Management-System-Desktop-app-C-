using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SourceManagement;

public class DeleteSourceHandler
{
    private readonly ISourceRepository _sourceRepository;

    public DeleteSourceHandler(ISourceRepository sourceRepository)
    {
        _sourceRepository = sourceRepository;
    }

    public bool Handle(int sourceId)
    {
        if (sourceId <= 0)
            throw new ArgumentException("رقم المصدر غير صالح");

        return _sourceRepository.DeleteSource(sourceId);
    }
}
