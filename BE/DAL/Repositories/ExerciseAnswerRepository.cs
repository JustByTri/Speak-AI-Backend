using DAL.Data;
using DAL.Entities;
using DAL.IRepositories;

namespace DAL.Repositories
{
    public class ExerciseAnswerRepository : GenericRepository<ExerciseAnswer>, IExerciseAnswerRepository
    {
        public ExerciseAnswerRepository(SpeakAIContext context) : base(context)
        {
        }
    }
}
