using SportsShop.Data;
using SportsShop.Models;
using SportsShop.Repositorio;
using SportsShop.Repositorio.IRepositorio;

namespace SportShop.Repositorio
{
    public class SportRepositorio : Repositorio<Sport>, ISportRepositorio
    {

        private readonly ApplicationDbContext _context;

        public SportRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
