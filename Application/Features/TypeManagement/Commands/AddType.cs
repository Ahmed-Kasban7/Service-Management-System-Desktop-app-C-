using Application.DTOs.DeviceDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.TypeManagement.Commands;

public class AddTypeHandler
{

    private readonly IDeviceTypeRepository _typeRepository;

    public AddTypeHandler(IDeviceTypeRepository typeRepository)
    {
        _typeRepository = typeRepository;
    }

    public bool Handle(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentNullException("لا يمكن أن يكون النوع فارغًا.");

        if (type.Length > 100)
            throw new ArgumentException("لا يمكن أن يتجاوز طول النوع 200 حرف.");


        return _typeRepository.AddType(type);
    }
}
