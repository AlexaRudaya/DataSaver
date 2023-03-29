using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DataSaver.Infrastructure.Repositories
{
    public sealed class LinkRepository : BaseRepository<Link>, ILinkRepository
    {
        private readonly LinkContext _linkContext;

        public LinkRepository(LinkContext linkContext) : base(linkContext) 
        {
            _linkContext = linkContext;
        }

        public async Task<IEnumerable<Link>> GetAllByFilterAsync(
            Func<IQueryable<Link>, IIncludableQueryable<Link, object>>? include = null,
            params Expression<Func<Link, bool>>[] expressions)
        {
            IQueryable<Link> query = _linkContext.Links;

            var expressionsList = expressions.ToList();
            expressionsList.ForEach(_ => query = query.Where(_));

            if (include is not null)
            {
                query = include(query);
            } 

            return await query.AsNoTracking().ToListAsync();
        }
    }
}
