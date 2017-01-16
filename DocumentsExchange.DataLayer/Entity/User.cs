using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using DocumentsExchange.DataLayer.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DocumentsExchange.DataLayer.Entity
{
    public class User : IdentityUser<int, AppUserLogin, AppUserRole, AppUserClaim>
    {
        [DisplayName("id")]
        public override int Id { get; set; }

        [DisplayName("Логин")]
        public override string UserName { get; set; }

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
