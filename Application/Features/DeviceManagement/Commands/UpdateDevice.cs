using Application.DTOs.DeviceDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.DeviceManagement.Commands;

public class UpdateDeviceHandler
{
    private readonly IDeviceRepository _deviceRepository;

    public UpdateDeviceHandler(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public bool Handle(DeviceInfoDTO deviceDto)
    {
        if (deviceDto == null || deviceDto.DeviceId <= 0)
            throw new ArgumentException("بيانات الجهاز غير صالحة للتحديث.");

        return _deviceRepository.UpdateCustomerDevice(deviceDto);
    }
}