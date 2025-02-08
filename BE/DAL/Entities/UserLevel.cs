using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class UserLevel
    {
        public int UserLevelId { get; set; }
        public decimal Point { get; set; }
        public string LevelName { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string LevelId { get; set; }

        // Navigation properties
        public virtual Level Level { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
