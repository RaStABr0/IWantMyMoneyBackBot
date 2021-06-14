using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IWantMyMoneyBackBot.Data.Repositories
{
    /// <summary>
    /// Репозиторий пользоваетелей.
    /// </summary>
    public class UsersRepository : Repository<User>
    {
        public event Action<User> UserAdded; 
        

        public event Action<User> UserRemoved; 
        
        public UsersRepository(Context ctx) : base(ctx) => _set = ctx.Users;

        /// <summary>
        /// Добавить нового пользователя.
        /// </summary>
        public async Task Add(User newUser)
        {
            if (_set.Contains(newUser)) return;
            
            await _set.AddAsync(newUser);
            
            UserAdded?.Invoke(newUser);
            DataChanged?.Invoke();
        }

        public async Task<User> Get(long userId)
        {
            var user = await _set.FirstOrDefaultAsync(u => u.TelegramId == userId);

            return user;
        }

        /// <summary>
        /// Получить всех пользователей.
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetAll() => await _set.ToListAsync();

        /// <summary>
        /// Удалить пользователя.
        /// </summary>
        /// <param name="telegramId">Идентификатор пользователя.</param>
        public void RemoveUser(long telegramId)
        {
            var userToRemove = _set.First(u => u.TelegramId == telegramId);
            _set.Remove(userToRemove);

            UserRemoved?.Invoke(userToRemove);
            DataChanged?.Invoke();
        }
    }
}