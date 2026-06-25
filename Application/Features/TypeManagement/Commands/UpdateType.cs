using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.TypeManagement.Commands;

public class UpdateTypeHandler
{

    private readonly IDeviceTypeRepository _typeRepository;

    public UpdateTypeHandler(IDeviceTypeRepository typeRepository)
    {
        _typeRepository = typeRepository;
    }

    public bool Handle(int brandId, string Name)
    {


        if (brandId <= 0)
            throw new ArgumentException("رقم النوع غير صالح");

        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentException("لا يمكن أن يكون اسم النوع فارغاً.");

        if (Name.Length > 200)
            throw new ArgumentException("لا يمكن أن يتجاوز طول النوع 200 حرف.");

        return _typeRepository.UpdateType(brandId, Name);
    }
}
