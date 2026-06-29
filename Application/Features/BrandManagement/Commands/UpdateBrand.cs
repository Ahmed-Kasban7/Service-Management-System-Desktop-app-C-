using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.BrandManagement.Commands;

public class UpdateBrandHandler
{
    private readonly IDeviceBrandRepository _brandRepository;

    public UpdateBrandHandler(IDeviceBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public bool Handle(int brandId , string Name)
    {
        if (brandId <= 0)
            throw new ArgumentException("رقم الماركة غير صالح");

        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentException("لا يمكن أن يكون اسم الماركة فارغاً.");


        return _brandRepository.UpdateBrand(brandId , Name);
    }
}
