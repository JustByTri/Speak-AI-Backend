using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class EnrolledCourse
    {
        public int EnrolledCourseId { get; set; }
        public bool IsCompleted { get; set; }
        public decimal ProgressPoints { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Course Course { get; set; }
        public virtual ICollection<TopicProgress> TopicProgresses { get; set; }
    }

}
