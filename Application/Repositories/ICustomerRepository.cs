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

public interface ICustomerRepository  : IRepository<Customer>
{
    void Create(Customer customer);
    IEnumerable<CustomerSummaryDto> GetPagedCustomerSummaries(int pageNumber , int rowPerPage);

    int GetCustomerCount();
    CustomerProfileDto GetCustomerFullProfile(int id );
    List<CustomerSummaryDto> SearchCustomerPagedBy(string word , int pageNumber, int rowPerPage);
    public int GetSearchCustomerCount(string word);

    Customer GetCustomerById(int id);

    public bool UpdateCustomerInfo(Customer customerInfo);
}
