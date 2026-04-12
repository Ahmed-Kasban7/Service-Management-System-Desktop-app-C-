using Application.DTOs.DeviceDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories;

public interface IDeviceSpecRepository
{
    public List<SpecDto> GetSpecsByTypeId(int typeId);
    public List<SpecDto> GetAllSpecs();
    bool AddSpec(string spec, int TypeId);

}
