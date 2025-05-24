namespace DAL.Entities
{
    public class ExerciseAnswer : BaseEntity
    {
        public Guid ExerciseQuestionId { get; set; }
        public string Content { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }

      
        public virtual ExerciseQuestion ExerciseQuestion { get; set; }
    }
}
