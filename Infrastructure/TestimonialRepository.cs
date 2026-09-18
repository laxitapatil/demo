using Core.Domain;
using Dapper;
using Infrastructure.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class TestimonialRepository : BaseRepository, IRepository<Testimonial>
    {
        public TestimonialRepository(DBContext dbContext) : base(dbContext) { }

        public async Task<Testimonial?> SelectOne(int id) =>
            await dbContext.Set<Testimonial>().Where(t => t.Id == id).FirstOrDefaultAsync();

        public async Task Insert(Testimonial testimonial)
        {
            await dbContext.Set<Testimonial>().AddAsync(testimonial);
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(Testimonial testimonial)
        {
            dbContext.Set<Testimonial>().Update(testimonial);
            await dbContext.SaveChangesAsync();
        }

        public async Task Delete(Testimonial testimonial)
        {
            dbContext.Set<Testimonial>().Remove(testimonial);
            await dbContext.SaveChangesAsync();
        }

        public async Task<string> SelectAll(int? id, Guid? companyId)
        {
            DynamicParameters parameter = new();
            parameter.Add("_id", id);
            parameter.Add("_company_id", companyId);
            return await connection.QueryFirstAsync<string>("SELECT testimonial_selectall(@_id, @_company_id)", parameter);
        }
    }
}