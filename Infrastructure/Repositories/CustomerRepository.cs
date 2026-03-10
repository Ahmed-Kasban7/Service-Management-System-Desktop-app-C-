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
    
    public CustomerRepository()
    {
        _deviceRepository = new DeviceRepository();
        _phoneRepository = new PhoneRepository();
    }
    public List<CustomerSummaryDto> GetPagedCustomerSummaries( int pageNumber , int rowsPerPage)
    {
        List<CustomerSummaryDto> customers = new List<CustomerSummaryDto>();
        using var conn = DatabaseInitializer.GetConnection();

        using SqlCommand command = new SqlCommand("SP_GetPagedCustomerSummaries", conn);

        command.Parameters.AddWithValue("@PageNumber", pageNumber);
        command.Parameters.AddWithValue("@RowsPerPage", rowsPerPage);

        command.CommandType = CommandType.StoredProcedure;
        conn.Open();
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {

            customers.Add(new CustomerSummaryDto("C-" + reader["PersonID"].ToString()
                , reader["Name"].ToString(), reader["Address"].ToString(), reader["PhoneNumber"].ToString()));
        }
        return customers;
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
    public Customer GetCustomerById(int customerId)
    {
        using var conn = DatabaseInitializer.GetConnection();
       using var command = new SqlCommand("SP_GetCustomerByID", conn);

       command.Parameters.AddWithValue("@customerId", customerId);
        command.CommandType= CommandType.StoredProcedure;
        conn.Open();
        using var reader = command.ExecuteReader();
        Customer customer = null;

        if (reader.Read())
        {
             customer = new Customer(customerId, reader["Name"].ToString(),
                reader["Age"] != DBNull.Value ? Convert.ToInt32(reader["Age"]) : null,
                (ESex)Convert.ToInt32(reader["Sex"])
                , reader["Address"].ToString(), Convert.ToInt32(reader["Discount"]) );
        }

        return customer;
    }
    public CustomerProfileDTO GetCustomerFullProfile(int id)
    {
        CustomerProfileDTO customerProfileDTO = new CustomerProfileDTO();


        using var conn = DatabaseInitializer.GetConnection();
       using var command = new SqlCommand("SP_GetCustomerProfile", conn);
        command.CommandType = CommandType.StoredProcedure;
       command.Parameters.AddWithValue("@id", id);
        conn.Open();
        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            customerProfileDTO.ID = "C-" + reader["PersonID"].ToString();
            customerProfileDTO.Name = reader["Name"].ToString();
            customerProfileDTO.Address = reader["Address"].ToString();
            customerProfileDTO.Discount = reader["Discount"] != DBNull.Value ? Convert.ToInt32(reader["Discount"]) : 0;
            customerProfileDTO.Age = reader["Age"] != DBNull.Value ? Convert.ToInt32(reader["Age"]) : null;
            customerProfileDTO.Sex = (ESex)Convert.ToInt32(reader["Sex"]);
        }

       customerProfileDTO.Devices=  _deviceRepository.GetCustomerDevicesBy(id);
       customerProfileDTO.Phones= _phoneRepository.GetCustomerPhonesBy(id);

        return customerProfileDTO;
    }

    public int CreateCustomer(Customer customer)
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

        var phoneTable = new DataTable();
        phoneTable.Columns.Add("Phone", typeof(string));

        foreach (var phone in customer.Phones)
        {
            phoneTable.Rows.Add(phone.PhoneNumber);
        }

        var phoneParam = cmd.Parameters.AddWithValue("@Phones", phoneTable);
        phoneParam.SqlDbType = SqlDbType.Structured;
        phoneParam.TypeName = "PhoneList";

        var deviceTable = new DataTable();
        deviceTable.Columns.Add("BrandId", typeof(int));
        deviceTable.Columns.Add("TypeId", typeof(int));
        deviceTable.Columns.Add("SpecId", typeof(int));
        deviceTable.Columns.Add("SerialNumber", typeof(string));
        deviceTable.Columns.Add("Model", typeof(string));

        foreach (var d in customer.Devices)
        {
            deviceTable.Rows.Add(
                d.BrandID,
                d.TypeID,
                d.SpecID,
                d.SerialNumber,
                d.ModelName
            );
        }

        var deviceParam = cmd.Parameters.AddWithValue("@Devices", deviceTable);
        deviceParam.SqlDbType = SqlDbType.Structured;
        deviceParam.TypeName = "DeviceList";

        return cmd.ExecuteNonQuery();
    }

}
