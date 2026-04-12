using Application.DTOs.DeviceDTOs;
using Application.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data;

public class DeviceSpecRepository:IDeviceSpecRepository
{
    public List<SpecDto> GetSpecsByTypeId(int typeId)
    {
        List<SpecDto> specDTOs = new List<SpecDto>();
        using var conn = DatabaseInitializer.GetConnection();

        using var cmd = new SqlCommand("SP_GetSpecsByType", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("typeId", typeId);
        conn.Open();
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            specDTOs.Add(new SpecDto(Convert.ToInt32(reader["SpecID"]), reader["SpecName"].ToString(), Convert.ToInt32(reader["TypeID"]), reader["TypeName"].ToString()));
        }
        return specDTOs;
    }
    public List<SpecDto> GetAllSpecs()
    {
        List<SpecDto> specDTOs = new List<SpecDto>();
        using var conn = DatabaseInitializer.GetConnection();

        using var cmd = new SqlCommand("SP_GetAllSpecs", conn);
        cmd.CommandType =CommandType.StoredProcedure;
        conn.Open();
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            specDTOs.Add(new SpecDto(Convert.ToInt32(reader["SpecID"]), reader["SpecName"].ToString(), Convert.ToInt32(reader["TypeID"]), reader["TypeName"].ToString()));
        }
        return specDTOs;
    }

    public bool AddSpec(string spec, int typeId)
    {

        List<TypeDto> types = new List<TypeDto>();
        using var conn = DatabaseInitializer.GetConnection();

        using var cmd = new SqlCommand("SP_AddSpec", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Spec", spec);
        cmd.Parameters.AddWithValue("@typeId", typeId);

        conn.Open();
        var result = cmd.ExecuteNonQuery();

        return result > 0;
    }


}
