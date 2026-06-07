using Application.Common;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CustomerManagment.Commands;

public class DeleteCustomerHandler
{
    private readonly ICustomerRepository _customerRepository;

    public DeleteCustomerHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public Result Handle(int customerId)
    {
        if (customerId <= 0)
            return Result.Failure("رقم العميل غير صحيح.");

        return _customerRepository.Delete(customerId);
    }
}
