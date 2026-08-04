using Core.Response;

namespace Infrastructure.Base
{
    internal interface IRepository<T> where T : class
    {
        public Task<T> SelectOne(int id);
        public Task Insert(T obj);
        public Task Update(T obj);
        public Task Delete(T obj);
    }
}