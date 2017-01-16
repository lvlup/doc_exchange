using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsExchange.DataLayer.Entity
{
    public class User
    {
        [DisplayName("id")]
        public int Id { get; set; }

        [DisplayName("Логин")]
        public string UserName { get; set; }

        //[DisplayName("Имя")]
        public string FirstName { get; set; }

        //[DisplayName("Фамилия")]
        public string LastName { get; set; }


        [DisplayName("Активность")]
        public bool IsActive { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        [NotMapped]
        [DisplayName("ФИО")]
        public string FullName => FirstName + " " + LastName;

        //todo role

        //public virtual Organization Organization { get; set; }
        //public int OrganizationId { get; set; }

        //[DisplayName("Дата активности")]
        //public DateTime ActivityDateTime { get; set; }
    }
}
