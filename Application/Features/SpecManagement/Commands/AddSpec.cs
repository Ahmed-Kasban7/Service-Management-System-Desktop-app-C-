using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SpecManagement.Commands;

public class AddSpecHandler
{
    private readonly IDeviceSpecRepository _deviceSpecRepository;

    public AddSpecHandler(IDeviceSpecRepository deviceSpecRepository)
    {
        _deviceSpecRepository = deviceSpecRepository;
    }

    public bool Handle(string spec, int TypeId)
    {
        if (string.IsNullOrWhiteSpace(spec))
            throw new ArgumentNullException("لا يمكن أن يكون النوع فارغًا.");

        if (spec.Length > 200)
            throw new ArgumentException("لا يمكن أن يتجاوز طول النوع 200 حرف.");

        if (TypeId <= 0)
            throw new ArgumentException("ادخل نوع صحيح");

        return _deviceSpecRepository.AddSpec(spec, TypeId);
    }
}
