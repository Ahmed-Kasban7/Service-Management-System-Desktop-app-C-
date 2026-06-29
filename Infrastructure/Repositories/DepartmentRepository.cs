using Application.Common;
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
    public bool AddDepartment(string departmentName)
    {
      
        using var conn = DatabaseInitializer.GetConnection();
        conn.Open();

        using var cmd = new SqlCommand("SP_AddDepartment", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@DepName", string.IsNullOrWhiteSpace(departmentName) ? DBNull.Value : departmentName.Trim());
        var result = cmd.ExecuteScalar();

        int rowsAffected = result != null ? Convert.ToInt32(result) : 0;

        return rowsAffected > 0;
    }

    public bool UpdateDepartment(int departmentId, string departmentName)
    {
        
        using var conn = DatabaseInitializer.GetConnection();
        conn.Open();

        using var cmd = new SqlCommand("SP_UpdateDepartment", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@DepartmentId", departmentId);
        cmd.Parameters.AddWithValue("@DepartmentName", departmentName.Trim());

        var result = cmd.ExecuteScalar();
        int rowsAffected = result != null ? Convert.ToInt32(result) : 0;

        return rowsAffected > 0;
    }
    public bool DeleteDepartment(int departmentId)
    {
        
        using var conn = DatabaseInitializer.GetConnection();
        conn.Open();

        using var cmd = new SqlCommand("SP_DeleteDepartment", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@DepartmentId", departmentId);

        var result = cmd.ExecuteScalar();
        int rowsAffected = result != null ? Convert.ToInt32(result) : 0;

        return rowsAffected > 0;

    }

    public bool AddRole(string roleName, int departmentId)
    {
       
        using var conn = DatabaseInitializer.GetConnection();
        conn.Open();

        using var cmd = new SqlCommand("SP_AddDepartmentRole", conn)
        {
        CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@RoleName", roleName.Trim());
        cmd.Parameters.AddWithValue("@DepartmentId", departmentId);

        var result = cmd.ExecuteScalar();
        int rowsAffected = result != null ? Convert.ToInt32(result) : 0;

        return rowsAffected > 0;
    }

    public bool UpdateRole(int roleId, string roleName)
    {

        using var conn = DatabaseInitializer.GetConnection();
        conn.Open();

        using var cmd = new SqlCommand("SP_UpdateRole", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@RoleId", roleId);
        cmd.Parameters.AddWithValue("@RoleName", roleName.Trim());

        var result = cmd.ExecuteScalar();
        int rowsAffected = result != null ? Convert.ToInt32(result) : 0;

        return rowsAffected > 0;

    }

    public bool DeleteRole(int roleId)
    {
       
       
         using var conn = DatabaseInitializer.GetConnection();
         conn.Open();

         using var cmd = new SqlCommand("SP_DeleteRole", conn)
         {
             CommandType = CommandType.StoredProcedure
         };

         cmd.Parameters.AddWithValue("@RoleId", roleId);

        var result = cmd.ExecuteScalar();
        int rowsAffected = result != null ? Convert.ToInt32(result) : 0;

        return rowsAffected > 0;

    }
    public IEnumerable<DepartmentWithRolesDto> GetAllDepartmentRoles()
    {
        var list = new List<DepartmentWithRolesDto>();

        using var conn = DatabaseInitializer.GetConnection();
        conn.Open();

        using var cmd = new SqlCommand("SP_GetAllDepartmentRoles", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new DepartmentWithRolesDto
            {
                DepartmentId = Convert.ToInt32(reader["DepartmentID"]),
                DepartmentName = reader["DepartmentName"].ToString(),
                RoleId = Convert.ToInt32(reader["RoleID"]),
                RoleName = reader["RoleName"].ToString()
            });
        }

        return list;
    }
}
