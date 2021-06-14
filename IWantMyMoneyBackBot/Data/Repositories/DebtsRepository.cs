using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IWantMyMoneyBackBot.Data.Repositories
{
    /// <summary>
    /// Репозиторий для работы с сущностью 
    /// </summary>
    public class DebtsRepository : Repository<Debt>
    {
        public DebtsRepository(Context ctx) : base(ctx) => _set = ctx.Debts;

        public async Task Add(Debt newDebt)
        {
            await _set.AddAsync(newDebt);
            
            DataChanged?.Invoke();
        }

        public async Task AddRange(IEnumerable<Debt> newDebts)
        {
            await _set.AddRangeAsync(newDebts);
            
            DataChanged?.Invoke();
        }

        public void RemoveRange(IEnumerable<Debt> debtsToRemove)
        {
            _set.RemoveRange(debtsToRemove);
            
            DataChanged?.Invoke();
        }

        public (List<Debt> debts, List<Debt> lends) GetDebtsAndLends(User user)
        {
            var debts = _set.Where(d => d.DebtorId == user.TelegramId);
            var lends = _set.Where(d => d.LenderId == user.TelegramId);

            return (debts.ToList(), lends.ToList());
        } 

        // public static Debt Get(string debtor, string lender)
        // {
        //     var debt = _debts.FirstOrDefault(d => d.DebtorId == debtor && d.LenderId == lender);
        //
        //     return debt;
        // }
        //
        // public static List<Debt> GetMany(string debtorOrLender, bool isLender)
        // {
        //     var query = isLender
        //         ? _debts.Where(d => d.LenderId == debtorOrLender)
        //         : _debts.Where(d => d.DebtorId == debtorOrLender);
        //
        //     return query.ToList();
        // }
    }
}