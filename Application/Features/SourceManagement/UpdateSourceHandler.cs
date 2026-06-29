using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SourceManagement;

public class UpdateSourceHandler
{
    private readonly ISourceRepository _sourceRepository;

    public UpdateSourceHandler(ISourceRepository sourceRepository)
    {
        _sourceRepository = sourceRepository;
    }

    public bool Handle(int sourceId, string sourceName)
    {
        if (string.IsNullOrWhiteSpace(sourceName))
            throw new ArgumentException("اسم المصدر مطلوب");

        if (sourceId <= 0)
            throw new ArgumentException("رقم المصدر غير صالح");

        return _sourceRepository.UpdateSource(sourceId, sourceName);
    }
}