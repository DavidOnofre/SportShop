using System.Linq.Expressions;

namespace SportsShop.Repositorio.IRepositorio
{
    public interface IRepositorio<T> where T : class
    {
        Task Save(T t);

        Task<List<T>> GetAll();

        Task<T> GetById(Expression<Func<T, bool>>? filter = null);

        Task<T> Update(T t);

        Task Delete(T t);

        Task Commit();
    }
}
