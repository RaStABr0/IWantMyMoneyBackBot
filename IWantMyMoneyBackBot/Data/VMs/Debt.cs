using System;
using System.ComponentModel.DataAnnotations;

namespace IWantMyMoneyBackBot
{
    /// <summary>
    /// Сущность займа.
    /// </summary>
    public class Debt
    {
        [Key]
        public Guid Id { get; set; }
        
        /// <summary>
        /// Должник.
        /// </summary>
        public long DebtorId { get; set; }
        
        /// <summary>
        /// Сумма займа.
        /// </summary>
        public float Sum { get; set; }
        
        /// <summary>
        /// Заниматель.
        /// </summary>
        public long LenderId { get; set; }
    }
}