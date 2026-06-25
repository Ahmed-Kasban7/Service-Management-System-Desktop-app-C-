using Application.DTOs.DeviceDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SpecManagement.Queries;

public class GetAllSpecsHandler
{
    private readonly IDeviceSpecRepository _deviceSpecRepository;

    public GetAllSpecsHandler(IDeviceSpecRepository deviceSpecRepository)
    {
        _deviceSpecRepository = deviceSpecRepository;
    }

    public List<SpecDto> Handle()
    {
        return _deviceSpecRepository.GetAllSpecs();
    }
}
