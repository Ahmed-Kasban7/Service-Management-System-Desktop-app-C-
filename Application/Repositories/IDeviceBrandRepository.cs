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
    bool AddBrand(string brandName);
    bool DeleteBrand(int id);
    bool UpdateBrand(int id, string brandName);


}
