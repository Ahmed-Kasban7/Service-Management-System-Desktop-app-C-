using Application.Common.Interfaces;
using Application.DTOs;
using Domain.Entities;
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
    public  List<CustomerSummaryDTO> GetAllCustomers()
    {
        List<CustomerSummaryDTO> customers = new List<CustomerSummaryDTO>();
        var conn = DatabaseInitializer.GetConnection();

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

            };

            customers.Add(customer);

        }
        return customers;
    }

    public CustomerProfileDTO GetCustomerByID(int id)
    {
        CustomerProfileDTO customerProfileDTO = new CustomerProfileDTO();

        return customerProfileDTO;
    }


}
