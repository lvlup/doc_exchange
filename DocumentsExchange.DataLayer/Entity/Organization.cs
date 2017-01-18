﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsExchange.DataLayer.Entity
{
   public class Organization
    {
        public int Id { get; set; }

        [DisplayName("Название")]
        [MaxLength(20)]
        [Required(ErrorMessage = "Введите название организации")]
        public string Name { get; set; }

        [DisplayName("Дата создания")]
        public DateTime CreatedDateTime { get; set; }

        [DisplayName("Активность")]
        public bool IsActive { get; set; }

        public virtual ICollection<RecordT1> RecordT1s { get; set; }

        public virtual ICollection<RecordT2> RecordT2s { get; set; }

        public virtual ICollection<File> Files { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }

    
    public enum TableName
    {
        Table1,
        Table2
    }
}
