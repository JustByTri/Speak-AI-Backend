using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime Birthday { get; set; }
        public string Gender { get; set; }
        public bool Status { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public virtual UserLevel UserLevel { get; set; }
        public virtual ICollection<EnrolledCourse> EnrolledCourses { get; set; }
        public virtual ICollection<TopicProgress> TopicProgresses { get; set; }
        public virtual ICollection<ExerciseProgress> ExerciseProgresses { get; set; }
    }
}
