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
    IEnumerable<TypeDto> GetAllTypes();

    bool AddType(string type);

    bool DeleteType(int id);

    bool UpdateType(int id, string type);


}
