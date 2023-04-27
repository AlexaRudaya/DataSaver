namespace DataSaver.ApplicationCore.Interfaces.IRepository
{
    public interface ILinkRepository : IBaseRepository<Link>
    {
        /*Task<IEnumerable<Link>> GetAllByFilterAsync(
            Func<IQueryable<Link>, IIncludableQueryable<Link, object>>? include = null,
            params Expression<Func<Link, bool>>[] expressions);

        Task<IEnumerable<Link>> GetAllSortedAsync(
            Func<IQueryable<Link>, IIncludableQueryable<Link, object>>? include = null,
            params Expression<Func<Link, object>>[] sortExpressions);*/
    }
}
