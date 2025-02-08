using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Exercise
    {
        public int ExerciseId { get; set; }
        public string Content { get; set; }
        public decimal MaxPoint { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TopicId { get; set; }

        // Navigation properties
        public virtual Topic Topic { get; set; }
        public virtual ICollection<ExerciseProgress> ExerciseProgresses { get; set; }
    }
}
