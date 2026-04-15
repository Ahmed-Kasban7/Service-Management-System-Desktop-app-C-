using Application.DTOs.DeviceDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.TypeManagement;

public class DeviceTypeService
{
    private readonly IDeviceTypeRepository _typeRepository;

    public DeviceTypeService(IDeviceTypeRepository typeRepository)
    {
        _typeRepository = typeRepository;
    }

    public List<TypeDto> GetAllTypes()
    {
        return _typeRepository.GetAllTypes();
    }

    public bool AddType(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentNullException("لا يمكن أن يكون النوع فارغًا.");

        if(type.Length >100)
            throw new ArgumentException("لا يمكن أن يتجاوز طول النوع 100 حرف.");


        return _typeRepository.AddType(type);
    }
}
