using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class TopicProgress
    {
        public int TopicProgressId { get; set; }
        public decimal ProgressPoints { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int EnrolledCourseId { get; set; }
        public int TopicId { get; set; }
        public int UserId { get; set; }

        // Navigation properties
        public virtual EnrolledCourse EnrolledCourse { get; set; }
        public virtual Topic Topic { get; set; }
        public virtual User User { get; set; }
    }


}
