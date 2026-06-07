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
    PagedResult<CustomerSummaryDto> GetPagedCustomerSummaries(int pageNumber , int pageSize);

    int GetCustomerCount();
    CustomerBasicInfoDto GetCustomerBasicInfo(int id );
    PagedResult<CustomerSummaryDto> SearchCustomerPaged(string word , int pageNumber, int rowPerPage);

    public bool UpdateCustomerInfo(Customer customerInfo);

    public bool IsCustomerExist(int id);
    public Result Delete(int id);
    public int Create(Customer customer);

    public IEnumerable<CustomerLookupDto>  GetCustomersLookup();
}
