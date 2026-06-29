using Application.Common;
using Application.DTOs.EmployeeDTOs;
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

    public int Create(CreateEmployeeDto dto)
    {
        using var conn = DatabaseInitializer.GetConnection();
        conn.Open();

        using var cmd = new SqlCommand("SP_CreateEmployee", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@Name", dto.Name);
        cmd.Parameters.AddWithValue("@Age", dto.Age.HasValue ? dto.Age : DBNull.Value);
        cmd.Parameters.AddWithValue("@Sex", dto.Sex);
        cmd.Parameters.Add("@Address", SqlDbType.NVarChar).Value =
            string.IsNullOrWhiteSpace(dto.Address)
                ? DBNull.Value
                : dto.Address;
        cmd.Parameters.AddWithValue("@HireDate", dto.HireDate);
        cmd.Parameters.AddWithValue("@RoleId", dto.RoleId);
        cmd.Parameters.AddWithValue("@DepartmentId", dto.DepartmentId);
        cmd.Parameters.AddWithValue("@CompensationType", dto.CompensationType);
        cmd.Parameters.AddWithValue("@CommissionType", dto.CommissionType.HasValue ? dto.CommissionType : DBNull.Value);
        cmd.Parameters.AddWithValue("@BaseSalary",
            dto.BaseSalary.HasValue ? dto.BaseSalary : DBNull.Value);
        cmd.Parameters.AddWithValue("@Commission",
            dto.Commission.HasValue ? dto.Commission : DBNull.Value);

        // Phones
        var phonesTable = new DataTable();
        phonesTable.Columns.Add("Phone", typeof(string));
        foreach (var phone in dto.Phones)
            phonesTable.Rows.Add(phone);

        var phonesParam = cmd.Parameters.AddWithValue("@Phones", phonesTable);
        phonesParam.SqlDbType = SqlDbType.Structured;
        phonesParam.TypeName = "PhoneList";

        // Attachments
        var attachmentsTable = new DataTable();
        attachmentsTable.Columns.Add("AttachmentData", typeof(byte[])); 

        foreach (var imageBytes in dto.Attachments)
        {
            if (imageBytes != null && imageBytes.Length > 0)
            {
                attachmentsTable.Rows.Add(imageBytes); 
            }
        }

        var attachmentsParam = cmd.Parameters.AddWithValue("@Attachments", attachmentsTable);
        attachmentsParam.SqlDbType = SqlDbType.Structured;
        attachmentsParam.TypeName = "AttachmentList";

        var employeeIdParam = new SqlParameter("@EmployeeId", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        cmd.Parameters.Add(employeeIdParam);

        cmd.ExecuteNonQuery();

        return (int)employeeIdParam.Value;
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

    public PagedResult<EmployeeSummaryDto> GetPagedEmployeeSummaries(int pageNumber, int pageSize)
    {
        using var conn = DatabaseInitializer.GetConnection();

        using SqlCommand command = new SqlCommand("GetPagedEmployeeSummaries", conn);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@PageNumber", pageNumber);
        command.Parameters.AddWithValue("@RowsPerPage", pageSize);

        var totalParam = new SqlParameter("@TotalEmployeeCount", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(totalParam);

        conn.Open();

        var employees = new List<EmployeeSummaryDto>();

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                employees.Add(new EmployeeSummaryDto
                {
                    EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                    EmployeeNumber = reader["EmployeeNumber"].ToString(),
                    Name = reader["Name"].ToString(),
                    DepartmentName = reader["DepartmentName"].ToString(),
                    RoleName = reader["RoleName"].ToString(),
                    Phone = reader["Phone"].ToString(),
                });
            }
        }

        int totalCount = totalParam.Value != DBNull.Value ? (int)totalParam.Value : 0;

        return new PagedResult<EmployeeSummaryDto>(employees, totalCount, pageNumber, pageSize);
    }
    public PagedResult<EmployeeSummaryDto> SearchEmployee(int pageNumber, int pageSize , string searchWord)
    {
        using var conn = DatabaseInitializer.GetConnection();

        using SqlCommand command = new SqlCommand("SP_SearchEmployee", conn);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@PageNumber", pageNumber);
        command.Parameters.AddWithValue("@RowsPerPage", pageSize);
        command.Parameters.AddWithValue("@SearchWord", searchWord);

        var totalParam = new SqlParameter("@TotalEmployeeCount", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(totalParam);

        conn.Open();

        var employees = new List<EmployeeSummaryDto>();

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                employees.Add(new EmployeeSummaryDto
                {
                    EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                    EmployeeNumber = reader["EmployeeNumber"].ToString(),
                    Name = reader["Name"].ToString(),
                    DepartmentName = reader["DepartmentName"].ToString(),
                    RoleName = reader["RoleName"].ToString(),
                    Phone = reader["Phone"].ToString(),
                });
            }
        }

        int totalCount = totalParam.Value != DBNull.Value ? (int)totalParam.Value : 0;

        return new PagedResult<EmployeeSummaryDto>(employees, totalCount, pageNumber, pageSize);
    }

    public EmployeeProfileDto GetEmployeeProfileById(int employeeId)
    {
        using var conn = DatabaseInitializer.GetConnection();
        using SqlCommand command = new SqlCommand("SP_GetEmployeeProfile", conn);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@EmployeeID", employeeId);

        conn.Open();

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new EmployeeProfileDto
            {
                Id = Convert.ToInt32(reader["Id"]),
                EmployeeNumber = reader["EmployeeNumber"].ToString(),
                Name = reader["Name"].ToString(),
                Sex = Convert.ToByte(reader["Sex"]),
                HireDate = Convert.ToDateTime(reader["HireDate"]),
                Address = reader["Address"] == DBNull.Value ? null : reader["Address"].ToString(),
                DepartmentName = reader["DepartmentName"].ToString(),
                DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                RoleName = reader["RoleName"].ToString(),
                RoleId = Convert.ToInt32(reader["RoleId"]),
                CompensationType = Convert.ToByte(reader["CompensationType"]),
                CompensationTypeText = reader["CompensationTypeText"].ToString(),
                CommissionType = reader["CommissionType"] == DBNull.Value? null : Convert.ToBoolean(reader["CommissionType"]),
                BaseSalary =
    reader["BaseSalary"] == DBNull.Value
        ? null
        : Convert.ToDecimal(reader["BaseSalary"]),

                Commission =
    reader["Commission"] == DBNull.Value
        ? null
        : Convert.ToDecimal(reader["Commission"]),

                Age = reader["Age"] == DBNull.Value ? null : Convert.ToInt32(reader["Age"])
            };
        }

        return null; 
    }
    public bool Update(UpdateEmployeeDto dto)
    {
        using var conn = DatabaseInitializer.GetConnection();
        conn.Open();

        using var cmd = new SqlCommand("SP_UpdateEmployee", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@EmployeeId", dto.EmployeeId);
        cmd.Parameters.AddWithValue("@Name", dto.Name);
        cmd.Parameters.AddWithValue("@Age", dto.Age.HasValue ? dto.Age : DBNull.Value);
        cmd.Parameters.AddWithValue("@Sex", dto.Sex);
        cmd.Parameters.AddWithValue("@Address", dto.Address ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@RoleId", dto.RoleId);
        cmd.Parameters.AddWithValue("@DepartmentId", dto.DepartmentId);
        cmd.ExecuteNonQuery();
        return  true;
    }


}
