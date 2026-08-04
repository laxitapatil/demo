using Npgsql;
using System.Text.Json.Nodes;

namespace Infrastructure.Base
{
    public class BaseRepository : IDisposable
    {
        protected readonly NpgsqlConnection connection;
        protected readonly DBContext dbContext;

        public BaseRepository(DBContext dbContext)
        {
            this.dbContext = dbContext;
            connection = dbContext.connection;
        }

        public void Dispose() { }

        protected JsonNode? ConvertStringToJSON(string result)
        {
            return JsonObject.Parse(result);
        }
    }
}