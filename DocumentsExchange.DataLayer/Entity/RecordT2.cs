using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsExchange.DataLayer.Entity
{
    public class RecordT2
    {
        public RecordT2()
        {
            File = new FilePath();
        }

        public int Id { get; set; }

        public virtual Organization Organization { get; set; }
        public int OranizationId { get; set; }

        public FilePath File { get; set; }

        [DisplayName("Дата")]
        public DateTime CreatedDateTime { get; set; }

        [DisplayName("п/п №")]
        [Required(ErrorMessage = "Поле п/п № не может быть пустой")]
        public int NumberPaymentOrder { get; set; }

        [DisplayName("Наимен. отправителя")]
        [MaxLength(20)]
        [Required(ErrorMessage = "Введите наименование получателя")]
        public string OrganizationSender { get; set; }

        [DisplayName("Наимен. получателя")]
        [MaxLength(20)]
        [Required(ErrorMessage = "Введите наименование отправителя")]
        public string OrganizationReceiver { get; set; }

        [DisplayName("Сумма")]
        [Required(ErrorMessage = "Поле сумма не может быть пустой")]
        public decimal Amount { get; set; }

        [DisplayName("%")]
        [Required(ErrorMessage = "Поле % не может быть пустой")]
        public int Percent { get; set; }

        [DisplayName("Вычет %")]
        public decimal Deduction { get; set; }

        [DisplayName("Получено")]
        public decimal Received { get; set; }

        public virtual User SenderUser { get; set; }

        public int SenderUserId { get; set; }

        [NotMapped]
        [DisplayName("Отправитель")]
        public string SenderFullName => SenderUser?.FullName;

        [DisplayName("log")]
        public virtual Log Log { get; set; }

        public int LogId { get; set; }
    }
}
