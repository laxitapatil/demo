using Dapper;
using Infrastructure.Base;
using Npgsql;
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace Infrastructure
{
    public class DashboardRepository : BaseRepository
    {
        public DashboardRepository(DBContext dbContext) : base(dbContext) { }


        public async Task<JsonNode?> SelectDashboardStatistics(int? visitorId, int? reporterId)
        {
            DynamicParameters parameter = new();
            parameter.Add("_visitor_id", visitorId);
            parameter.Add("_reporter_id", reporterId);
            string? response = await connection.QueryFirstAsync<string>("SELECT dashboard_case_statistics(@_visitor_id, @_reporter_id)", parameter);
            return string.IsNullOrWhiteSpace(response) ? JsonNode.Parse("[]") : JsonNode.Parse(response);
        }

    }
}