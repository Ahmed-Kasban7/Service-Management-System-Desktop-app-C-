using Application.Common;
using Application.DTOs.SparePartDtos;
using Application.DTOs.VisitDTOs;
using Application.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Infrastructure.Repositories;

public class VisitRepository : IVisitRepository
{
    public int CreateVisitWithParts(CreateVisistDto newVisit)
    {
        DataTable partsDataTable = CreateSparePartsDataTable(newVisit.SpareParts.ToList());

        using var connection = DatabaseInitializer.GetConnection();
        using var command = new SqlCommand("sp_CreateVisitWithParts", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = newVisit.AppointmentID;
        command.Parameters.Add("@Notes", SqlDbType.NVarChar).Value = (object)newVisit.Notes ?? DBNull.Value;
        command.Parameters.Add("@ActionsTaken", SqlDbType.NVarChar).Value = (object)newVisit.ActionsTaken ?? DBNull.Value;
        command.Parameters.Add("@Diagnosis", SqlDbType.NVarChar).Value = newVisit.Diagnosis;

        command.Parameters.Add("@TotalCostToCustomer", SqlDbType.Decimal).Value = newVisit.TotalCostToCustomer;
        command.Parameters.Add("@TransportationCost", SqlDbType.Decimal).Value = newVisit.TransportationCost;
        command.Parameters.Add("@AmountPaid", SqlDbType.Decimal).Value = newVisit.AmountPaid;

        command.Parameters.Add("@TransportationBearer", SqlDbType.TinyInt)
               .Value = newVisit.TransportationBearer.HasValue
                    ? (byte)newVisit.TransportationBearer.Value
                    : DBNull.Value;
        command.Parameters.Add("@PartsTransportationCost", SqlDbType.Decimal).Value = (object)newVisit.PartsTransportationCost ?? DBNull.Value;
        command.Parameters.Add("@PaidByEmployeeID", SqlDbType.Int)
       .Value = (object?)newVisit.PaidByEmployeeID ?? DBNull.Value;

        SqlParameter tvpParameter = command.Parameters.AddWithValue("@SparePartsList", partsDataTable);
        tvpParameter.SqlDbType = SqlDbType.Structured;
        tvpParameter.TypeName = "dbo.UsedSparePartsType";

        connection.Open();

        object result = command.ExecuteScalar();

        if (result != null && int.TryParse(result.ToString(), out int insertedId))
        {
            return insertedId;
        }

        throw new Exception("فشل السيرفر في إرجاع المعرف الخاص بالزيارة الجديدة.");
    }

    private DataTable CreateSparePartsDataTable(List<SparePartDto> spareParts)
    {
        DataTable table = new DataTable();

        table.Columns.Add("PartName", typeof(string));
        table.Columns.Add("Quantity", typeof(int));
        table.Columns.Add("UnitPrice", typeof(decimal)); 

        if (spareParts != null && spareParts.Count > 0)
        {
            foreach (var part in spareParts)
            {
                if (!string.IsNullOrWhiteSpace(part.PartName))
                {
                    table.Rows.Add(part.PartName, part.Quantity, part.UnitPrice);
                }
            }
        }

        return table;
    }

    public VisitDetailsDto GetVisitDetailsByAppointmentId(int appointmentId)
    {
        using var connection = DatabaseInitializer.GetConnection();
        using var command = new SqlCommand("SP_VisitDetails", connection);

        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = appointmentId;

        connection.Open();
        using SqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            var visitDetails = new VisitDetailsDto
            {
                ScheduledDate = reader.GetDateTime(reader.GetOrdinal("ScheduledDate")),
                VisitType = Convert.ToInt32( reader["VisitType"]),
                TechnicianName = reader.IsDBNull(reader.GetOrdinal("TechnicianName")) ? "—" : reader.GetString(reader.GetOrdinal("TechnicianName")),
                AssistantName = reader.IsDBNull(reader.GetOrdinal("AssistantName")) ? "—" : reader.GetString(reader.GetOrdinal("AssistantName")),
                DriverName = reader.IsDBNull(reader.GetOrdinal("DriverName")) ? "—" : reader.GetString(reader.GetOrdinal("DriverName")),
                PaidByEmployeeName = reader.IsDBNull(reader.GetOrdinal("PaidByEmployeeName")) ? "—" : reader.GetString(reader.GetOrdinal("PaidByEmployeeName")),

                Diagnosis = reader.GetString(reader.GetOrdinal("Diagnosis")),
                ActionsTaken = reader.IsDBNull(reader.GetOrdinal("ActionsTaken")) ? "" : reader.GetString(reader.GetOrdinal("ActionsTaken")),
                Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? "" : reader.GetString(reader.GetOrdinal("Notes")),

                TotalCostToCustomer = reader.GetDecimal(reader.GetOrdinal("TotalCostToCustomer")),
                TransportationCost = reader.GetDecimal(reader.GetOrdinal("TransportationCost")),
                AmountPaid = reader.GetDecimal(reader.GetOrdinal("AmountPaid")),
                RemainingAmount = reader.GetDecimal(reader.GetOrdinal("RemainingAmount")),

                PartsTransportationCost = reader.IsDBNull(reader.GetOrdinal("PartsTransportationCost")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PartsTransportationCost")),
                TransportationBearer = reader.IsDBNull(reader.GetOrdinal("TransportationBearer")) ? (byte)0 : reader.GetByte(reader.GetOrdinal("TransportationBearer")),

                SpareParts = new List<VisitSparePartDto>()
            };

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    var part = new VisitSparePartDto
                    {
                        PartName = reader.GetString(reader.GetOrdinal("PartName")),
                        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                        UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                        TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice"))
                    };

                    visitDetails.SpareParts.Add(part);
                }
            }

            return visitDetails;
        }

        return null;
    }



}