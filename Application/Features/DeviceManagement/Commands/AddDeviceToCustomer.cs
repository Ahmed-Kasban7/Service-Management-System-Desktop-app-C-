using Application.Common;
using Application.DTOs.DeviceDTOs;
using Application.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.DeviceManagement.Commands;

public class AddDeviceToCustomerHandler
{
    private readonly IDeviceRepository _deviceRepository;

    public AddDeviceToCustomerHandler(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public Result<int> AddDeviceToCustomer(int customerId, DeviceAddDTO deviceDto)
    {
        if (deviceDto.BrandID <= 0 || deviceDto.TypeID <= 0 || deviceDto.SpecID <= 0)
            return Result<int>.Failure( "برجاء اختيار النوع والماركة والموصفات.");

        var device = new Device(deviceDto.SerialNumber, deviceDto.Model, deviceDto.BrandID, deviceDto.TypeID, deviceDto.SpecID);

        int deviceId = _deviceRepository.AddDeviceToCustomer(customerId, device);

        return Result<int>.Success(deviceId);
    }
}
