using Application.DTOs.DeviceDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class DeviceTypeService
{
    private readonly IDeviceTypeRepository _typeRepository;

    public DeviceTypeService(IDeviceTypeRepository typeRepository)
    {
        _typeRepository = typeRepository;
    }

    public List<TypeDto> GetAllTypes()
    {
        return _typeRepository.GetAllTypes();
    }

}
