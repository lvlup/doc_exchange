using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsExchange.DataLayer.Entity
{
    public class FileCategory
    {
        [DisplayName("id")]
        public int Id { get; set; }

        [DisplayName("Название")]
        [MaxLength(20)]
        [Required(ErrorMessage = "Введите название")]
        public string Name { get; set; }

        [DisplayName("Активность")]
        public bool IsActive { get; set; }

        [DisplayName("Дата создания")]
        public DateTime CreatedDateTime { get; set; }

        public virtual ICollection<File> Files { get; set; } 
    }
}
