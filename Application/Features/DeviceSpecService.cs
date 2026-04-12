using Application.DTOs.DeviceDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class DeviceSpecService
{
    private readonly IDeviceSpecRepository _deviceSpecRepository;

    public DeviceSpecService(IDeviceSpecRepository deviceSpecRepository)
    {
        _deviceSpecRepository = deviceSpecRepository;
    }

    public List<SpecDto> GetSpecsByTypeId(int typeId)
    {
        return _deviceSpecRepository.GetSpecsByTypeId(typeId);
    }

    public List<SpecDto> GetAllSpecs()
    {
        return _deviceSpecRepository.GetAllSpecs();
    }
    public bool AddSpec(string spec , int TypeId )
    {
        if (string.IsNullOrWhiteSpace(spec))
            throw new ArgumentNullException("لا يمكن أن يكون النوع فارغًا.");

        if (spec.Length > 100)
            throw new ArgumentException("لا يمكن أن يتجاوز طول النوع 100 حرف.");

        if (TypeId <= 0)
            throw new ArgumentException("ادخل نوع صحيح");

        return _deviceSpecRepository.AddSpec(spec, TypeId);
    }
}
