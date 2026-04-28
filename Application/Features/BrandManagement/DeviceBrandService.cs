using Application.DTOs.DeviceDTOs;
using Application.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.BrandManagement;

public class DeviceBrandService
{
    private readonly IDeviceBrandRepository _brandRepository;

    public DeviceBrandService(IDeviceBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public IEnumerable<BrandDto> GetAllBrands()
    {
        return _brandRepository.GetAllBrands();
    }
    public BrandDto GetBrandBy(int Id)
    {
        return _brandRepository.GetBrandBy(Id);
    }
    public bool AddDeviceBrand(string brand)
    {

        if (string.IsNullOrWhiteSpace(brand))
            throw new ArgumentNullException("لا يمكن أن يكون الماركة فارغًا.");

        if (brand.Length > 100)
            throw new ArgumentException("لا يمكن أن يتجاوز طول النوع 100 حرف.");

        return _brandRepository.AddBrand(brand);
    }

}
