using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsExchange.DataLayer.Entity
{

    public enum Currency
    {
        [Description("Доллар")]
        Usd,
        [Description("Евро")]
        Euro,
        [Description("Рубль")]
        Rub
    }

    public class RecordT1
    {
        public RecordT1()
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
        public  int NumberPaymentOrder { get; set; }

        [DisplayName("Наимен. отправителя")]
        public string OrganizationSender { get; set; }

        [DisplayName("Наимен. получателя")]
        public string OrganizationReceiver { get; set; }

        [DisplayName("Сумма")]
        public decimal Amount { get; set; }

        [DisplayName("%")]
        public int Percent { get; set; }

        [DisplayName("Курс")]
        public decimal Course { get; set; }

        [DisplayName("Swift")]
        public decimal Swift { get; set; }

        [DisplayName("Отправлено")]
        public decimal Sent { get; set; }

        [DisplayName("Валюта")]
        public Currency Currency { get; set; }

        public virtual User SenderUser { get; set; }

        [NotMapped]
        [DisplayName("Отправитель")]
        public string SenderFullName => SenderUser?.FullName;

        public int SenderUserId { get; set; }

        [DisplayName("Сумма в указанной валюте")]
        public decimal AmountInCurrency { get; set; }

        [DisplayName("Итог")]
        public decimal Total { get; set; }

        [DisplayName("log")]
        public virtual Log Log { get; set; }

        public int LogId { get; set; }
    }
}
