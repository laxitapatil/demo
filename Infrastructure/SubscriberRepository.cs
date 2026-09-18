using Core.Domain;
using Dapper;
using Infrastructure.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class SubscriberRepository : BaseRepository, IRepository<Subscriber>
    {
        public SubscriberRepository(DBContext dbContext) : base(dbContext) { }

        public async Task<Subscriber?> SelectOne(int id) =>
            await dbContext.Set<Subscriber>().Where(s => s.Id == id).FirstOrDefaultAsync();

        public async Task Insert(Subscriber subscriber)
        {
            await dbContext.Set<Subscriber>().AddAsync(subscriber);
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(Subscriber subscriber)
        {
            dbContext.Set<Subscriber>().Update(subscriber);
            await dbContext.SaveChangesAsync();
        }

        public async Task Delete(Subscriber subscriber)
        {
            dbContext.Set<Subscriber>().Remove(subscriber);
            await dbContext.SaveChangesAsync();
        }

        public async Task<string> SelectAll()
        {
            return await connection.QueryFirstAsync<string>("SELECT subscriber_selectall()");
        }
    }
}