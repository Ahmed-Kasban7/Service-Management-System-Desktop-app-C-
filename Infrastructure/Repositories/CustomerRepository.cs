using Application.Common;
using Application.DTOs;
using Application.DTOs.CustomerDTOs;
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
        return null;
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

    public IEnumerable<CustomerSummaryDto> GetPagedCustomerSummaries( int pageNumber , int rowsPerPage)
    {
        using var conn = DatabaseInitializer.GetConnection();

        using SqlCommand command = new SqlCommand("SP_GetPagedCustomerSummaries", conn);

        command.Parameters.AddWithValue("@PageNumber", pageNumber);
        command.Parameters.AddWithValue("@RowsPerPage", rowsPerPage);

        command.CommandType = CommandType.StoredProcedure;
        conn.Open();
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {

             yield return  new CustomerSummaryDto("C-" + reader["PersonID"].ToString()
                , reader["Name"].ToString(), reader["Address"].ToString(), reader["PhoneNumber"].ToString());
        }
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
    public bool Delete(int customerId)
    {
        using var conn = DatabaseInitializer.GetConnection();

        using var command = new SqlCommand("SP_DeletePerson", conn);
        command.Parameters.AddWithValue("@personId", customerId);
        command.CommandType = CommandType.StoredProcedure;
        conn.Open();
        int row = command.ExecuteNonQuery();

        return row > 0;
    }
    public List<CustomerSummaryDto> SearchCustomerPagedBy(string word , int PageNumber , int RowPerPage)
    {
        List<CustomerSummaryDto> customers = new List<CustomerSummaryDto>();
        var conn = DatabaseInitializer.GetConnection();

        conn.Open();

        using SqlCommand command = new SqlCommand("SP_SearchCustomerPaged", conn);
        command.Parameters.AddWithValue("@word", word);
        command.Parameters.AddWithValue("@PageNumber", PageNumber);
        command.Parameters.AddWithValue("@RowPerPage", RowPerPage);
        command.CommandType = CommandType.StoredProcedure;
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            customers.Add(new CustomerSummaryDto("C-" + reader["PersonID"].ToString()
                , reader["Name"].ToString(), reader["Address"].ToString(), reader["PhoneNumber"].ToString()));
        }

        return customers;
    }
    public bool UpdateCustomerInfo(Customer customerInfo)
    {

         using var conn = DatabaseInitializer.GetConnection();
         using var command = new SqlCommand("SP_UpdateCustomerInfo", conn);
         command.Parameters.AddWithValue("@Name", customerInfo.Name);
         command.Parameters.AddWithValue("@Sex", (int)customerInfo.Sex );
         command.Parameters.AddWithValue("@Age", customerInfo.Age is null ? DBNull.Value : customerInfo.Age);
         command.Parameters.AddWithValue("@PersonId", customerInfo.Id);
         command.Parameters.AddWithValue("@Address", customerInfo.Address);
         command.Parameters.AddWithValue("@Discount", customerInfo.Discount);
         command.CommandType = CommandType.StoredProcedure;
         conn.Open();
         var rows = command.ExecuteNonQuery();
        return rows > 0;
    }

    public int GetSearchCustomerCount(string word)
    {
        var conn = DatabaseInitializer.GetConnection();

        using SqlCommand command = new SqlCommand("SP_SearchCustomerCount", conn);
        command.Parameters.AddWithValue("@word", word);
        command.CommandType = CommandType.StoredProcedure;
        conn.Open();

        return  (int) command.ExecuteScalar();
    }
    //public Customer Get(int customerId)
    //{
    //   // using var conn = DatabaseInitializer.GetConnection();
    //   //using var command = new SqlCommand("SP_GetCustomerByID", conn);

    //   //command.Parameters.AddWithValue("@customerId", customerId);
    //   // command.CommandType= CommandType.StoredProcedure;
    //   // conn.Open();
    //   // using var reader = command.ExecuteReader();
    //   // Customer customer = null;

    //   // if (reader.Read())
    //   // {
    //   //      customer = new Customer(reader["Name"].ToString(),
    //   //         reader["Age"] != DBNull.Value ? Convert.ToInt32(reader["Age"]) : null,
    //   //         (ESex)Convert.ToInt32(reader["Sex"])
    //   //         , reader["Address"].ToString(), Convert.ToInt32(reader["Discount"]) );
    //   // }

    //   // return customer;
    //}
    public CustomerProfileDto GetCustomerFullProfile(int id)
    {
        using var conn = DatabaseInitializer.GetConnection();
        using var command = new SqlCommand("SP_GetCustomerProfile", conn);

        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@id", id);

        conn.Open();

        using var reader = command.ExecuteReader();

        if (!reader.Read())
            return null;

        var customerProfileDTO = new CustomerProfileDto(
            "C-" + reader["PersonID"].ToString(),
            reader["Name"].ToString(),
            reader["Address"].ToString(),
            reader["Discount"] != DBNull.Value ? Convert.ToInt32(reader["Discount"]) : 0,
            reader["Age"] != DBNull.Value ? Convert.ToInt32(reader["Age"]) : null,
            (ESex)Convert.ToInt32(reader["Sex"]) , _deviceRepository.GetCustomerDevicesBy(id).ToList() , _phoneRepository.GetCustomerPhonesBy(id)
        );
        return customerProfileDTO;
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
