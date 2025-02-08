using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Topic
    {
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public decimal MaxPoint { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CourseId { get; set; }

        // Navigation properties
        public virtual Course Course { get; set; }
        public virtual ICollection<Exercise> Exercises { get; set; }
        public virtual ICollection<TopicProgress> TopicProgresses { get; set; }
    }
}
