﻿using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        [NotNull]
        public DbSet<ExerciseProgress> ExerciseProgresses { get; set; } 

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure primary keys
            modelBuilder.Entity<Course>().HasKey(c => c.Id);
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<EnrolledCourse>().HasKey(u => u.Id);
            modelBuilder.Entity<ExerciseProgress>().HasKey(u => u.Id);  
            modelBuilder.Entity<Exercise>().HasKey(u => u.Id);
            modelBuilder.Entity<Topic>().HasKey(u => u.Id);
            modelBuilder.Entity<UserLevel>().HasKey(u => u.Id); 
            modelBuilder.Entity<TopicProgress>().HasKey(u => u.Id);
            modelBuilder.Entity<RefreshToken>().HasKey(u => u.Id);
  


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
            // Cấu hình User và UserLevel
    
            // Cấu hình Course và Level
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Level)
                .WithMany(l => l.Courses)
                .HasForeignKey(c => c.LevelId)
                .OnDelete(DeleteBehavior.NoAction);

            // Cấu hình Topic và Course
            modelBuilder.Entity<Topic>()
                .HasOne(t => t.Course)
                .WithMany(c => c.Topics)
                .HasForeignKey(t => t.CourseId)
                .OnDelete(DeleteBehavior.NoAction);

            // Cấu hình Exercise và Topic
            modelBuilder.Entity<Exercise>()
                .HasOne(e => e.Topic)
                .WithMany(t => t.Exercises)
                .HasForeignKey(e => e.TopicId)
                .OnDelete(DeleteBehavior.NoAction);

            // Cấu hình EnrolledCourse với User và Course
            modelBuilder.Entity<EnrolledCourse>()
                .HasOne(ec => ec.User)
                .WithMany(u => u.EnrolledCourses)
                .HasForeignKey(ec => ec.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EnrolledCourse>()
                .HasOne(ec => ec.Course)
                .WithMany(c => c.EnrolledCourses)
                .HasForeignKey(ec => ec.CourseId)
                .OnDelete(DeleteBehavior.NoAction);

            // Cấu hình TopicProgress
            modelBuilder.Entity<TopicProgress>()
                .HasOne(tp => tp.User)
                .WithMany(u => u.TopicProgresses)
                .HasForeignKey(tp => tp.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TopicProgress>()
                .HasOne(tp => tp.Topic)
                .WithMany(t => t.TopicProgresses)
                .HasForeignKey(tp => tp.TopicId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TopicProgress>()
                .HasOne(tp => tp.EnrolledCourse)
                .WithMany(ec => ec.TopicProgresses)
                .HasForeignKey(tp => tp.EnrolledCourseId)
                .OnDelete(DeleteBehavior.NoAction);

            // Cấu hình ExerciseProgress
            modelBuilder.Entity<ExerciseProgress>()
                .HasOne(ep => ep.User)
                .WithMany(u => u.ExerciseProgresses)
                .HasForeignKey(ep => ep.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ExerciseProgress>()
                .HasOne(ep => ep.Exercise)
                .WithMany(e => e.ExerciseProgresses)
                .HasForeignKey(ep => ep.ExerciseId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ExerciseProgress>()
                .HasOne(ep => ep.EnrolledCourse)
                .WithMany()
                .HasForeignKey(ep => ep.EnrolledCourseId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Level>().HasData(
     new Level
     {
         LevelId = 1,
         LevelName = "A0,A1",
         MinPoint = 0,
         MaxPoint = 100,
    
     },
     new Level
     {
         LevelId = 2,
         LevelName = "B1,B2",
         MinPoint = 101,
         MaxPoint = 200,
   
     },
     new Level
     {
         LevelId = 3,
         LevelName = "C1,B2",
         MinPoint = 201,
         MaxPoint = 300,
     
     }
 );
        }
    }
}
