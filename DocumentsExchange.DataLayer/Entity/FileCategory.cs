using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsExchange.DataLayer.Entity
{
    public class FileCategory
    {
        [DisplayName("id")]
        public int Id { get; set; }

        [DisplayName("id")]
        public string Name { get; set; }

        [DisplayName("id")]
        public bool IsActive { get; set; }

        [DisplayName("Дата создания")]
        public DateTime CreatedDateTime { get; set; }

        public virtual ICollection<File> Files { get; set; } 
    }
}
