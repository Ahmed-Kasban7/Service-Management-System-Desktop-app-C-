using Application.DTOs.CustomerDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers;

public static class CustomerHelper
{
    public static IEnumerable<CustomerSummaryDto> GetPagedCustomers(IEnumerable<CustomerSummaryDto> customers 
        , int pageNumber , int pageSize)
    {

    }
}
