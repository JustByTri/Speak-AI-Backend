using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Types
    {
        [Key]
        public int TypeId { get; set; } 
        public string TypeName { get; set; }
        public string Content { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Exercise> Exercises { get; set; }
        public virtual ICollection<ExerciseQuestion> ExerciseQuestions { get; set; }
    }
}
