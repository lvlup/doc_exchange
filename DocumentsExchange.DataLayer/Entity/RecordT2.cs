using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsExchange.DataLayer.Entity
{
    public class RecordT2
    {
        public int Id { get; set; }

        public virtual Organization Organization { get; set; }
        public int OranizationId { get; set; }

        public virtual FilePath File { get; set; }
        public int FilePathId { get; set; }

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
        public virtual ICollection<Log> Logs { get; set; }
    }
}
