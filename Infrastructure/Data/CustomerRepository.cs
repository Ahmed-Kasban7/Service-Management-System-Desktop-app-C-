using Application.Common.Interfaces;
using Application.DTOs;
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
    public List<CustomerSummaryDTO> SearchCustomerBy(string s)
    {
        List<CustomerSummaryDTO> customers = new List<CustomerSummaryDTO>();
        var conn = DatabaseInitializer.GetConnection();

        string script = @"
    SELECT DISTINCT 
        p.PersonID,
        p.Name,
        (SELECT TOP 1 PhoneNumber 
         FROM Phones 
         WHERE Phones.PersonID = p.PersonID) AS PhoneNumber,
        c.Address
    FROM Persons p
    JOIN Customers c ON p.PersonID = c.PersonID
    LEFT JOIN Phones ph ON p.PersonID = ph.PersonID
    WHERE p.Name LIKE @search 
       OR ph.PhoneNumber LIKE @search";

        conn.Open();
        using SqlCommand command = new SqlCommand(script, conn);

        command.Parameters.AddWithValue("@search", "%" +s + "%");

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            customers.Add(new CustomerSummaryDTO
            {
                ID = "C-" + reader["PersonID"].ToString(),
                Name = reader["Name"].ToString(),
                Address = reader["Address"].ToString(),
                Phone = reader["PhoneNumber"]?.ToString()
            });
        }

        return customers;
    }

    public List<CustomerSummaryDTO> GetAllCustomers()
    {
        List<CustomerSummaryDTO> customers = new List<CustomerSummaryDTO>();
        using var conn = DatabaseInitializer.GetConnection();

        string script = @"
        SELECT 
            p.PersonID, 
            p.Name, 
            c.Address, 
            (SELECT TOP 1 PhoneNumber FROM Phones WHERE PersonID = p.PersonID) AS PhoneNumber 
        FROM Customers AS c  
        JOIN Persons AS p ON c.PersonID = p.PersonID  
        WHERE p.IsDeleted = 0";

        conn.Open();
        using SqlCommand command = new SqlCommand(script, conn);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            CustomerSummaryDTO customer = new CustomerSummaryDTO
            {
                ID = "C-"+reader["PersonID"].ToString() ,
                Name = reader["Name"].ToString(),
                Address = reader["Address"].ToString(),
                Phone = reader["PhoneNumber"].ToString()

            }; // complexity ->  O(n) , Space -> O(n)

            customers.Add(customer);

        }
        return customers;
    }

    public CustomerProfileDTO GetCustomerFullProfile(int id)
    {
        CustomerProfileDTO customerProfileDTO = new CustomerProfileDTO();

        var script = @"select p.PersonID, 
                      p.Name, 
                      Sex = case
                                when p.Sex = 1 then 'ذكر'
                                when p.Sex = 2 then 'انثى'
                                else 'غير معروف'
                            end,
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
            customerProfileDTO.Sex = reader["Sex"].ToString();
        }
       customerProfileDTO.Devices=  _deviceRepository.GetCustomerDevicesBy(id);
       customerProfileDTO.Phones= _phoneRepository.GetCustomerPhonesBy(id);

        return customerProfileDTO;
    }


    public bool DeleteCustomer(int id)
    {
        var script = @"Update Persons set IsDeleted = 1 where PersonID = @id";

        using var conn = DatabaseInitializer.GetConnection();
        conn.Open();
        using var command = new SqlCommand(script, conn);
        command.Parameters.AddWithValue("id", id);
        int row = command.ExecuteNonQuery();

        if (row > 0) 
            return true;
        
        return false;
    }


}
