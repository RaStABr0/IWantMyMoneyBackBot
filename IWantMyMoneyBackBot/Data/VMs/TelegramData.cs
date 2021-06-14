using System.ComponentModel.DataAnnotations;

namespace IWantMyMoneyBackBot
{
    public class TelegramData
    {
        [Key]
        public string BotId { get; set; }
    }
}