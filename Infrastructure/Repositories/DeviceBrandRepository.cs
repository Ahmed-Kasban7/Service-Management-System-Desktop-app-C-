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

public class DeviceBrandRepository:IDeviceBrandRepository
{
    public List<BrandDto> GetAllBrands()
    {
        List<BrandDto> brands = new List<BrandDto>();
        using var conn = DatabaseInitializer.GetConnection();

        using var cmd = new SqlCommand("SP_GetAllBrands", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        conn.Open();
        using var reader=cmd.ExecuteReader();

        while (reader.Read())
        {
            brands.Add(new BrandDto(Convert.ToInt32(reader["BrandID"]), reader["BrandName"].ToString()));
        }

        return brands;
    }
    public BrandDto GetBrandBy(int id)
    {
        BrandDto brand = null;
        using var conn = DatabaseInitializer.GetConnection();

        string script = @"select * from Brands where BrandID = @id";
        using var cmd = new SqlCommand(script, conn);
        cmd.Parameters.AddWithValue("id", id);
        conn.Open();
        using var reader=   cmd.ExecuteReader();

        if(reader.Read())
        {
            brand = new BrandDto(Convert.ToInt32(reader["BrandID"]), reader["BrandName"].ToString());
        }

        return brand;
    }
    public bool AddBrand(string brandName)
    {
       
            using var conn = DatabaseInitializer.GetConnection();

            using var cmd = new SqlCommand("SP_AddBrand", conn);
             cmd.CommandType  = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@brandName", brandName);

            conn.Open();
            int rowsAffected = cmd.ExecuteNonQuery();

            return rowsAffected > 0;
       
    }
}
