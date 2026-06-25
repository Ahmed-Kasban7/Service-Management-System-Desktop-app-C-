using Application.Common;
using Application.DTOs.CampaignDTOs;
using Application.DTOs.OrderDTOs;
using Application.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Infrastructure.Repositories;
public class CampaignRepository : ICampaignRepository
{
    public int Create(CreateCampaignDto newCampaign)
    {
        using var connection = DatabaseInitializer.GetConnection();
        using var command = new SqlCommand("SP_CreateCampaign", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@CampaignName", newCampaign.CampaignName);
        command.Parameters.AddWithValue("@StartDate", newCampaign.StartDate);
        command.Parameters.AddWithValue("@EndDate", newCampaign.EndDate);
        command.Parameters.AddWithValue("@SourceId", newCampaign.SourceId);
        command.Parameters.AddWithValue("@Discount", newCampaign.Discount);
        command.Parameters.AddWithValue("@CampaignCost", newCampaign.CampaignCost);
        command.Parameters.AddWithValue("@Note", (object)newCampaign.Note ?? DBNull.Value);
        connection.Open();
        int result = Convert.ToInt32(command.ExecuteScalar());
        return result;
    }

    public IEnumerable<CampaignLookup> GetCampaignsBySourceId(int sourceId)
    {
        var list = new List<CampaignLookup>();

        using var conn = DatabaseInitializer.GetConnection();
        using var cmd = new SqlCommand("SP_GetCampaignBySourceId", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add("@SourceId", SqlDbType.Int).Value = sourceId;

        conn.Open();
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new CampaignLookup
            (
                 Convert.ToInt32(reader["CampaignId"]),
                 reader["CampaignName"].ToString() ?? string.Empty,
                 Convert.ToInt32(reader["Discount"])
            ));
        }

        return list;
    }
    public PagedResult<CampaignSummariesDto> GetPagedCampaignSummaries(int pageNumber, int pageSize)
    {
        var campaign = new List<CampaignSummariesDto>();
        using var conn = DatabaseInitializer.GetConnection();
        using var cmd = new SqlCommand("SP_GetPagedCampaignSummaries", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
        cmd.Parameters.AddWithValue("@RowsPerPage", pageSize);
        var totalParam = new SqlParameter("@TotalCampaignCount", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        cmd.Parameters.Add(totalParam);
        conn.Open();
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                campaign.Add(new CampaignSummariesDto(
                    Convert.ToInt32( reader["CampaignId"]),
                   reader["CampaignName"].ToString(),
                   reader["SourceName"].ToString(),
                DateOnly.FromDateTime((DateTime)reader["StartDate"]),
                DateOnly.FromDateTime((DateTime)reader["EndDate"])
                ));
            }
        }
        int totalCount = totalParam.Value != DBNull.Value ? (int)totalParam.Value : 0;
        return new PagedResult<CampaignSummariesDto>(campaign, totalCount, pageNumber, pageSize);
    }

    public CampaignDetailsDto GetCampaignDetails(int id)
    {
        using var conn = DatabaseInitializer.GetConnection();
        using var cmd = new SqlCommand("SP_CampaignDetails", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@CampaignId", SqlDbType.Int).Value = id;

        conn.Open();
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new CampaignDetailsDto(
                CampaignName: reader["CampaignName"].ToString() ?? string.Empty,
                SourceName: reader["SourceName"].ToString() ?? string.Empty,

                StartDate: DateOnly.FromDateTime(Convert.ToDateTime(reader["StartDate"])),
                EndDate: DateOnly.FromDateTime(Convert.ToDateTime(reader["EndDate"])),

                Discount: Convert.ToInt32(reader["Discount"]),

                Note: reader["Notes"] == DBNull.Value ? null : reader["Notes"].ToString(),

                CampaignCost: reader["CampaignCost"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["CampaignCost"])
            );
        }

        return null;
    }
    public PagedResult<CampaignSummariesDto> SearchCampaignsPaged(string searchWord, int pageNumber, int pageSize)
    {
        var campaign = new List<CampaignSummariesDto>();
        using var conn = DatabaseInitializer.GetConnection();
        using var cmd = new SqlCommand("SP_SearchCampaignPaged", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@SearchWord", (object)searchWord ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
        cmd.Parameters.AddWithValue("@RowPerPage", pageSize);
        var totalParam = new SqlParameter("@TotalCampaignCount", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        cmd.Parameters.Add(totalParam);
        conn.Open();
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                campaign.Add(new CampaignSummariesDto(
                   Convert.ToInt32(reader["CampaignId"]),
                   reader["CampaignName"].ToString(),
                   reader["SourceName"].ToString(),
                DateOnly.FromDateTime((DateTime)reader["StartDate"]),
                DateOnly.FromDateTime((DateTime)reader["EndDate"])
                ));
            }
        }
        int totalCount = totalParam.Value != DBNull.Value ? (int)totalParam.Value : 0;
        return new PagedResult<CampaignSummariesDto>(campaign, totalCount, pageNumber, pageSize);
    }

    public bool DeleteCampaign(int campaignId)
    {
        using var connection = DatabaseInitializer.GetConnection();
        using var command = new SqlCommand("SP_DeleteCampaign", connection);

        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@CampaignId", SqlDbType.Int).Value = campaignId;

        connection.Open();

        object result = command.ExecuteScalar();
        int rowsAffected = result != null ? Convert.ToInt32(result) : 0;

        return rowsAffected > 0; 
    }

    public bool UpdateCampaign(UpdateCampaignDto campaign)
    {
        using var connection = DatabaseInitializer.GetConnection();
        using var command = new SqlCommand("SP_UpdateCampaign", connection);

        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@CampaignId", SqlDbType.Int).Value = campaign.CampaignId;
        command.Parameters.Add("@CampaignName", SqlDbType.NVarChar, 250).Value = campaign.CampaignName;
        command.Parameters.Add("@StartDate", SqlDbType.Date).Value = campaign.StartDate;
        command.Parameters.Add("@EndDate", SqlDbType.Date).Value = campaign.EndDate;
        command.Parameters.Add("@SourceId", SqlDbType.Int).Value = campaign.SourceId;
        command.Parameters.Add("@Discount", SqlDbType.Int).Value = campaign.Discount;
        command.Parameters.Add("@Notes", SqlDbType.NVarChar, -1).Value = (object)campaign.Note ?? DBNull.Value;

        connection.Open();

        object result = command.ExecuteScalar();
        int rowsAffected = result != null ? Convert.ToInt32(result) : 0;

        return rowsAffected > 0;
    }
}