using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories;

public interface IDeviceBrandRepository
{
    List<BrandDTO> GetAllBrands();
    BrandDTO GetBrandBy(int id);
    public bool AddBrand(string brandName);
}
