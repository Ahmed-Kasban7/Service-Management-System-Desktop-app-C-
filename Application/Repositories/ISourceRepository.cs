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
    IEnumerable< SourceDto> GetAllSources();
}
