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

        if (Name.Length > 200)
            throw new ArgumentException("لا يمكن أن يتجاوز طول الماركة 200 حرف.");

        return _brandRepository.UpdateBrand(brandId , Name);
    }
}
