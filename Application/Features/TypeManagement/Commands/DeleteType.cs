using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.TypeManagement.Commands;

public class DeleteTypeHandler
{
    private readonly IDeviceTypeRepository _typeRepository;

    public DeleteTypeHandler(IDeviceTypeRepository typeRepository)
    {
        _typeRepository = typeRepository;
    }

    public bool Handle(int Id)
    {
        if (Id <= 0)
            throw new ArgumentException("رقم النوع غير صالح");


        return _typeRepository.DeleteType(Id);
    }
}
