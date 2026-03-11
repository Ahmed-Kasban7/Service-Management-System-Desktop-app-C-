using Application.DTOs.DeviceDTOs;
using Application.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class DeviceBrandService
{
    private readonly IDeviceBrandRepository _brandRepository;

    public DeviceBrandService(IDeviceBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public List<BrandDto> GetAllBrands()
    {
        return _brandRepository.GetAllBrands();
    }
    public BrandDto GetBrandBy(int Id)
    {
        return _brandRepository.GetBrandBy(Id);
    }
    public bool AddDeviceBrand(string name)
    {
        return _brandRepository.AddBrand(name);
    }

}
