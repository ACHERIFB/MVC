using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ciqual.Models
{

    /// <summary>
    /// Modélise une page d'éléments
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageItems<T> : List<T>
    {
        // Indice de la page courante
        public int PageIndex { get; private set; }
        // Nombre total de pages
        public int TotalPages { get; private set; }

        /// <summary>
        /// Crée une page d'éléments à partir d'une liste
        /// </summary>
        /// <param name="items">Liste des éléments de la page</param>
        /// <param name="count">Nombre total d'éléments de la source dont est extraite la liste</param>
        /// <param name="pageIndex">Indice de la page</param>
        /// <param name="pageSize">Nombre d'éléments par page</param>
        public PageItems(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;

            TotalPages = (int)Math.Ceiling(count / (double)pageSize);


            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get { return (PageIndex > 1); }
        }


        public bool HasNextPage
        {
            get { return (PageIndex < TotalPages); }
        }


        public static async Task<PageItems<T>> CreateAsync(IQueryable<T> source,
              int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();

            var items = await source.Skip((pageIndex - 1) * pageSize)
                           .Take(pageSize).ToListAsync();

            return new PageItems<T>(items, count, pageIndex, pageSize);
        }
    }

}

