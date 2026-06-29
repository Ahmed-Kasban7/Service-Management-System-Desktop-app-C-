using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SourceManagement;

public class AddSourceHandler
{
    private readonly ISourceRepository _sourceRepository;

    public AddSourceHandler(ISourceRepository sourceRepository)
    {
        _sourceRepository = sourceRepository;
    }

    public bool Handle(string sourceName)
    {
        if (string.IsNullOrWhiteSpace(sourceName))
            throw new ArgumentNullException("الاسم مطلوب");

        return _sourceRepository.AddSource(sourceName);
    }
}
