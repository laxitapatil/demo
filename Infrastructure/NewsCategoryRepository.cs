using Core.Domain;
using Dapper;
using Infrastructure.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class NewsCategoryRepository : BaseRepository, IRepository<NewsCategory>
    {
        public NewsCategoryRepository(DBContext dbContext) : base(dbContext) { }

        public async Task<NewsCategory?> SelectOne(int id) =>
            await dbContext.Set<NewsCategory>().Where(n => n.Id == id).FirstOrDefaultAsync();

        public async Task Insert(NewsCategory newsCategory)
        {
            await dbContext.Set<NewsCategory>().AddAsync(newsCategory);
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(NewsCategory newsCategory)
        {
            dbContext.Set<NewsCategory>().Update(newsCategory);
            await dbContext.SaveChangesAsync();
        }

        public async Task Delete(NewsCategory newsCategory)
        {
            dbContext.Set<NewsCategory>().Remove(newsCategory);
            await dbContext.SaveChangesAsync();
        }

        public async Task<string> SelectAll(int? id)
        {
            DynamicParameters parameter = new();
            parameter.Add("_id", id);
            return await connection.QueryFirstAsync<string>("SELECT news_category_selectall(@_id)", parameter);
        }
    }
}