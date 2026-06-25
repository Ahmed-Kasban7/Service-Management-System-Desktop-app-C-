using Application.DTOs.PersonDTOs;
using Application.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    public bool IsEmployeeExists(int employeeId)
    {
        return false;
    }


    public IEnumerable<PersonLookupDto> GetEmployeesLookup(string roleName)
    {
        using var connection = DatabaseInitializer.GetConnection();
        connection.Open();
        using var command = new SqlCommand("SP_GetEmployeesLookup", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@RoleName", roleName);
        var result = new List<PersonLookupDto>();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new PersonLookupDto
            (
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetString(reader.GetOrdinal("Name"))
            ));
        }
        return result;
    }
    public IEnumerable<PersonLookupDto> GetAllEmployeesLookup()
    {
        using var connection = DatabaseInitializer.GetConnection();
        connection.Open();
        using var command = new SqlCommand("SP_GetAllEmployeesLookup", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        var result = new List<PersonLookupDto>();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new PersonLookupDto
            (
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetString(reader.GetOrdinal("Name"))
            ));
        }
        return result;
    }


}
