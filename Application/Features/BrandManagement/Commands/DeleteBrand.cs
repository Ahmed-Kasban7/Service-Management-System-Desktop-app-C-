using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.BrandManagement.Commands;

public class DeleteBrandHandler
{
    private readonly IDeviceBrandRepository _brandRepository;

    public DeleteBrandHandler(IDeviceBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public bool Handle(int brandId)
    {
        if (brandId <= 0)
            throw new ArgumentException("رقم الماركة غير صالح");

        
        return _brandRepository.DeleteBrand(brandId);
    }
}
