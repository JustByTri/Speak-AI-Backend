using BLL.Interface;
using Common.DTO;
using DAL.Entities;
using DAL.UnitOfWork;
using DTO.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    namespace BLL.Services
    {
        public class CourseService : ICourseService
        {
            private readonly IUnitOfWork _unitOfWork;

            public CourseService(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<ResponseDTO> CreateCourseWithTopicsAndExercisesAsync(CreateCourseDTO courseDto)
            {
                try
                {

                    var totalTopics = courseDto.Topics.Count;
                    var pointPerTopic = Math.Round(courseDto.MaxPoint / totalTopics, 2);

                    var course = new Course
                    {
                        Id = Guid.NewGuid(),
                        CourseName = courseDto.CourseName,
                        Description = courseDto.Description,
                        MaxPoint = courseDto.MaxPoint,
                        IsFree = courseDto.IsFree,
                        LevelId = courseDto.LevelId,
                        IsDeleted = false,
                        IsActive = true,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _unitOfWork.Course.AddAsync(course);

                    foreach (var topicDto in courseDto.Topics)
                    {
                        var totalExercises = topicDto.Exercises.Count;
                        var pointPerExercise = Math.Round(pointPerTopic / totalExercises, 2);

                        var topic = new Topic
                        {
                            Id = Guid.NewGuid(),
                            TopicName = topicDto.TopicName,
                            MaxPoint = pointPerTopic,
                            IsDeleted = false,
                            IsActive = true,
                            UpdatedAt = DateTime.UtcNow,
                            CreatedAt = DateTime.UtcNow,
                            CourseId = course.Id
                        };

                        await _unitOfWork.Topic.AddAsync(topic);

                        foreach (var exerciseDto in topicDto.Exercises)
                        {
                            var exercise = new Exercise
                            {
                                Id = Guid.NewGuid(),
                                Content = exerciseDto.Content,
                                MaxPoint = pointPerExercise,
                                IsDeleted = false,
                                IsActive = true,
                                UpdatedAt = DateTime.UtcNow,
                                CreatedAt = DateTime.UtcNow,
                                TopicId = topic.Id
                            };

                            await _unitOfWork.Exercise.AddAsync(exercise);
                        }
                    }

                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseDTO("Create course successfully", 201, true);
                }
                catch (Exception ex)
                {
                    return new ResponseDTO($"Error: {ex.Message}", 500, false);
                }
            }
            public async Task<ResponseDTO> GetCourseByIdAsync(Guid courseId)
            {
                try
                {
                    var course = await _unitOfWork.Course.GetByIdAsync(courseId);
                    if (course == null)
                        return new ResponseDTO("Not Found ", 404, false);

                    return new ResponseDTO("Get Course Successfully", 200, true, course);
                }
                catch (Exception ex)
                {
                    return new ResponseDTO($"Error: {ex.Message}", 500, false);
                }
            }

            public async Task<ResponseDTO> UpdateCourseAsync(Guid courseId, UpdateCourseDTO courseDto)
            {
                try
                {
                    var course = await _unitOfWork.Course.GetByIdAsync(courseId);
                    if (course == null)
                        return new ResponseDTO("Not found course", 404, false);

                    course.CourseName = courseDto.CourseName;
                    course.Description = courseDto.Description;
                    course.MaxPoint = courseDto.MaxPoint;
                    course.IsFree = courseDto.IsFree;
                    course.LevelId = courseDto.LevelId;
                    course.UpdatedAt = DateTime.UtcNow;

                    await _unitOfWork.Course.UpdateAsync(course);
                    await _unitOfWork.SaveChangeAsync();

                    return new ResponseDTO("Update course successfully", 200, true);
                }
                catch (Exception ex)
                {
                    return new ResponseDTO($"Error: {ex.Message}", 500, false);
                }
            }

            public async Task<ResponseDTO> DeleteCourseAsync(Guid courseId)
            {
                try
                {
                    var course = await _unitOfWork.Course.GetByIdAsync(courseId);
                    if (course == null)
                        return new ResponseDTO("Not found course", 404, false);

                    course.IsDeleted = true;
                    course.UpdatedAt = DateTime.UtcNow;

                    await _unitOfWork.Course.UpdateAsync(course);
                    await _unitOfWork.SaveChangeAsync();

                    return new ResponseDTO("Delete successfully", 200, true);
                }
                catch (Exception ex)
                {
                    return new ResponseDTO($"Error: {ex.Message}", 500, false);
                }
            }

            // Topic operations
            public async Task<ResponseDTO> GetTopicByIdAsync(Guid topicId)
            {
                try
                {
                    var topic = await _unitOfWork.Topic.GetByIdAsync(topicId);
                    if (topic == null)
                        return new ResponseDTO("Not found the topic", 404, false);

                    return new ResponseDTO("Get topic successfully", 200, true, topic);
                }
                catch (Exception ex)
                {
                    return new ResponseDTO($"Error: {ex.Message}", 500, false);
                }
            }

            public async Task<ResponseDTO> AddTopicToCourseAsync(Guid courseId, CreateTopicDTO topicDto)
            {
                try
                {
                    var course = await _unitOfWork.Course.GetByIdAsync(courseId);
                    if (course == null)
                        return new ResponseDTO("Not found the course", 404, false);

                    var topic = new Topic
                    {
                        Id = Guid.NewGuid(),
                        TopicName = topicDto.TopicName,
                        CourseId = courseId,
                        IsDeleted = false,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _unitOfWork.Topic.AddAsync(topic);
                    await _unitOfWork.SaveChangeAsync();

                    return new ResponseDTO("Add successfully", 201, true);
                }
                catch (Exception ex)
                {
                    return new ResponseDTO($"Error: {ex.Message}", 500, false);
                }
            }

            public async Task<ResponseDTO> UpdateTopicAsync(Guid topicId, UpdateTopicDTO topicDto)
            {
                try
                {
                    var topic = await _unitOfWork.Topic.GetByIdAsync(topicId);
                    if (topic == null)
                        return new ResponseDTO("Not found", 404, false);

                    topic.TopicName = topicDto.TopicName;
                    topic.IsActive = topicDto.IsActive;
                    topic.IsDeleted = topicDto.IsDeleted;
                    topic.UpdatedAt = DateTime.UtcNow;

                    await _unitOfWork.Topic.UpdateAsync(topic);
                    await _unitOfWork.SaveChangeAsync();

                    return new ResponseDTO("Update topic successfully", 200, true);
                }
                catch (Exception ex)
                {
                    return new ResponseDTO($"Error: {ex.Message}", 500, false);
                }
            }

            public async Task<ResponseDTO> DeleteTopicAsync(Guid topicId)
            {
                try
                {
                    var topic = await _unitOfWork.Topic.GetByIdAsync(topicId);
                    if (topic == null)
                        return new ResponseDTO("Not found", 404, false);

                    topic.IsDeleted = true;
                    topic.UpdatedAt = DateTime.UtcNow;

                    await _unitOfWork.Topic.UpdateAsync(topic);
                    await _unitOfWork.SaveChangeAsync();

                    return new ResponseDTO("Delete topic successfully", 200, true);
                }
                catch (Exception ex)
                {
                    return new ResponseDTO($"Error: {ex.Message}", 500, false);
                }
            }

            public async Task<ResponseDTO> GetExerciseByIdAsync(Guid exerciseId)
            {
                try
                {
                    var exercise = await _unitOfWork.Exercise.GetByIdAsync(exerciseId);
                    if (exercise == null || exercise.IsDeleted)
                        return new ResponseDTO("Not found", 404, false);

                    return new ResponseDTO("Get exercise successfully", 200, true, exercise);
                }
                catch (Exception ex)
                {
                    return new ResponseDTO($"Error: {ex.Message}", 500, false);
                }
            }

            public async Task<ResponseDTO> AddExerciseToTopicAsync(Guid topicId, CreateExerciseDTO exerciseDto)
            {
                try
                {
                    var topic = await _unitOfWork.Topic.GetByIdAsync(topicId);
                    if (topic == null || topic.IsDeleted)
                        return new ResponseDTO("Not found", 404, false);

                    var exercise = new Exercise
                    {
                        Id = Guid.NewGuid(),
                        Content = exerciseDto.Content,
                        TopicId = topicId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    };

                    await _unitOfWork.Exercise.AddAsync(exercise);
                    await _unitOfWork.SaveChangeAsync();

                    return new ResponseDTO("Add exercise successfully", 201, true);
                }
                catch (Exception ex)
                {
                    return new ResponseDTO($"Error: {ex.Message}", 500, false);
                }
            }

            public async Task<ResponseDTO> UpdateExerciseAsync(Guid exerciseId, UpdateExerciseDTO exerciseDto)
            {
                try
                {
                    var exercise = await _unitOfWork.Exercise.GetByIdAsync(exerciseId);
                    if (exercise == null || exercise.IsDeleted)
                        return new ResponseDTO("Not found", 404, false);

                    exercise.Content = exerciseDto.Content;
                    exercise.UpdatedAt = DateTime.UtcNow;

                    await _unitOfWork.Exercise.UpdateAsync(exercise);
                    await _unitOfWork.SaveChangeAsync();

                    return new ResponseDTO("Update exercise successfully", 200, true);
                }
                catch (Exception ex)
                {
                    return new ResponseDTO($"Error: {ex.Message}", 500, false);
                }
            }

            public async Task<ResponseDTO> DeleteExerciseAsync(Guid exerciseId)
            {
                try
                {
                    var exercise = await _unitOfWork.Exercise.GetByIdAsync(exerciseId);
                    if (exercise == null || exercise.IsDeleted)
                        return new ResponseDTO("Not found", 404, false);

                    exercise.IsDeleted = true;
                    exercise.UpdatedAt = DateTime.UtcNow;

                    await _unitOfWork.Exercise.UpdateAsync(exercise);
                    await _unitOfWork.SaveChangeAsync();

                    return new ResponseDTO("Delete exercise sucessfully", 200, true);
                }
                catch (Exception ex)
                {
                    return new ResponseDTO($"Error: {ex.Message}", 500, false);
                }
            }

            public async Task<ResponseDTO> EnrollCourseAsync(Guid userId, Guid courseId)
            {
                try
                {
                    var user = await _unitOfWork.User.GetByIdAsync(userId);
                    var courseResponse = await GetCourseByIdAsync(courseId);

                    if (user == null || !courseResponse.IsSuccess)
                        return new ResponseDTO("User or course not found", 404, false);

                    var enrolledCourse = new EnrolledCourse
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId,
                        CourseId = courseId,
                        IsCompleted = false,
                        ProgressPoints = 0,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _unitOfWork.EnrolledCourse.AddAsync(enrolledCourse);


                    var topics = await _unitOfWork.Topic.GetAllByListAsync(t => t.CourseId == courseId && !t.IsDeleted);
                    foreach (var topic in topics)
                    {
                        var topicResponse = await GetTopicByIdAsync(topic.Id);
                        if (!topicResponse.IsSuccess) continue;

                        var topicProgress = new TopicProgress
                        {
                            Id = Guid.NewGuid(),
                            EnrolledCourseId = enrolledCourse.Id,
                            TopicId = topic.Id,
                            UserId = userId,
                            ProgressPoints = 0,
                            IsCompleted = false,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        await _unitOfWork.TopicProgress.AddAsync(topicProgress);


                        var exercises = await _unitOfWork.Exercise.GetAllByListAsync(e => e.TopicId == topic.Id && !e.IsDeleted);
                        foreach (var exercise in exercises)
                        {
                            var exerciseResponse = await GetExerciseByIdAsync(exercise.Id);
                            if (!exerciseResponse.IsSuccess) continue;

                            var exerciseProgress = new ExerciseProgress
                            {
                                Id = Guid.NewGuid(),
                                EnrolledCourseId = enrolledCourse.Id,
                                ExerciseId = exercise.Id,
                                UserId = userId,
                                ProgressPoints = 0,
                                IsCompleted = false,
                                CreatedAt = DateTime.UtcNow
                            };
                            await _unitOfWork.ExerciseProgress.AddAsync(exerciseProgress);
                        }
                    }

                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseDTO("Enrolled successfully", 201, true);
                }
                catch (Exception ex)
                {
                    return new ResponseDTO($"Error: {ex.Message}", 500, false);
                }
            }
            public async Task<ResponseDTO> GetEnrolledCourseDetailsAsync(Guid enrolledCourseId)
            {
                try
                {
                    var enrolledCourse = await _unitOfWork.EnrolledCourse.GetByIdAsync(enrolledCourseId);
                    if (enrolledCourse == null)
                        return new ResponseDTO("Not found", 404, false);

                    var course = await _unitOfWork.Course.GetByIdAsync(enrolledCourse.CourseId);
                    var topicsProgress = await _unitOfWork.TopicProgress.GetByEnrolledCourseAsync(enrolledCourseId);

                    var enrolledCourseDetails = new EnrolledCourseDetailsDTO
                    {
                        Course = new CourseDTO
                        {
                            Id = course.Id,
                            CourseName = course.CourseName,
                            Description = course.Description,
                            MaxPoint = course.MaxPoint,
                            IsFree = course.IsFree,
                            IsActive = course.IsActive,
                            LevelId = course.LevelId
                        },
                        Progress = enrolledCourse.ProgressPoints,
                        Topics = topicsProgress.Select(tp => new TopicProgressDTO
                        {
                            Id = tp.TopicId,
                            Progress = tp.ProgressPoints
                        }).ToList()
                    };

                    return new ResponseDTO("Success", 200, true, enrolledCourseDetails);
                }
                catch (Exception ex)
                {
                    return new ResponseDTO($"Error: {ex.Message}", 500, false);
                }
            }

            public async Task<ResponseDTO> SubmitExerciseAsync(Guid exerciseId, Guid userId, decimal earnedPoints)
            {
                try
                {
                    var exercise = await _unitOfWork.Exercise.GetByIdAsync(exerciseId);
                    var exerciseProgress = await _unitOfWork.ExerciseProgress.GetByUserAndExerciseAsync(userId, exerciseId);
                    if (exercise == null || exerciseProgress == null)
                        return new ResponseDTO("Not found", 404, false);


                    exerciseProgress.ProgressPoints += earnedPoints;
                    exerciseProgress.IsCompleted = (exerciseProgress.ProgressPoints >= exercise.MaxPoint);
                    await _unitOfWork.ExerciseProgress.UpdateAsync(exerciseProgress);


                    var allExerciseProgressInTopic = await _unitOfWork.ExerciseProgress.GetByUserAndTopicAsyncz(userId, exercise.TopicId);


                    var totalPoints = allExerciseProgressInTopic.Sum(ep => ep.ProgressPoints);


                    var topicProgress = await _unitOfWork.TopicProgress.GetByUserAndTopicAsync(userId, exercise.TopicId);
                    var topic = await _unitOfWork.Topic.GetByIdAsync(exercise.TopicId);

                    topicProgress.ProgressPoints = totalPoints;
                    topicProgress.IsCompleted = (totalPoints >= topic.MaxPoint);
                    await _unitOfWork.TopicProgress.UpdateAsync(topicProgress);

                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseDTO("Updated successfully", 200, true);
                }
                catch (Exception ex)
                {
                    return new ResponseDTO($"Error: {ex.Message}", 500, true);
                }
            }


            public async Task<ResponseDTO> GetAllCoursesAsync()
            {
                try
                {
                    var listCourse = await _unitOfWork.Course.GetAllByListAsync(c => true && c.IsDeleted == false);
                    return new ResponseDTO("Success", 200, true, listCourse);

                }
                catch (Exception ex)
                {
                    return new ResponseDTO($"Error: {ex.Message}", 500, false);
                }
            }

            public async Task<IEnumerable<Course>> GetAllCourses(string search = "")
            {
                var query = _unitOfWork.Course.GetAll()
                    .Where(c => !c.IsDeleted);

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(c =>
                        c.CourseName.Contains(search) ||
                        c.Description.Contains(search)
                    );
                }

                return await query.ToListAsync();
            }

            public async Task<IEnumerable<Course>> SearchCourses(string keyword)
            {
                return await _unitOfWork.Course
                    .FindAll(c =>
                        c.CourseName.Contains(keyword) ||
                        c.Description.Contains(keyword))
                    .ToListAsync();
            }

        }
    }
}

