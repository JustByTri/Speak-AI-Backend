using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class ExerciseQuestion : BaseEntity
    {
        public Guid ExerciseId { get; set; }
        public int TypeId { get; set; } 
        public string Content { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }

   
        public virtual Exercise Exercise { get; set; }
        public virtual Types Type { get; set; }
        public virtual ICollection<ExerciseAnswer> ExerciseAnswers { get; set; }
    }
}
