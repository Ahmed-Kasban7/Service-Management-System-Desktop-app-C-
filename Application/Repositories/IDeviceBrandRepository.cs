using Application.DTOs.DeviceDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories;

public interface IDeviceBrandRepository
{
    IEnumerable<BrandDto> GetAllBrands();
    BrandDto GetBrandBy(int id);
    public bool AddBrand(string brandName);
}
