using Application.DTOs;
using Application.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data;

public class DeviceBrandRepository:IDeviceBrandRepository
{
    public List<BrandDTO> GetAllBrands()
    {
        List<BrandDTO> brands = new List<BrandDTO>();
        using var conn = DatabaseInitializer.GetConnection();

        string script = @"select * from Brands ";
        using var cmd = new SqlCommand(script, conn);
        conn.Open();
        using var reader=   cmd.ExecuteReader();

        while (reader.Read())
        {
            brands.Add(new BrandDTO(Convert.ToInt32(reader["BrandID"]), reader["BrandName"].ToString()));
        }

        return brands;
    }
    public BrandDTO GetBrandBy(int id)
    {
        BrandDTO brand = null;
        using var conn = DatabaseInitializer.GetConnection();

        string script = @"select * from Brands where BrandID = @id";
        using var cmd = new SqlCommand(script, conn);
        cmd.Parameters.AddWithValue("id", id);
        conn.Open();
        using var reader=   cmd.ExecuteReader();

        if(reader.Read())
        {
            brand = new BrandDTO(Convert.ToInt32(reader["BrandID"]), reader["BrandName"].ToString());
        }

        return brand;
    }
    public bool AddBrand(string brandName)
    {
        try
        {
            using var conn = DatabaseInitializer.GetConnection();
            string script = @"INSERT INTO Brands (BrandName) VALUES (@name)";

            using var cmd = new SqlCommand(script, conn);
            cmd.Parameters.AddWithValue("@name", brandName);

            conn.Open();
            int rowsAffected = cmd.ExecuteNonQuery();

            return rowsAffected > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
