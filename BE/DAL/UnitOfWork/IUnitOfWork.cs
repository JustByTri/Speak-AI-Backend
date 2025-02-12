using DAL.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICourseRepository Course { get; }
        IUserRepository User { get; }
        ILevelRepository Level { get; }
        IUserLevelRepository UserLevel { get; }
        ITopicRepository Topic { get; }
        IExerciseRepository Exercise { get; }
        IEnrolledCourseRepository EnrolledCourse { get; }
        ITopicProgressRepository TopicProgress { get; }
        IExerciseProgressRepository ExerciseProgress { get; }
        IRefreshTokenRepository RefreshToken { get; }
        IChatRepository ChatMessages { get; }
        IOrderRepository Order { get; }
        void Dispose();
        Task<bool> SaveChangeAsync();
        bool SaveChange();
    }

}
