using Application.DTOs;
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
    List<CustomerSummaryDTO> GetAllCustomers();
    CustomerProfileDTO GetCustomerFullProfile(int id );
    List<CustomerSummaryDTO> SearchCustomerBy(string s);

    bool DeleteCustomer(int id);
     Customer GetCustomerById(int id);

    public bool UpdateCustomerInfo(CustomerUpdateDTO customerInfo);


}
