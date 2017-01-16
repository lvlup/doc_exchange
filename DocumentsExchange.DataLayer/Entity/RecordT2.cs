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
        public int NumberPaymentOrder { get; set; }

        [DisplayName("Наимен. отправителя")]
        public string OrganizationSender { get; set; }

        [DisplayName("Наимен. получателя")]
        public string OrganizationReceiver { get; set; }

        [DisplayName("Сумма")]
        public decimal Amount { get; set; }

        [DisplayName("%")]
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
