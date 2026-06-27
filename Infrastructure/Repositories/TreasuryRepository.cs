using Application.DTOs.Treasury;
using Application.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.DTOs.Treasury;
using Application.Repositories;

namespace Infrastructure.Repositories;

public class TreasuryRepository : ITreasuryRepository
{
    public BalanceDTOs GetTreasuryOverview()
    {
        using var conn = DatabaseInitializer.GetConnection();
        using var cmd = new SqlCommand("SP_GetCurrentBalance", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        conn.Open();
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            decimal balance = reader["CurrentBalance"] != DBNull.Value ? Convert.ToDecimal(reader["CurrentBalance"]) : 0.00m;
            DateTime lastUpdated = reader["LastUpdated"] != DBNull.Value ? Convert.ToDateTime(reader["LastUpdated"]) : DateTime.Now;

            return new BalanceDTOs ( balance,lastUpdated);
        }

        return new BalanceDTOs (0.00m,  DateTime.Now );
    }
}
