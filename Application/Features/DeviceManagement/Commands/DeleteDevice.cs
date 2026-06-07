using Application.Common;
using Application.Features.OrderManagement.Queries;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.DeviceManagement.Commands;

public class DeleteDeviceHandler
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly GetDeviceOrders _getDeviceOrders;

    public DeleteDeviceHandler(IDeviceRepository deviceRepository, GetDeviceOrders getDeviceOrders)
    {
        _deviceRepository = deviceRepository;
        _getDeviceOrders = getDeviceOrders;
    }

    public Result Handle(int deviceId)
    {
        if (deviceId <= 0)
            return Result.Failure("رقم الجهاز غير صحيح");

        var deviceOrders = _getDeviceOrders.Handle(deviceId);
        if (deviceOrders != null && deviceOrders.Any())
            return Result.Failure("لا يمكن حذف هذا الجهاز لأن له طلبات صيانة مسجلة");

        bool deleted = _deviceRepository.DeleteCustomerDevice(deviceId);
        if (!deleted)
            return Result.Failure("لا يمكن حذف هذا الجهاز لأنه يجب أن يكون لعميل جهاز واحد على الأقل");

        return Result.Success();

    }
}
