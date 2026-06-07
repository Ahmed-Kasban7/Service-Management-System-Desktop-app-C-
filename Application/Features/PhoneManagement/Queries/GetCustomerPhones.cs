using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PhoneManagement.Queries;

public class GetCustomerPhonesHandler
{
   private readonly IPhoneRepository _phoneRepository;

    public GetCustomerPhonesHandler(IPhoneRepository phoneRepository)
    {
        _phoneRepository = phoneRepository;
    }

    public IEnumerable<string> Handle(int customerId)
    {
        if (customerId <= 0)
            throw new ArgumentOutOfRangeException("رقم العميل غير صالح");

        return _phoneRepository.GetCustomerPhones(customerId);
    }

}
