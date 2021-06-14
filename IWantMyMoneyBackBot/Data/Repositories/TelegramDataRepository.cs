using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace IWantMyMoneyBackBot.Data.Repositories
{
    /// <summary>
    /// Репозиторий, для работы с данными для работы с Telegram.
    /// </summary>
    public class TelegramDataRepository : Repository<TelegramData>
    {
        public TelegramDataRepository(Context ctx) : base(ctx)
        {
            _set = ctx.TelegramData;
        }
        
        /// <summary>
        /// Добавляет данные настроек.
        /// </summary>
        /// <param name="tgData">Данные настроек.</param>
        public void Add(TelegramData tgData)
        {
            _set.Add(tgData);
            DataChanged?.Invoke();
        }

        /// <summary>
        /// Возвращает данные настроек.
        /// </summary>
        public TelegramData GetData() => _set.FirstOrDefault();
    }
}