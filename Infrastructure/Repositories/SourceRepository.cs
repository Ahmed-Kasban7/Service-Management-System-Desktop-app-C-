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

}
