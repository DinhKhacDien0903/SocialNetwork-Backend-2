namespace SocialNetwork.Domain.IRepositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIDAsync(string id);

        Task<T> AddAsync(T entity);

        void Update(T entity);

        void Delete(T Entity);

        Task SaveChangeAsync();

    }
}
