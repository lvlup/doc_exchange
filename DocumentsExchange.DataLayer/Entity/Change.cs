using System;

namespace DocumentsExchange.DataLayer.Entity
{
    public class Change
    {
        public int Id { get; set; }

        public string PropertyName { get; set; }

        public DateTime TimeSpan { get; set; }

        public string CurrentValue { get; set; }

        public string OldValue { get; set; }

        public virtual User User { get; set; }

        public int UserId { get; set; }
    }
}
