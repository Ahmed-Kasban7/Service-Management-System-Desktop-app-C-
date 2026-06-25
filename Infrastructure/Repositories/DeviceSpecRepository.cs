using Application.Common;
using Application.DTOs.DeviceDTOs;
using Application.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit;

namespace Infrastructure.Data;

public class DeviceSpecRepository:IDeviceSpecRepository
{
    public IEnumerable<SpecDto> GetSpecsByTypeId(int typeId)
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

    public bool AddSpec(string specName, int typeId)
    {
        using var conn = DatabaseInitializer.GetConnection();
        using var cmd = new SqlCommand("SP_AddSpec", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Spec", specName);
        cmd.Parameters.AddWithValue("@TypeId", typeId);

        conn.Open();

        var result = cmd.ExecuteScalar();

        return result != null && Convert.ToInt32(result) == 1;
    }

    public bool DeleteSpec(int specId)
    {
        using var conn = DatabaseInitializer.GetConnection();
        using var cmd = new SqlCommand("SP_DeleteSpec", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@SpecId", specId);

        conn.Open();

        var result = cmd.ExecuteScalar();

        return result != null && Convert.ToInt32(result) == 1;
    }

    public bool UpdateSpec(int specId, string newSpecName)
    {
        using var conn = DatabaseInitializer.GetConnection();
        using var cmd = new SqlCommand("SP_UpdateSpec", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@SpecId", specId);
        cmd.Parameters.AddWithValue("@SpecName", newSpecName);

        var returnParameter = cmd.Parameters.Add("@ReturnVal", System.Data.SqlDbType.Int);
        returnParameter.Direction = System.Data.ParameterDirection.ReturnValue;

        conn.Open();

        cmd.ExecuteNonQuery();

        int result = Convert.ToInt32(returnParameter.Value);

        if (result == 1)
        {
            return true; 
        }
        else if (result == -1)
        {
            throw new Exception("عفواً، هذا الاسم مستخدم بالفعل لمواصفة أخرى في نفس هذا النوع.");
        }

        return false; 
    }
}
