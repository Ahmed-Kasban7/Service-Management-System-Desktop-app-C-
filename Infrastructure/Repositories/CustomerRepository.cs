using Application.Common;
using Application.DTOs;
using Application.DTOs.CustomerDTOs;
using Application.DTOs.OrderDTOs;
using Application.Repositories;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data;

public class CustomerRepository : ICustomerRepository
{
    private DeviceRepository _deviceRepository;
    private PhoneRepository _phoneRepository;
    
    public CustomerRepository(DeviceRepository deviceRepository , PhoneRepository phoneRepository)
    {
        _deviceRepository = deviceRepository;
        _phoneRepository = phoneRepository;
    }
    public int Create(Customer customer)
    {
        using var conn = DatabaseInitializer.GetConnection();
        conn.Open();

        using var cmd = new SqlCommand("SP_CreateCustomer", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Name", customer.Name);
        cmd.Parameters.AddWithValue("@Age", customer.Age);
        cmd.Parameters.AddWithValue("@Sex", (int)customer.Sex);
        cmd.Parameters.AddWithValue("@Discount", customer.Discount);
        cmd.Parameters.AddWithValue("@Address", customer.Address);
        var outputParam = new SqlParameter("@CustomerId", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };

        cmd.Parameters.Add(outputParam);

        var phoneParam = cmd.Parameters.AddWithValue("@Phones", BuildPhoneTable(customer.Phones));
        phoneParam.SqlDbType = SqlDbType.Structured;
        phoneParam.TypeName = "PhoneList";

        var deviceParam = cmd.Parameters.AddWithValue("@Devices", BuildDeviceTable(customer.Devices));
        deviceParam.SqlDbType = SqlDbType.Structured;
        deviceParam.TypeName = "DeviceList";

        cmd.ExecuteNonQuery();

        return (int)outputParam.Value;
    }
    public Customer Get(int id)
    {
        using var conn = DatabaseInitializer.GetConnection();
        using var cmd = new SqlCommand("SP_GetCustomerByID", conn);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add("@customerId", SqlDbType.Int).Value = id;

        conn.Open();

        using var reader = cmd.ExecuteReader();

        if (!reader.Read())
            return null;

        return new Customer(id,reader["Name"].ToString() ,
            reader["Age"] != DBNull.Value ? Convert.ToInt32(reader["Age"]) : null, 
            (ESex)Convert.ToInt32(reader["Sex"]), reader["Address"].ToString()  
            , reader["Discount"] != DBNull.Value ? Convert.ToInt32(reader["Discount"]) : 0) ;
           
    }
    private DataTable BuildPhoneTable(IReadOnlySet<Phone> phones)
    {
        var table = new DataTable();
        table.Columns.Add("Phone", typeof(string));

        foreach (var phone in phones)
            table.Rows.Add(phone.PhoneNumber);

        return table;
    }

    private DataTable BuildDeviceTable(IReadOnlySet<Device> devices)
    {
        var table = new DataTable();
        table.Columns.Add("BrandId", typeof(int));
        table.Columns.Add("TypeId", typeof(int));
        table.Columns.Add("SpecId", typeof(int));
        table.Columns.Add("SerialNumber", typeof(string));
        table.Columns.Add("Model", typeof(string));

        foreach (var d in devices)
            table.Rows.Add(d.BrandID, d.TypeID, d.SpecID, d.SerialNumber, d.ModelName);

        return table;
    }

    public  PagedResult<CustomerSummaryDto> GetPagedCustomerSummaries( int pageNumber , int pageSize)
    {
        using var conn = DatabaseInitializer.GetConnection();

        using SqlCommand command = new SqlCommand("SP_GetPagedCustomerSummaries", conn);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@PageNumber", pageNumber);
        command.Parameters.AddWithValue("@RowsPerPage", pageSize);

        var totalParam = new SqlParameter("@TotalOrderCount", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(totalParam);

        conn.Open();

        var customers = new List<CustomerSummaryDto>();

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {

                 customers.Add(new CustomerSummaryDto(Convert.ToInt32( reader["CustomerID"]),reader["CustomerNumber"].ToString()
                    , reader["Name"].ToString(), reader["Address"].ToString(), reader["PhoneNumber"].ToString()));
            }

        }

        int totalCount = totalParam.Value != DBNull.Value ? (int)totalParam.Value : 0;

        return new PagedResult<CustomerSummaryDto>(customers, totalCount, pageNumber, pageSize);

    }

    public int GetCustomerCount()
    {
        using var conn = DatabaseInitializer.GetConnection();
        using SqlCommand command = new SqlCommand("SP_GetCustomerCount", conn);
        command.CommandType = CommandType.StoredProcedure;
        conn.Open();
        int custoemrCount = (int) command.ExecuteScalar();
        return custoemrCount;
    }
    public Result Delete(int customerId)
    {
        using var conn = DatabaseInitializer.GetConnection();
        using var cmd = new SqlCommand("SP_DeleteCustomer", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@CustomerId", customerId);

        try
        {
            conn.Open();
            cmd.ExecuteNonQuery();
            return Result.Success();
        }
        catch (SqlException ex)
        {
            return Result.Failure(ex.Message);
        }
    }
    public PagedResult<CustomerSummaryDto> SearchCustomerPaged(string word, int pageNumber, int rowPerPage)
    {
        var customers = new List<CustomerSummaryDto>();
        int totalCount = 0;

        using var conn = DatabaseInitializer.GetConnection();
        using var command = new SqlCommand("SP_SearchCustomerPaged", conn);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add("@SearchWord", SqlDbType.NVarChar, 100).Value = word ?? "";
        command.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pageNumber;
        command.Parameters.Add("@RowPerPage", SqlDbType.Int).Value = rowPerPage;

        conn.Open();

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            if (totalCount == 0 && reader["TotalCount"] != DBNull.Value)
                totalCount = Convert.ToInt32(reader["TotalCount"]);

            customers.Add(new CustomerSummaryDto(Convert.ToInt32(reader["CustomerID"]), reader["CustomerNumber"].ToString()
                    , reader["Name"].ToString(), reader["Address"].ToString(), reader["PhoneNumber"].ToString()));
        }

        return new PagedResult<CustomerSummaryDto>(
            customers,
            totalCount,
            pageNumber,
            rowPerPage
        );
    }
    public bool UpdateCustomerInfo(Customer customerInfo)
    {
        using var conn = DatabaseInitializer.GetConnection();
        using var command = new SqlCommand("SP_UpdateCustomerInfo", conn);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Name", customerInfo.Name);
        command.Parameters.AddWithValue("@Sex", (int)customerInfo.Sex);
        command.Parameters.AddWithValue("@Age", customerInfo.Age is null ? DBNull.Value : customerInfo.Age);
        command.Parameters.AddWithValue("@customerId", customerInfo.CustomerId);
        command.Parameters.AddWithValue("@Address", customerInfo.Address);
        command.Parameters.AddWithValue("@Discount", customerInfo.Discount);

        conn.Open();
        int rowsAffected = command.ExecuteNonQuery();

        return rowsAffected > 0;
    }


    public CustomerBasicInfoDto GetCustomerBasicInfo(int id)
    {
        using var conn = DatabaseInitializer.GetConnection();
        using var command = new SqlCommand("SP_GetCustomerProfile", conn);

        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@id", id);

        conn.Open();

        using var reader = command.ExecuteReader();

        if (!reader.Read())
            return null;

        var CustomerBasicInfo = new CustomerBasicInfoDto(
            Convert.ToInt32(reader["CustomerID"]),
            reader["CustomerNumber"].ToString(),
            reader["Name"].ToString(),
            reader["Address"].ToString(),
            reader["Discount"] != DBNull.Value ? Convert.ToInt32(reader["Discount"]) : 0,
            reader["Age"] != DBNull.Value ? Convert.ToInt32(reader["Age"]) : null,
            (ESex)Convert.ToInt32(reader["Sex"]) ,
            Convert.ToDateTime(reader["DateCreated"])
        );

        return CustomerBasicInfo;
    }
    public IEnumerable<CustomerLookupDto> GetCustomersLookup()
    {
        using var conn = DatabaseInitializer.GetConnection();
        using var command = new SqlCommand("SP_GetCustomersLookup", conn);

        var customerLookup = new List<CustomerLookupDto>();
        command.CommandType = CommandType.StoredProcedure;
        conn.Open();

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            customerLookup.Add(new CustomerLookupDto(Convert.ToInt32(reader["Id"]), reader["Name"].ToString()));
           
        }
        return customerLookup;
    }

    public bool IsCustomerExist(int id)
    {
        using var conn = DatabaseInitializer.GetConnection();
        using var cmd = new SqlCommand("SP_IsCustomerExist", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@customerId", id);
        conn.Open();
        var res = cmd.ExecuteScalar();
        return res != null;
    }

}
