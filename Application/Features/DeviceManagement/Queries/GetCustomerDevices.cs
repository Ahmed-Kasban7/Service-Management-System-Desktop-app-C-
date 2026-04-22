using Application.Common;
using Application.DTOs.DeviceDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.DeviceManagement.Queries;

public class GetCustomerDevicesHandler
{
    private readonly IDeviceRepository _deviceRepository;

    public GetCustomerDevicesHandler(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public Result<IEnumerable<CustomerDeviceLookupDto>> Handle(int customerId)
    {
        if (customerId <= 0)
            return Result<IEnumerable<CustomerDeviceLookupDto>>.Failure("رقم العميل غير صالح");

        return Result< IEnumerable <CustomerDeviceLookupDto>>.Success(_deviceRepository.GetCustomerDevicesLookup(customerId));
    }
}
