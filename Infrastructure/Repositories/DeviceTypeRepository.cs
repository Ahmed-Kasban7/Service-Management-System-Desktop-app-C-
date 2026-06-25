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
    public IEnumerable<TypeDto> GetAllTypes()
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


    public bool AddType(string type)
    {
        using var conn = DatabaseInitializer.GetConnection();
        using var cmd = new SqlCommand("SP_AddDeviceType", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@TypeName", type);

        conn.Open();

        var result = cmd.ExecuteScalar();

        return result != null && Convert.ToInt32(result) == 1;
    }
    public bool DeleteType(int id)
    {
        using var conn = DatabaseInitializer.GetConnection();
        using var cmd = new SqlCommand("SP_DeleteType", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add(new SqlParameter("@typeId", SqlDbType.Int) { Value = id });

        var returnParam = new SqlParameter
        {
            Direction = ParameterDirection.ReturnValue
        };
        cmd.Parameters.Add(returnParam);

        conn.Open();
        cmd.ExecuteNonQuery();

        int result = (int)returnParam.Value;

        return result == 1;
    }

    public bool UpdateType(int id, string typeName)
    {
        using var conn = DatabaseInitializer.GetConnection();
        using var cmd = new SqlCommand("SP_UpdateType", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add(new SqlParameter("@typeId", SqlDbType.Int) { Value = id });
        cmd.Parameters.Add(new SqlParameter("@typeName", SqlDbType.NVarChar, 200) { Value = typeName });

        var returnParam = new SqlParameter
        {
            Direction = ParameterDirection.ReturnValue
        };
        cmd.Parameters.Add(returnParam);

        conn.Open();
        cmd.ExecuteNonQuery();

        int result = (int)returnParam.Value;

        if (result == -1)
        {
            throw new Exception("اسم النوع هذا موجود بالفعل في النظام، لا يمكن التكرار.");
        }

        return result == 1;
    }
}
