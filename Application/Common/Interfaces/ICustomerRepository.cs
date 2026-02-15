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
    //bool CreateCustomer(Customer customer);
    List<CustomerSummaryDTO> GetAllCustomers();
    CustomerProfileDTO GetCustomerFullProfile(int id );
    List<CustomerSummaryDTO> SearchCustomerBy(string s);
    //List<Customer> SearchCustomrByName(string name);

    bool DeleteCustomer(int id);

    //bool UpdateCustomer(Customer customer);


}
