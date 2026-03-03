using Application.Common.Interfaces;
using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class DeviceService 
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IDevcieRepository _deviceRepository;

    public DeviceService(ICustomerRepository customerRepository, IDevcieRepository deviceRepository)
    {
        _customerRepository = customerRepository;
        _deviceRepository = deviceRepository;
    }

    public bool AddDeviceToCustomer(int customerId, DeviceAddDTO deviceDto)
    {
        var device = new Device(deviceDto.SerialNumber, deviceDto.Model, deviceDto.BrandID, deviceDto.TypeID, deviceDto.SpecID);
        
        return _deviceRepository.AddDeviceToCustomer(customerId, device);
    }

    public bool DeleteCustomerDevice(int deviceId)
    {
        bool deleted = _deviceRepository.DeleteCustomerDevice(deviceId);

        if (!deleted)
            throw new Exception("لا يمكن حذف هذا الجهاز لأنه يجب أن يكون لعميل جهاز واحد على الأقل.");

        return true;
    }
    public bool UpdateCustomerDevice(DeviceInfoDTO deviceDto)
    {
        if (deviceDto == null || deviceDto.DeviceId <= 0)
            throw new ArgumentException("بيانات الجهاز غير صالحة للتحديث.");

        return _deviceRepository.UpdateCustomerDevice(deviceDto);
    }

}
