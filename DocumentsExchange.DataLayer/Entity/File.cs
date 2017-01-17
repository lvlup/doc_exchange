using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsExchange.DataLayer.Entity
{

    public class File
    {
        public int Id { get; set; }

        public virtual Organization Organization { get; set; }
        public int OranizationId { get; set; }

        public virtual User User { get; set; }
        public int UserId { get; set; }

        public virtual FileCategory Category { get; set; }
        public int CategoryId { get; set; }

        [DisplayName("Имя")]
        [StringLength(255)]
        [MaxLength(20)]
        [Required(ErrorMessage = "Введите название файла")]
        public string FileName { get; set; }

        [DisplayName("Дата добавления")]
        public DateTime AddedDateTime { get; set; }

        [StringLength(100)]
        public string ContentType { get; set; }

        public byte[] Content { get; set; }

        [NotMapped]
        [DisplayName("Автор добавления")]
        public string UserFullName => User?.FullName;

        [DisplayName("Тип файла")]
        public string FileType { get; set; }

        
    }
}
