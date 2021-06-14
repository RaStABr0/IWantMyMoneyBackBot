using System.ComponentModel.DataAnnotations;

namespace IWantMyMoneyBackBot
{
    public class User
    {
        [Key]
        public long TelegramId { get; set; }
        
        public string Name { get; set; }
    }
}