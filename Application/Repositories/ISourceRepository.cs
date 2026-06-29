using Application.DTOs.SourceDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories;

public interface ISourceRepository
{
    bool AddSource(string sourceName);
    bool UpdateSource(int sourceId, string sourceName);
    bool DeleteSource(int sourceId);
    IEnumerable< SourceDto> GetAllSources();
}
