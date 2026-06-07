using Application.DTOs.CustomerDTOs;
using Application.DTOs.DeviceDTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories;

public interface IDeviceRepository
{
    bool AddDeviceToCustomer(int customerId, Device device);
    public IEnumerable<DeviceInfoDTO> GetCustomerDevices(int customerId);
    public bool DeleteCustomerDevice(int deviceId);
    public bool UpdateCustomerDevice(DeviceInfoDTO deviceDto );
     
    public bool IsDeviceExist(int deviceId);
    public bool IsDeviceAssignedToCustomer(int deviceId, int customerId);
    IEnumerable<CustomerDeviceLookupDto> GetCustomerDevicesLookup(int customerId);
}
