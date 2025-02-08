using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Level : BaseEntity
    {
       
        public string LevelName { get; set; }
        public decimal MinPoint { get; set; }
        public decimal MaxPoint { get; set; }
        public DateTime UpdatedAt { get; set; }
      

      
        public virtual ICollection<UserLevel> UserLevels { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
