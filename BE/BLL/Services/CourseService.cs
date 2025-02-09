using BLL.Interface;
using Common.DTO;
using DAL.Entities;
using DAL.UnitOfWork;
using DTO.DTO;
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

                    exercise.Content  =  exerciseDto.Content;
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
        }
    }
}
       
    
