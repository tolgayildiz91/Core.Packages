using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistence.Paging
{
    public static class IQueryablePaginateExtensions
    {
        public static async Task<Paginate<T>> ToPaginateAsync<T>(
            this IQueryable<T> source,
            int index,
            int size,
            CancellationToken cancellationToken = default
            )
        {
            int count = await source.CountAsync(cancellationToken).ConfigureAwait(false);
            List<T> items = await source.Skip(index*size).Take(size).ToListAsync(cancellationToken).ConfigureAwait(false);

            Paginate<T> list = new Paginate<T>()
            {
                Index = index, 
                Count = count, 
                Items = items, 
                Size = size, 
                Pages = (int)Math.Ceiling(count/(double)size)
            };

            return list;
 
        }

        public static Paginate<T> ToPaginate<T>(this IQueryable<T> source, int index, int size,
                                            int from = 0)
        {
            if (from > index) throw new ArgumentException($"From: {from} > Index: {index}, must from <= Index");

            int count = source.Count();
            List<T> items = source.Skip((index - from) * size).Take(size).ToList();
            Paginate<T> list = new()
            {
                Index = index,
                Size = size,
                Count = count,
                Items = items,
                Pages = (int)Math.Ceiling(count / (double)size)
            };
            return list;
        }
    }
}
