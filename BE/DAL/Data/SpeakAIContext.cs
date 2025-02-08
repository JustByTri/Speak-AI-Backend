using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class SpeakAIContext : DbContext
    {
        public SpeakAIContext(DbContextOptions<SpeakAIContext> options)
            : base(options)
        {
        }

        public DbSet<Level> Levels { get; set; }
        public DbSet<UserLevel> UserLevels { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<EnrolledCourse> EnrolledCourses { get; set; }
        public DbSet<TopicProgress> TopicProgresses { get; set; }
        public DbSet<ExerciseProgress> ExerciseProgresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure primary keys
            modelBuilder.Entity<Level>().HasKey(l => l.LevelId);
            modelBuilder.Entity<UserLevel>().HasKey(ul => ul.UserLevelId);
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<Course>().HasKey(c => c.CourseId);
            modelBuilder.Entity<Topic>().HasKey(t => t.TopicId);
            modelBuilder.Entity<Exercise>().HasKey(e => e.ExerciseId);
            modelBuilder.Entity<EnrolledCourse>().HasKey(ec => ec.EnrolledCourseId);
            modelBuilder.Entity<TopicProgress>().HasKey(tp => tp.TopicProgressId);
            modelBuilder.Entity<ExerciseProgress>().HasKey(ep => ep.ExerciseProgressId);

            modelBuilder.Entity<Course>()
      .Property(c => c.MaxPoint)
      .HasPrecision(18, 2);

            modelBuilder.Entity<EnrolledCourse>()
                .Property(ec => ec.ProgressPoints)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Exercise>()
                .Property(e => e.MaxPoint)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ExerciseProgress>()
                .Property(ep => ep.ProgressPoints)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Level>()
                .Property(l => l.MaxPoint)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Level>()
                .Property(l => l.MinPoint)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Topic>()
                .Property(t => t.MaxPoint)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TopicProgress>()
                .Property(tp => tp.ProgressPoints)
                .HasPrecision(18, 2);

            modelBuilder.Entity<UserLevel>()
                .Property(ul => ul.Point)
                .HasPrecision(18, 2);
            modelBuilder.Entity<EnrolledCourse>()
       .HasOne(ec => ec.User)
       .WithMany(u => u.EnrolledCourses)
       .HasForeignKey(ec => ec.UserId)
       .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TopicProgress>()
                .HasOne(tp => tp.User)
                .WithMany(u => u.TopicProgresses)
                .HasForeignKey(tp => tp.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ExerciseProgress>()
                .HasOne(ep => ep.User)
                .WithMany(u => u.ExerciseProgresses)
                .HasForeignKey(ep => ep.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure relationships for EnrolledCourse
            modelBuilder.Entity<TopicProgress>()
                .HasOne(tp => tp.EnrolledCourse)
                .WithMany(ec => ec.TopicProgresses)
                .HasForeignKey(tp => tp.EnrolledCourseId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ExerciseProgress>()
                .HasOne(ep => ep.EnrolledCourse)
                .WithMany()
                .HasForeignKey(ep => ep.EnrolledCourseId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
