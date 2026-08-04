using Core.Domain;
using Core.Response;
using Dapper;
using Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;
using System.Text.Json.Nodes;

namespace Infrastructure
{
    public class AspNetUserLogsRepository : BaseRepository
    {
        public AspNetUserLogsRepository(DBContext dbContext) : base(dbContext) { }


        public async Task Insert(AspNetUserLogs userLogs)
        {
            await dbContext.AspNetUserLogs.AddAsync(userLogs);
            await dbContext.SaveChangesAsync();
        }

        public async Task<int> Update(AspNetUserLogs userLogs)
        {
            dbContext.AspNetUserLogs.Update(userLogs);
            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(AspNetUserLogs userLogs)
        {
            dbContext.AspNetUserLogs.Remove(userLogs);
            return await dbContext.SaveChangesAsync();
        }
        public async Task<JsonNode?> SelectAll(DateTime? fromDate, DateTime? toDate, string? searchText)
        {
            DynamicParameters parameters = new();
            parameters.Add("_from_date", fromDate);
            parameters.Add("_to_date", toDate);
            parameters.Add("_search_text", searchText);

            string? response = await connection.QueryFirstOrDefaultAsync<string>("SELECT aspnet_user_logs_selectall(@_from_date, @_to_date, @_search_text)",parameters);
            return string.IsNullOrWhiteSpace(response) ? JsonNode.Parse("[]") : JsonNode.Parse(response);
        }
        public async Task<AspNetUserLogs?>SelectOne(int? id) =>
            await dbContext.AspNetUserLogs.Where(AspNetUserLogs => AspNetUserLogs.Id == id).FirstOrDefaultAsync();
    }
}
