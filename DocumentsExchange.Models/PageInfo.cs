using System;
using DocumentsExchange.Common.Extensions;

namespace DocumentsExchange.DataAccessLayer.Models
{
    public class PageInfo
    {
        public PageInfo(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }

        public PageInfo()
        {

        }

        public int? LastId { get; set; }

        public DateTime? TimeStamp { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public bool NoPaging => Page == 0 && PageSize == 0;

        public long? TimeStampTicks
        {
            get { return TimeStamp?.Convert(); }
            set
            {
                if (!value.HasValue)
                    return;

                TimeStamp = value.Value.Convert();
            }
        }
    }
}
