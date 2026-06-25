using Application.Repositories;
using System;

namespace Application.Features.SpecManagement.Commands;

public class UpdateSpecHandler
{
    private readonly IDeviceSpecRepository _deviceSpecRepository;

    public UpdateSpecHandler(IDeviceSpecRepository deviceSpecRepository)
    {
        _deviceSpecRepository = deviceSpecRepository;
    }

    public bool Handle(int specId, string newSpecName)
    {
        if (specId <= 0)
            throw new ArgumentException("رقم المواصفة غير صالح");

        if (string.IsNullOrWhiteSpace(newSpecName))
            throw new ArgumentException("لا يمكن أن يكون اسم المواصفة فارغاً");

        return _deviceSpecRepository.UpdateSpec(specId, newSpecName.Trim());
    }
}