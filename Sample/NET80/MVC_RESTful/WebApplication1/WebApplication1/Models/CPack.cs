using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class CPack
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // ★ 自動生成主鍵
        public long _SeqNo { get; set; } = 0;
        public string _Msg { get; set; } = string.Empty;
        public int _Code { get; set; } = 0;
        public DateTime _UpdateTime { get; set; } = DateTime.MinValue;
    }
}
