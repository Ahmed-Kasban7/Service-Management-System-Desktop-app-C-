using Application.DTOs.DeviceDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SpecManagement.Queries;

public class GetSpecsByTypeIdHandler
{
    private readonly IDeviceSpecRepository _deviceSpecRepository;

    public GetSpecsByTypeIdHandler(IDeviceSpecRepository deviceSpecRepository)
    {
        _deviceSpecRepository = deviceSpecRepository;
    }

    public IEnumerable<SpecDto> Handle(int typeId)
    {
        if(typeId <=0) 
            return Enumerable.Empty<SpecDto>();

        return _deviceSpecRepository.GetSpecsByTypeId(typeId);
    }

}
