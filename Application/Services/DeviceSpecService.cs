using Application.DTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class DeviceSpecService
{
    private readonly IDeviceSpecRepository _deviceSpecRepository;

    public DeviceSpecService(IDeviceSpecRepository deviceSpecRepository)
    {
        _deviceSpecRepository = deviceSpecRepository;
    }

    public List<SpecDTO> GetSpecsByTypeId(int typeId)
    {
        return _deviceSpecRepository.GetSpecsByTypeId(typeId);
    }
}
