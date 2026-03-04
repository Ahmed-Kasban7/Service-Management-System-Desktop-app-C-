using Application.DTOs;
using Application.DTOs.CustomerDTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces;

public interface ICustomerRepository
{
    int CreateCustomer(Customer customer);
    List<CustomerSummary> GetPagedCustomerSummaries(int pageNumber , int rowPerPage);

    int GetCustomerCount();
    CustomerProfileDTO GetCustomerFullProfile(int id );
    List<CustomerSummary> SearchCustomerBy(string s);

    bool DeleteCustomer(int id);
     Customer GetCustomerById(int id);

    public bool UpdateCustomerInfo(CustomerUpdateDTO customerInfo);


}
