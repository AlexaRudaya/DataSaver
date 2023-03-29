using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DataSaver.ApplicationCore.Interfaces.IRepository
{
    public interface ILinkRepository : IBaseRepository<Link>
    {
        public Task<IEnumerable<Link>> GetAllByFilterAsync(
           Func<IQueryable<Link>, IIncludableQueryable<Link, object>>? include = null,
           params Expression<Func<Link, bool>>[] expressions);
    }
}
