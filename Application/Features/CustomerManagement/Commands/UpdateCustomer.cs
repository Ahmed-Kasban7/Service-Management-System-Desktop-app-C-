using Application.Common;
using Application.DTOs.CustomerDTOs;
using Application.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CustomerManagment.Commands;

public class UpdateCustomerHandler
{

    private readonly ICustomerRepository _customerRepository;

    public UpdateCustomerHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public Result<bool> Handle(CustomerUpdateDto customerInfo)
    {

        if (customerInfo == null)
            return Result<bool>.Failure("بيانات العميل غير صالحة");

        var customer = new Customer(
               customerInfo.Id,
               customerInfo.Name,
               customerInfo.Age,
               customerInfo.Sex,
               customerInfo.Address,
               customerInfo.Discount
        );

       var result = _customerRepository.UpdateCustomerInfo(customer);

        if(!result)
            return Result<bool>.Failure("فشل في تحديث بيانات العميل");

        return Result<bool>.Success(true);
    }

}
