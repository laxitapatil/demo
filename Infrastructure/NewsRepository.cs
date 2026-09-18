using Core.Domain;
using Dapper;
using Infrastructure.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class NewsRepository : BaseRepository, IRepository<News>
    {
        public NewsRepository(DBContext dbContext) : base(dbContext) { }

        public async Task<News?> SelectOne(int id) =>
            await dbContext.Set<News>().Where(n => n.Id == id).FirstOrDefaultAsync();

        public async Task Insert(News news)
        {
            await dbContext.Set<News>().AddAsync(news);
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(News news)
        {
            dbContext.Set<News>().Update(news);
            await dbContext.SaveChangesAsync();
        }

        public async Task Delete(News news)
        {
            dbContext.Set<News>().Remove(news);
            await dbContext.SaveChangesAsync();
        }

        public async Task<string> SelectAll(int? id, Guid? companyId)
        {
            DynamicParameters parameter = new();
            parameter.Add("_id", id);
            parameter.Add("_company_id", companyId);
            return await connection.QueryFirstAsync<string>("SELECT news_selectall(@_id, @_company_id)", parameter);
        }
    }
}