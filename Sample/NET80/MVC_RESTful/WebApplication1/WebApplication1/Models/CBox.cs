using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class CBox
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // ★ 自動生成主鍵
        public long _Key { get; set; } = 0;
        public string _Name { get; set; } = string.Empty;
        public int _Level { get; set; } = 0;
        public DateTime _CreateTime { get; set; } = DateTime.MinValue;
    }
}
