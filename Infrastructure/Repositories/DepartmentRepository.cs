using Application.DTOs.DepartmentDTOs;
using Application.DTOs.DepartmentRolesDTOs;
using Application.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    public IEnumerable<DepartmentLookupDto> GetDepartmentsLookup()
    {
        using var conn = DatabaseInitializer.GetConnection();
        conn.Open();
        using var cmd = new SqlCommand("SP_GetDepartmentsLookup", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        var result = new List<DepartmentLookupDto>();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new DepartmentLookupDto
            {
                DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                DepartmentName = reader.GetString(reader.GetOrdinal("DepartmentName"))
            });
        }
        return result;
    }

    public IEnumerable<RoleLookupDto> GetRolesByDepartment(int departmentId)
    {
        using var conn = DatabaseInitializer.GetConnection();
        conn.Open();
        using var cmd = new SqlCommand("SP_GetRolesByDepartment", conn)
        {
            CommandType = CommandType.StoredProcedure
        };
        cmd.Parameters.AddWithValue("@DepartmentId", departmentId);

        var result = new List<RoleLookupDto>();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new RoleLookupDto
            {
                RoleId = reader.GetInt32(reader.GetOrdinal("RoleID")),
                RoleName = reader.GetString(reader.GetOrdinal("RoleName"))
            });
        }
        return result;
    }
}
