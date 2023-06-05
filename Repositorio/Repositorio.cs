using Microsoft.EntityFrameworkCore;
using SportsShop.Data;
using SportsShop.Repositorio.IRepositorio;
using System.Linq.Expressions;

namespace SportsShop.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {

        private readonly ApplicationDbContext _context;
        internal DbSet<T> _dbSet;

        //constructor
        public Repositorio(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T t)
        {
            _context.Remove(t);
            await Commit();
        }

        public async Task<List<T>> GetAll()
        {
            IQueryable<T> query = _dbSet;
            return await query.ToListAsync();
        }

        public async Task<T> GetById(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task Save(T t)
        {
            await _context.AddAsync(t);
            await Commit();
        }

        public async Task<T> Update(T t)
        {
            _context.Update(t);
            await Commit();
            return t;
        }
    }
}
