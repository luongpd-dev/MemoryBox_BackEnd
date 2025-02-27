using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Domain.Entities
{
    public class Attachment
    {
        public Guid AttachmentId { get; set; }
        public Guid MessageId { get; set; }
        public virtual Message Message { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
