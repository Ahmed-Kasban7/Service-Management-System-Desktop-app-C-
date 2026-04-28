using Application.DTOs.DeviceDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.BrandManagement.Queries;

public class GetAllBrandsHandler
{
    private readonly IDeviceBrandRepository _brandRepository;

    public GetAllBrandsHandler(IDeviceBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public IEnumerable<BrandDto> Handle()
    {
        return _brandRepository.GetAllBrands();
    }
}

