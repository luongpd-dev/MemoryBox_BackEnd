using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Domain.Entities
{
    public class Statistic
    {
        public Guid StatId { get; set; }
        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; }
        public int TotalMessages { get; set; } = 0;
        public int OpenedMessages { get; set; } = 0;
        public int PendingMessages { get; set; } = 0;
        public int MissedMessages { get; set; } = 0;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
