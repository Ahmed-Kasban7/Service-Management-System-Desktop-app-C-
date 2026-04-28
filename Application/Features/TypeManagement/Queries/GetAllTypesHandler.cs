using Application.DTOs.DeviceDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.TypeManagement.Queries;

public class GetAllTypesHandler
{
    private readonly IDeviceTypeRepository _typeRepository;

    public GetAllTypesHandler(IDeviceTypeRepository typeRepository)
    {
        _typeRepository = typeRepository;
    }

    public IEnumerable<TypeDto> Handle()
    {
        return _typeRepository.GetAllTypes();
    }
}
