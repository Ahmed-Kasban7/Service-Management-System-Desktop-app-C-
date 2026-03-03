using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces;

public interface IDevcieRepository
{
    bool AddDeviceToCustomer(int customerId, Device device);
    public List<DeviceInfoDTO> GetCustomerDevicesBy(int customerId);
    public bool DeleteCustomerDevice(int deviceId);
    public bool UpdateCustomerDevice(DeviceInfoDTO deviceDto );

}
