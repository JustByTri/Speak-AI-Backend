using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public class Exercise : BaseEntity
    {
        public string Content { get; set; }
        public decimal MaxPoint { get; set; }
        public int TypeId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Guid TopicId { get; set; }
        public virtual Topic Topic { get; set; }
        public virtual Types Type { get; set; }
        public virtual ICollection<ExerciseProgress> ExerciseProgresses { get; set; }
        public virtual ICollection<ExerciseQuestion> ExerciseQuestions { get; set; }
    }
}