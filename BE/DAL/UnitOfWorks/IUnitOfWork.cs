using DAL.GenericRepository.IRepository;
using DAL.IRepositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace DAL.UnitOfWorks
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
        ITransactionRepository Transaction { get; }

        IVoucherRepository Voucher { get; }

        Task<IDbContextTransaction> BeginTransactionAsync(System.Data.IsolationLevel isolationLevel);

        void Dispose();
        Task<bool> SaveChangeAsync();


        bool SaveChange();
    }

}
