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

    public bool AddDeviceToCustomer(int customerId, DeviceAddDTO deviceDto)
    {
        if (deviceDto.BrandID <= 0 || deviceDto.TypeID <= 0 || deviceDto.SpecID <= 0)
            throw new ArgumentException("برجاء اختيار النوع والماركة والموصفات.");

        var device = new Device(deviceDto.SerialNumber, deviceDto.Model, deviceDto.BrandID, deviceDto.TypeID, deviceDto.SpecID);

        return _deviceRepository.AddDeviceToCustomer(customerId, device);
    }
}
