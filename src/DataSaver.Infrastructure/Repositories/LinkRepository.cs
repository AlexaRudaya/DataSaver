namespace DataSaver.Infrastructure.Repositories
{
    public sealed class LinkRepository : BaseRepository<Link>, ILinkRepository
    {
        private readonly LinkContext _linkContext;

        public LinkRepository(LinkContext linkContext) : base(linkContext) 
        {
            _linkContext = linkContext;
        }

        //public async Task<IEnumerable<Link>> GetAllByFilterAsync(
        //    Func<IQueryable<Link>, IIncludableQueryable<Link, object>>? include = null,
        //    params Expression<Func<Link, bool>>[] expressions)
        //{
        //    IQueryable<Link> query = _linkContext.Links;

        //    var expressionsList = expressions.ToList();
        //    expressionsList.ForEach(_ => query = query.Where(_));

        //    if (include is not null)
        //    {
        //        query = include(query);
        //    } 

        //    return await query.AsNoTracking().ToListAsync();
        //}

        public async Task<IEnumerable<Link>> GetAllByAsync(
            Func<IQueryable<Link>, IIncludableQueryable<Link, object>>? include = null, 
            Expression<Func<Link, object>> sortExpression = null, params Expression<Func<Link, object>>[] filterExpressions)
        {
            IQueryable<Link> query = _linkContext.Links;

            var sortExpressionsList = sortExpressions.ToList();
            //sortExpressionsList.ForEach(_ => query = query.OrderByDescending(_));

            if (include is not null)
            {
                query = include(query);
            }

            return await query.AsNoTracking().ToListAsync();
        }
        private IQueryable<Link> GetAllByFilterAsync(
           Func<IQueryable<Link>, IIncludableQueryable<Link, object>>? include = null,
           params Expression<Func<Link, bool>>[] expressions)
        {
            IQueryable<Link> query = _linkContext.Links;

            if (include is not null)
            {
                query = include(query);
            }

            var expressionsList = expressions.ToList();
            expressionsList.ForEach(_ => query = query.Where(_));

            return query;
        }

        private IQueryable<Link> GetAllBySortAsync(
           Func<IQueryable<Link>, IIncludableQueryable<Link, object>>? include = null,
           Expression<Func<Link, bool>> expression)
        {
            IQueryable<Link> query = _linkContext.Links;

            if (include is not null)
            {
                query = include(query);
            }

            query = query.OrderBy(expression);          
           
            return query;
        }

    }
}
