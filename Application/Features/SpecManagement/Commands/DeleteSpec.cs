using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SpecManagement.Commands;

public class DeleteSpecHandler
{
    private readonly IDeviceSpecRepository _deviceSpecRepository;

    public DeleteSpecHandler(IDeviceSpecRepository deviceSpecRepository)
    {
        _deviceSpecRepository = deviceSpecRepository;
    }

    public bool Handle(int SpecId)
    {
        if (SpecId <= 0)
            throw new ArgumentException("رقم الموصفة غير صالح");

        return _deviceSpecRepository.DeleteSpec(SpecId);
    }
}
