using System.Collections.Generic;
using System.Linq;

namespace DocumentsExchange.Models
{
    public class ItemsResult<TItem>
    {
        public IEnumerable<TItem> Items { get; private set; }

        public int Total { get; private set; }

        public ItemsResult(IEnumerable<TItem> items, int total)
        {
            Items = items;
            Total = total;
        }

        public ItemsResult(IEnumerable<TItem> items)
        {
            Items = items;
            Total = items.Count();
        }
    }
}
