using Application.DTOs.DeviceDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.BrandManagement.Commands;

public class CreateBrandHandler
{
    private readonly IDeviceBrandRepository _brandRepository;

    public CreateBrandHandler(IDeviceBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public bool Handle(string brand)
    {

        if (string.IsNullOrWhiteSpace(brand))
            throw new ArgumentNullException("لا يمكن أن يكون الماركة فارغًا.");

        if (brand.Length > 200)
            throw new ArgumentException("لا يمكن أن يتجاوز طول النوع 200 حرف.");

        return _brandRepository.AddBrand(brand);
    }
}
