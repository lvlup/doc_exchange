using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsExchange.DataLayer.Entity
{
    [ComplexType]
    public class FilePath
    {
        [StringLength(255)]
        public string FileName { get; set; }

        [StringLength(255)]
        public string OriginalFileName { get; set; }
    }
}
