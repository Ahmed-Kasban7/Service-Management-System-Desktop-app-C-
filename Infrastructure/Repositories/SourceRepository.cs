using Application.DTOs.SourceDTOs;
using Application.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class SourceRepository:ISourceRepository
{
    public IEnumerable<SourceDto> GetAllSources()
    {
        var sourcesList = new List<SourceDto>();

        using var connection = DatabaseInitializer.GetConnection();
        using var command = new SqlCommand("SP_GetAllSources", connection);

        command.CommandType = CommandType.StoredProcedure;

        connection.Open();

        using var reader  =  command.ExecuteReader();

        while (reader.Read())
        {
            var source = new SourceDto(reader["SourceName"].ToString(), Convert.ToInt32(reader["SourceID"]) );
            sourcesList.Add(source);
        }

        return sourcesList;

    }
    public bool AddSource(string sourceName)
    {
        using var conn = DatabaseInitializer.GetConnection();
        conn.Open();

        using var cmd = new SqlCommand("SP_AddSource", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@SourceName", sourceName.Trim());

        var result = cmd.ExecuteScalar();
        int rowsAffected = result != null ? Convert.ToInt32(result) : 0;

        return rowsAffected > 0;
    }

    public bool UpdateSource(int sourceId, string sourceName)
    {
        using var conn = DatabaseInitializer.GetConnection();
        conn.Open();

        using var cmd = new SqlCommand("SP_UpdateSource", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@SourceId", sourceId);
        cmd.Parameters.AddWithValue("@SourceName", sourceName.Trim());

        var result = cmd.ExecuteScalar();
        int rowsAffected = result != null ? Convert.ToInt32(result) : 0;

        return rowsAffected > 0;
    }

    public bool DeleteSource(int sourceId)
    {
        using var conn = DatabaseInitializer.GetConnection();
        conn.Open();

        using var cmd = new SqlCommand("SP_DeleteSource", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@SourceId", sourceId);

        var result = cmd.ExecuteScalar();
        int rowsAffected = result != null ? Convert.ToInt32(result) : 0;

        return rowsAffected > 0;
    }

}
