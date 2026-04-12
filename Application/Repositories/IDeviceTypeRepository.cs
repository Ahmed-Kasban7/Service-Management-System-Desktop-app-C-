using Application.DTOs.DeviceDTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories;

public interface IDeviceTypeRepository
{
    List<TypeDto> GetAllTypes();

    bool AddType(string type);

}
