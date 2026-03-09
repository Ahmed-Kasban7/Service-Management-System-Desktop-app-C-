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
    public List<CustomerSummary> GetPagedCustomerSummaries( int pageNumber , int rowsPerPage)
    {
        List<CustomerSummary> customers = new List<CustomerSummary>();
        using var conn = DatabaseInitializer.GetConnection();

        using SqlCommand command = new SqlCommand("SP_GetPagedCustomerSummaries", conn);

        command.Parameters.AddWithValue("@PageNumber", pageNumber);
        command.Parameters.AddWithValue("@RowsPerPage", rowsPerPage);

        command.CommandType = CommandType.StoredProcedure;
        conn.Open();
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {

            customers.Add(new CustomerSummary("C-" + reader["PersonID"].ToString()
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
    public List<CustomerSummary> SearchCustomerPagedBy(string word , int PageNumber , int RowPerPage)
    {
        List<CustomerSummary> customers = new List<CustomerSummary>();
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
            customers.Add(new CustomerSummary("C-" + reader["PersonID"].ToString()
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

        var script = @"select p.PersonID, 
                      p.Name, 
                      p.Sex ,
                      p.Age, 
                      c.Address,
                      c.Discount
               from Persons p
               join Customers c on p.PersonID = c.PersonID
               where p.PersonID = @id";

        using var conn = DatabaseInitializer.GetConnection();
       using var command = new SqlCommand(script, conn);
       command.Parameters.AddWithValue("id", id);
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

       using  SqlTransaction transaction = conn.BeginTransaction();
        try
        {
            var scriptperson = "insert into Persons(Name , Age , Sex)  values(@Name , @Age , @Sex)  SELECT SCOPE_IDENTITY();";
            using var cmdPerson = new SqlCommand(scriptperson, conn, transaction);
            cmdPerson.Parameters.AddWithValue("Name", customer.Name);
            cmdPerson.Parameters.AddWithValue("Age", (object)customer.Age ?? DBNull.Value);
            cmdPerson.Parameters.AddWithValue("Sex", (int)customer.Sex);

            int personId=  Convert.ToInt32(cmdPerson.ExecuteScalar());

            var scriptCustomer = "insert into Customers(PersonID,Address , Discount) values (@PersonId, @Address , @Discount)";
            using  var cmdCustomer = new SqlCommand(scriptCustomer, conn, transaction);
            cmdCustomer.Parameters.AddWithValue("PersonId", personId);
            cmdCustomer.Parameters.AddWithValue("Address", customer.Address);
            cmdCustomer.Parameters.AddWithValue("Discount", customer.Discount);
            cmdCustomer.ExecuteNonQuery();

            var scriptphone = "insert into phones(personid , phonenumber) values (@personid , @phone)";
            using var cmdphone = new SqlCommand(scriptphone, conn, transaction);
            foreach (var phone in customer.Phones)
            {
                cmdphone.Parameters.Clear();

                cmdphone.Parameters.AddWithValue("personid", personId);
                cmdphone.Parameters.AddWithValue("phone", phone.PhoneNumber);
                cmdphone.ExecuteNonQuery();
            }

            var scriptdevice = "insert into devices (customerid ,brandid , typeid , specid ,serialnumber , modelname) values (@customerid , @brandid , @typeid , @specid , @serialnumber , @modelname)";
            using var cmddevice = new SqlCommand(scriptdevice, conn, transaction);
            foreach (var device in customer.Devices)
            {
                cmddevice.Parameters.Clear();

                cmddevice.Parameters.AddWithValue("customerid", personId);
                cmddevice.Parameters.AddWithValue("brandid", device.BrandID);
                cmddevice.Parameters.AddWithValue("typeid", device.TypeID);
                cmddevice.Parameters.AddWithValue("specid", device.SpecID);
                cmddevice.Parameters.AddWithValue("serialnumber", device.SerialNumber ?? (object)DBNull.Value);
                cmddevice.Parameters.AddWithValue("modelname", device.SerialNumber ?? (object)DBNull.Value);
                cmddevice.ExecuteNonQuery();
            }

            transaction.Commit();

            return personId;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }

    }

}
