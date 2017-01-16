using System;
using System.Collections.Generic;
using System.Linq;

namespace DocumentsExchange.DataLayer.Entity
{
    public class Log
    {
        public Log()
        {
            Changes = new HashSet<Change>();
        }

        public int Id { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public virtual User User { get; set; }

        public int UserId { get; set; }

        public virtual ICollection<Change> Changes { get; set; }

        public override string ToString()
        {
            string lastChangeInfo = "";
            if (Changes.Count > 0)
            {
                var lastChange = Changes.Last();
                lastChangeInfo = $"\nДата изменения: {lastChange.TimeSpan.ToString("dd.MM.yyyy")}\nИзменил: {User.FullName}\nИзменил значение поля \"{lastChange.PropertyName}\"\nПредыдущее значение: {lastChange.OldValue}\nТекущее значение: {lastChange.CurrentValue}";
            }
            
            return string.Format($"Дата добавления: {CreatedDateTime.ToString("dd.MM.yyyy")}\nДобавил: {User.FullName}{lastChangeInfo}");
        }
    }
}
