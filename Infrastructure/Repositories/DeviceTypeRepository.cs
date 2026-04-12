using Application.DTOs.DeviceDTOs;
using Application.Repositories;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data;

public class DeviceTypeRepository:IDeviceTypeRepository
{
    public List<TypeDto> GetAllTypes()
    {
        List<TypeDto> types = new List<TypeDto>();
        using var conn = DatabaseInitializer.GetConnection();

        using var cmd = new SqlCommand("SP_GetAllTypes", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        conn.Open();
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            types.Add(new TypeDto(Convert.ToInt32(reader["TypeID"]), reader["TypeName"].ToString()));
        }

        return types;
    }

  
    public bool AddType(string type )
    {
        List<TypeDto> types = new List<TypeDto>();
        using var conn = DatabaseInitializer.GetConnection();

        using var cmd = new SqlCommand("SP_AddDeviceType", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@TypeName", type );
        conn.Open();
        var result =cmd.ExecuteNonQuery();

        return result>0;
    }
}
