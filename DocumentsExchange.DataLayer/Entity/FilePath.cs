using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsExchange.DataLayer.Entity
{
    public class FilePath
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }
    }
}
