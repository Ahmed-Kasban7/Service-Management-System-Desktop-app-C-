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

public interface ICustomerRepository  : IQueryRepository<Customer>
{
    IEnumerable<CustomerSummaryDto> GetPagedCustomerSummaries(int pageNumber , int rowPerPage);

    int GetCustomerCount();
    CustomerProfileDto GetCustomerFullProfile(int id );
    List<CustomerSummaryDto> SearchCustomerPagedBy(string word , int pageNumber, int rowPerPage);
    public int GetSearchCustomerCount(string word);

    public bool UpdateCustomerInfo(Customer customerInfo);

    public bool IsCustomerExist(int id);
    public bool Delete(int id);
    public void Create(Customer customer);

    public IEnumerable<CustomerLookupDto>  GetCustomersLookup();
}
