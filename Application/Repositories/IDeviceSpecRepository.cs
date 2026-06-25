using Application.DTOs.DeviceDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories;

public interface IDeviceSpecRepository
{
    public IEnumerable<SpecDto> GetSpecsByTypeId(int typeId);
    public List<SpecDto> GetAllSpecs();
    bool AddSpec(string spec, int TypeId);
    bool DeleteSpec(int specId);
    bool UpdateSpec(int specId, string newSpecName);


}
