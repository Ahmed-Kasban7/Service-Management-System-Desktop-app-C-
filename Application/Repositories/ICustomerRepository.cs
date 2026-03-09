using Application.Common;
using Application.DTOs;
using Application.DTOs.CustomerDTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories;

public interface ICustomerRepository  : IRepository
{
    int CreateCustomer(Customer customer);
    List<CustomerSummary> GetPagedCustomerSummaries(int pageNumber , int rowPerPage);

    int GetCustomerCount();
    CustomerProfileDTO GetCustomerFullProfile(int id );
    List<CustomerSummary> SearchCustomerPagedBy(string word , int pageNumber, int rowPerPage);
    public int GetSearchCustomerCount(string word);

    Customer GetCustomerById(int id);

    public bool UpdateCustomerInfo(Customer customerInfo);


}
