using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class ExerciseProgress
    {
        public int ExerciseProgressId { get; set; }
        public decimal ProgressPoints { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public int EnrolledCourseId { get; set; }
        public int ExerciseId { get; set; }
        public int UserId { get; set; }

        // Navigation properties
        public virtual EnrolledCourse EnrolledCourse { get; set; }
        public virtual Exercise Exercise { get; set; }
        public virtual User User { get; set; }
    }
}
