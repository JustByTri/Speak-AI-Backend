using BLL.Interface;
using Common.DTO;
using Common.Enum;
using DAL.Entities;
using DAL.UnitOfWorks;
using DTO.DTO;
using Microsoft.EntityFrameworkCore;

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
                // Validate MaxPoint
                if (courseDto.MaxPoint % 1 != 0)
                {
                    return new ResponseDTO("Total course score must be an integer", StatusCodeEnum.BadRequest, false);
                }
                int maxPoint = (int)courseDto.MaxPoint;

                // Validate topic count and divisibility
                int totalTopics = courseDto.Topics.Count;
                if (totalTopics == 0 || maxPoint % totalTopics != 0)
                {
                    return new ResponseDTO(
                        $"The number of topics must be a divisor of {maxPoint}",
                        StatusCodeEnum.BadRequest,
                        false
                    );
                }
                int pointPerTopic = maxPoint / totalTopics;

                var course = new Course
                {
                    Id = Guid.NewGuid(),
                    CourseName = courseDto.CourseName,
                    Description = courseDto.Description,
                    MaxPoint = maxPoint,
                    IsPremium = courseDto.IsPremium,
                    LevelId = courseDto.LevelId,
                    IsDeleted = false,
                    IsActive = true,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    IsLock = false,
                    DisplayOrder = 0
                };
                await _unitOfWork.Course.AddAsync(course);

                // Process each Topic
                foreach (var topicDto in courseDto.Topics)
                {
                    // Validate exercise count and divisibility
                    int totalExercises = topicDto.Exercises.Count;
                    if (totalExercises == 0 || pointPerTopic % totalExercises != 0)
                    {
                        return new ResponseDTO(
                            $"Number of exercises in the topic '{topicDto.TopicName}' must be a divisor of {pointPerTopic}",
                            StatusCodeEnum.BadRequest,
                            false
                        );
                    }
                    int pointPerExercise = pointPerTopic / totalExercises;

                    // Create the Topic
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

                    // Process each Exercise
                    foreach (var exerciseDto in topicDto.Exercises)
                    {
                        // Validate TypeId
                        var type = await _unitOfWork.Types.GetByIdAsyncc(exerciseDto.TypeId);
                        if (type == null)
                        {
                            return new ResponseDTO(
                                $"Invalid TypeId {exerciseDto.TypeId} for exercise in topic '{topicDto.TopicName}'",
                                StatusCodeEnum.BadRequest,
                                false
                            );
                        }

                        // Validate questions
                        if (exerciseDto.Questions == null || exerciseDto.Questions.Count == 0)
                        {
                            return new ResponseDTO(
                                $"Exercise in topic '{topicDto.TopicName}' must have at least one question",
                                StatusCodeEnum.BadRequest,
                                false
                            );
                        }

                        // Create the Exercise
                        var exercise = new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Content = exerciseDto.Content,
                            MaxPoint = pointPerExercise,
                            TypeId = exerciseDto.TypeId,
                            IsDeleted = false,
                            IsActive = true,

                            CreatedAt = DateTime.UtcNow,
                            TopicId = topic.Id
                        };
                        await _unitOfWork.Exercise.AddAsync(exercise);

                        // Process each Exercise Question
                        foreach (var questionDto in exerciseDto.Questions)
                        {
                            // Validate answers
                            if (questionDto.Answers == null || questionDto.Answers.Count == 0)
                            {
                                return new ResponseDTO(
                                    $"Question in exercise '{exerciseDto.Content}' must have at least one answer",
                                    StatusCodeEnum.BadRequest,
                                    false
                                );
                            }

                            // Validate that there is at least one correct answer
                            if (!questionDto.Answers.Any(a => a.IsCorrect))
                            {
                                return new ResponseDTO(
                                    $"Question in exercise '{exerciseDto.Content}' must have at least one correct answer",
                                    StatusCodeEnum.BadRequest,
                                    false
                                );
                            }

                            // Create the Exercise Question
                            var exerciseQuestion = new ExerciseQuestion
                            {
                                Id = Guid.NewGuid(),
                                ExerciseId = exercise.Id,
                                TypeId = exerciseDto.TypeId, // Use the same TypeId as the exercise
                                Content = questionDto.Content,
                                UpdatedAt = DateTime.UtcNow,
                                CreatedAt = DateTime.UtcNow
                            };
                            await _unitOfWork.ExerciseQuestion.AddAsync(exerciseQuestion);

                            // Process each Exercise Answer
                            foreach (var answerDto in questionDto.Answers)
                            {
                                var exerciseAnswer = new ExerciseAnswer
                                {
                                    Id = Guid.NewGuid(),
                                    ExerciseQuestionId = exerciseQuestion.Id,
                                    Content = answerDto.Content,
                                    IsCorrect = answerDto.IsCorrect,
                                    UpdatedAt = DateTime.UtcNow,
                                    CreatedAt = DateTime.UtcNow
                                };
                                await _unitOfWork.ExerciseAnswer.AddAsync(exerciseAnswer);
                            }
                        }
                    }
                }

                await _unitOfWork.SaveChangeAsync();
                return new ResponseDTO("Create a successful course", StatusCodeEnum.Created, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", StatusCodeEnum.InteralServerError, false);
            }
        }

        public async Task<ResponseDTO> GetCourseByIdAsync(Guid courseId)
        {
            try
            {
                var course = await _unitOfWork.Course.GetByIdAsync(courseId);
                if (course == null || course.IsDeleted)
                    return new ResponseDTO("Course not found", StatusCodeEnum.NotFound, false);

                return new ResponseDTO("Get Course Successfully", StatusCodeEnum.OK, true, course);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", StatusCodeEnum.InteralServerError, false);
            }
        }

        public async Task<ResponseDTO> UpdateCourseAsync(Guid courseId, UpdateCourseDTO courseDto)
        {
            try
            {
                var course = await _unitOfWork.Course.GetByIdAsync(courseId);
                if (course == null || course.IsDeleted)
                    return new ResponseDTO("Course not found", StatusCodeEnum.NotFound, false);

                course.CourseName = courseDto.CourseName;
                course.Description = courseDto.Description;
                course.MaxPoint = courseDto.MaxPoint;
                course.LevelId = courseDto.LevelId;
                course.IsPremium = courseDto.IsPremium;
                course.IsActive = courseDto.IsActive;
                course.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Course.UpdateAsync(course);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO("Update course successfully", StatusCodeEnum.OK, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", StatusCodeEnum.InteralServerError, false);
            }
        }

        public async Task<ResponseDTO> DeleteCourseAsync(Guid courseId)
        {
            try
            {
                var course = await _unitOfWork.Course.GetByIdAsync(courseId);
                if (course == null || course.IsDeleted)
                    return new ResponseDTO("Course not found", StatusCodeEnum.NotFound, false);

                course.IsDeleted = true;
                course.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Course.UpdateAsync(course);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO("Delete successfully", StatusCodeEnum.OK, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", StatusCodeEnum.InteralServerError, false);
            }
        }

        public async Task<ResponseDTO> GetTopicByIdAsync(Guid topicId)
        {
            try
            {
                var topic = await _unitOfWork.Topic.GetByIdAsync(topicId);
                if (topic == null || topic.IsDeleted)
                    return new ResponseDTO("Topic not found", StatusCodeEnum.NotFound, false);

                return new ResponseDTO("Get topic successfully", StatusCodeEnum.OK, true, topic);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", StatusCodeEnum.InteralServerError, false);
            }
        }

        public async Task<ResponseDTO> AddTopicToCourseAsync(Guid courseId, CreateTopicDTO topicDto)
        {
            try
            {
                var course = await _unitOfWork.Course.GetByIdAsync(courseId);
                if (course == null || course.IsDeleted)
                    return new ResponseDTO("Course not found", StatusCodeEnum.NotFound, false);

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

                return new ResponseDTO("Add topic successfully", StatusCodeEnum.Created, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", StatusCodeEnum.InteralServerError, false);
            }
        }

        public async Task<ResponseDTO> UpdateTopicAsync(Guid topicId, UpdateTopicDTO topicDto)
        {
            try
            {
                var topic = await _unitOfWork.Topic.GetByIdAsync(topicId);
                if (topic == null || topic.IsDeleted)
                    return new ResponseDTO("Topic not found", StatusCodeEnum.NotFound, false);

                topic.TopicName = topicDto.TopicName;
                topic.IsActive = topicDto.IsActive;
                topic.IsDeleted = topicDto.IsDeleted;
                topic.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Topic.UpdateAsync(topic);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO("Update topic successfully", StatusCodeEnum.OK, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", StatusCodeEnum.InteralServerError, false);
            }
        }

        public async Task<ResponseDTO> DeleteTopicAsync(Guid topicId)
        {
            try
            {
                var topic = await _unitOfWork.Topic.GetByIdAsync(topicId);
                if (topic == null || topic.IsDeleted)
                    return new ResponseDTO("Topic not found", StatusCodeEnum.NotFound, false);

                topic.IsDeleted = true;
                topic.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Topic.UpdateAsync(topic);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO("Delete topic successfully", StatusCodeEnum.OK, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", StatusCodeEnum.InteralServerError, false);
            }
        }

        public async Task<ResponseDTO> GetExerciseByIdAsync(Guid exerciseId)
        {
            try
            {
                var exercise = await _unitOfWork.Exercise.GetByIdAsync(exerciseId);
                if (exercise == null || exercise.IsDeleted)
                    return new ResponseDTO("Exercise not found", StatusCodeEnum.NotFound, false);

                return new ResponseDTO("Get exercise successfully", StatusCodeEnum.OK, true, exercise);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", StatusCodeEnum.InteralServerError, false);
            }
        }

        public async Task<ResponseDTO> AddExerciseToTopicAsync(Guid topicId, CreateExerciseDTO exerciseDto)
        {
            try
            {
                var topic = await _unitOfWork.Topic.GetByIdAsync(topicId);
                if (topic == null || topic.IsDeleted)
                    return new ResponseDTO("Topic not found", StatusCodeEnum.NotFound, false);

                // Validate TypeId
                var type = await _unitOfWork.Types.GetByIdAsyncc(exerciseDto.TypeId);
                if (type == null)
                {
                    return new ResponseDTO(
                        $"Invalid TypeId {exerciseDto.TypeId} for exercise",
                        StatusCodeEnum.BadRequest,
                        false
                    );
                }

                // Validate questions
                if (exerciseDto.Questions == null || exerciseDto.Questions.Count == 0)
                {
                    return new ResponseDTO(
                        "Exercise must have at least one question",
                        StatusCodeEnum.BadRequest,
                        false
                    );
                }

                var exercise = new Exercise
                {
                    Id = Guid.NewGuid(),
                    Content = exerciseDto.Content,
                    TypeId = exerciseDto.TypeId,
                    TopicId = topicId,
                    IsDeleted = false,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,

                };

                await _unitOfWork.Exercise.AddAsync(exercise);

                // Process each Exercise Question
                foreach (var questionDto in exerciseDto.Questions)
                {
                    // Validate answers
                    if (questionDto.Answers == null || questionDto.Answers.Count == 0)
                    {
                        return new ResponseDTO(
                            $"Question in exercise '{exerciseDto.Content}' must have at least one answer",
                            StatusCodeEnum.BadRequest,
                            false
                        );
                    }

                    // Validate that there is at least one correct answer
                    if (!questionDto.Answers.Any(a => a.IsCorrect))
                    {
                        return new ResponseDTO(
                            $"Question in exercise '{exerciseDto.Content}' must have at least one correct answer",
                            StatusCodeEnum.BadRequest,
                            false
                        );
                    }

                    // Create the Exercise Question
                    var exerciseQuestion = new ExerciseQuestion
                    {
                        Id = Guid.NewGuid(),
                        ExerciseId = exercise.Id,
                        TypeId = exerciseDto.TypeId,
                        Content = questionDto.Content,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _unitOfWork.ExerciseQuestion.AddAsync(exerciseQuestion);

                    // Process each Exercise Answer
                    foreach (var answerDto in questionDto.Answers)
                    {
                        var exerciseAnswer = new ExerciseAnswer
                        {
                            Id = Guid.NewGuid(),
                            ExerciseQuestionId = exerciseQuestion.Id,
                            Content = answerDto.Content,
                            IsCorrect = answerDto.IsCorrect,
                            UpdatedAt = DateTime.UtcNow,
                            CreatedAt = DateTime.UtcNow
                        };
                        await _unitOfWork.ExerciseAnswer.AddAsync(exerciseAnswer);
                    }
                }

                await _unitOfWork.SaveChangeAsync();
                return new ResponseDTO("Add exercise successfully", StatusCodeEnum.Created, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", StatusCodeEnum.InteralServerError, false);
            }
        }

        public async Task<ResponseDTO> UpdateExerciseAsync(Guid exerciseId, UpdateExerciseDTO exerciseDto)
        {
            try
            {
                var exercise = await _unitOfWork.Exercise.GetByIdAsync(exerciseId);
                if (exercise == null || exercise.IsDeleted)
                    return new ResponseDTO("Exercise not found", StatusCodeEnum.NotFound, false);

                var type = await _unitOfWork.Types.GetByIdAsyncc(exerciseDto.TypeId);
                if (type == null)
                {
                    return new ResponseDTO(
                        $"Invalid TypeId {exerciseDto.TypeId} for exercise",
                        StatusCodeEnum.BadRequest,
                        false
                    );
                }

                exercise.Content = exerciseDto.Content;
                exercise.TypeId = exerciseDto.TypeId;


                await _unitOfWork.Exercise.UpdateAsync(exercise);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO("Update exercise successfully", StatusCodeEnum.OK, true);
            }
            catch (Exception ex)

            {
                return new ResponseDTO($"Error: {ex.Message}", StatusCodeEnum.InteralServerError, false);
            }
        }

        public async Task<ResponseDTO> DeleteExerciseAsync(Guid exerciseId)
        {
            try
            {
                var exercise = await _unitOfWork.Exercise.GetByIdAsync(exerciseId);
                if (exercise == null || exercise.IsDeleted)
                    return new ResponseDTO("Exercise not found", StatusCodeEnum.NotFound, false);

                exercise.IsDeleted = true;


                await _unitOfWork.Exercise.UpdateAsync(exercise);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO("Delete exercise successfully", StatusCodeEnum.OK, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", StatusCodeEnum.InteralServerError, false);
            }
        }

        public async Task<ResponseDTO> EnrollCourseAsync(Guid userId, Guid courseId)
        {
            try
            {
                var user = await _unitOfWork.User.GetByIdAsync(userId);
                var course = await _unitOfWork.Course.GetByIdAsync(courseId);
                if (user == null || course == null || course.IsDeleted)
                    return new ResponseDTO("User or course not found", StatusCodeEnum.NotFound, false);

                var existingEnrollment = await _unitOfWork.EnrolledCourse
                    .GetByConditionAsync(e => e.UserId == userId && e.CourseId == courseId);

                if (existingEnrollment != null)
                    return new ResponseDTO("User has already enrolled in this course", StatusCodeEnum.BadRequest, false);

                if (course.IsPremium && (!user.IsPremium || (user.PremiumExpiredTime != null && user.PremiumExpiredTime < DateTime.UtcNow)))
                    return new ResponseDTO("Only premium users can enroll in premium courses", StatusCodeEnum.Forbidden, false);

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
                        var exerciseProgress = new ExerciseProgress
                        {
                            Id = Guid.NewGuid(),
                            EnrolledCourseId = enrolledCourse.Id,
                            ExerciseId = exercise.Id,
                            UserId = userId,
                            ProgressPoints = 0,
                            IsCompleted = false,
                            CreatedAt = DateTime.UtcNow,

                        };
                        await _unitOfWork.ExerciseProgress.AddAsync(exerciseProgress);
                    }
                }

                await _unitOfWork.SaveChangeAsync();
                return new ResponseDTO("Enrolled successfully", StatusCodeEnum.Created, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", StatusCodeEnum.InteralServerError, false);
            }
        }

        public async Task<ResponseDTO> GetEnrolledCourseDetailsAsync(Guid enrolledCourseId)
        {
            try
            {
                var enrolledCourse = await _unitOfWork.EnrolledCourse.GetByIdAsync(enrolledCourseId);
                if (enrolledCourse == null)
                    return new ResponseDTO("Enrolled course not found", StatusCodeEnum.NotFound, false);

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

                return new ResponseDTO("Success", StatusCodeEnum.OK, true, enrolledCourseDetails);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", StatusCodeEnum.InteralServerError, false);
            }
        }

        public async Task<ResponseDTO> SubmitExerciseAsync(Guid exerciseId, Guid userId, decimal earnedPoints)
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted))
            {
                try
                {
                    var exercise = await _unitOfWork.Exercise.GetByIdAsync(exerciseId);
                    var exerciseProgress = await _unitOfWork.ExerciseProgress.GetByUserAndExerciseAsync(userId, exerciseId);

                    if (exercise == null || exerciseProgress == null)
                        return new ResponseDTO("Exercise or progress not found", StatusCodeEnum.NotFound, false);

                    exerciseProgress.ProgressPoints = Math.Min(exercise.MaxPoint, earnedPoints);
                    exerciseProgress.IsCompleted = exerciseProgress.ProgressPoints >= exercise.MaxPoint;


                    await _unitOfWork.ExerciseProgress.UpdateAsync(exerciseProgress);

                    var topicExercises = await _unitOfWork.ExerciseProgress
                        .GetAllByListAsync(ep =>
                            ep.UserId == userId &&
                            ep.Exercise.TopicId == exercise.TopicId);

                    decimal totalTopicPoints = topicExercises.Sum(ep => ep.ProgressPoints);
                    var topic = await _unitOfWork.Topic.GetByIdAsync(exercise.TopicId);

                    var topicProgress = await _unitOfWork.TopicProgress
                        .GetByConditionAsync(tp =>
                            tp.UserId == userId &&
                            tp.TopicId == exercise.TopicId);

                    if (topicProgress != null)
                    {
                        topicProgress.ProgressPoints = totalTopicPoints;
                        topicProgress.IsCompleted = totalTopicPoints >= topic.MaxPoint;
                        topicProgress.UpdatedAt = DateTime.UtcNow;
                        await _unitOfWork.TopicProgress.UpdateAsync(topicProgress);
                    }

                    var enrolledCourse = await _unitOfWork.EnrolledCourse
                        .GetByConditionWithIncludesAsync(
                            ec => ec.UserId == userId && ec.CourseId == topic.CourseId,
                            includes: ec => ec.Course);

                    if (enrolledCourse != null)
                    {
                        var courseTopics = await _unitOfWork.Topic
                            .GetAllByListAsync(t =>
                                t.CourseId == enrolledCourse.CourseId &&
                                !t.IsDeleted);

                        bool allTopicsCompleted = true;
                        decimal totalCoursePoints = 0;

                        foreach (var courseTopic in courseTopics)
                        {
                            var tp = await _unitOfWork.TopicProgress
                                .GetByConditionAsync(t =>
                                    t.UserId == userId &&
                                    t.TopicId == courseTopic.Id);

                            if (tp == null || !tp.IsCompleted)
                            {
                                allTopicsCompleted = false;
                            }
                            totalCoursePoints += tp?.ProgressPoints ?? 0;
                        }

                        enrolledCourse.ProgressPoints = Math.Min(totalCoursePoints, enrolledCourse.Course.MaxPoint);
                        enrolledCourse.IsCompleted = allTopicsCompleted;
                        enrolledCourse.UpdatedAt = DateTime.UtcNow;

                        await _unitOfWork.EnrolledCourse.UpdateAsync(enrolledCourse);
                    }

                    var responseData = new SubmitExerciseResponseDTO
                    {
                        ExerciseId = exerciseId,
                        ExerciseEarnedPoints = exerciseProgress.ProgressPoints,
                        IsExerciseCompleted = exerciseProgress.IsCompleted,
                        TopicId = exercise.TopicId,
                        TopicTotalPoints = totalTopicPoints,
                        IsTopicCompleted = topicProgress?.IsCompleted ?? false,
                        CourseId = topic.CourseId,
                        CourseTotalPoints = enrolledCourse?.ProgressPoints ?? 0,
                        IsCourseCompleted = enrolledCourse?.IsCompleted ?? false
                    };

                    await _unitOfWork.SaveChangeAsync();
                    await transaction.CommitAsync();

                    return new ResponseDTO("Submit exercise successfully", StatusCodeEnum.OK, true, responseData);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ResponseDTO($"Error: {ex.Message}", StatusCodeEnum.InteralServerError, false);
                }
            }
        }

        public async Task<ResponseDTO> GetAllCoursesAsync()
        {
            try
            {
                var courses = await _unitOfWork.Course.GetAllByListAsync(c => !c.IsDeleted);
                return new ResponseDTO("Success", StatusCodeEnum.OK, true, courses);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", StatusCodeEnum.InteralServerError, false);
            }
        }

        public async Task<ResponseDTO> GetByEnrollcoursebyUserID(Guid userId)
        {
            try
            {
                var enrolledCourses = await _unitOfWork.EnrolledCourse.GetEnrolledCourseByUserIdAsync(userId);
                return new ResponseDTO("Get Enrolled Courses Successfully", StatusCodeEnum.OK, true, enrolledCourses);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", StatusCodeEnum.InteralServerError, false);
            }
        }

        public async Task<ResponseDTO> GetCourseDetailAsync(Guid courseId)
        {
            try
            {
                var courseQuery = _unitOfWork.Course
                    .GetAll()
                    .Where(c => c.Id == courseId && !c.IsDeleted);

                courseQuery = courseQuery
                    .Include(c => c.Topics.Where(t => !t.IsDeleted))
                    .ThenInclude(t => t.Exercises.Where(e => !e.IsDeleted));

                var course = await courseQuery.FirstOrDefaultAsync();

                if (course == null)
                    return new ResponseDTO("Course not found", StatusCodeEnum.NotFound, false);

                var courseDetail = new CourseDTO
                {
                    Id = course.Id,
                    CourseName = course.CourseName,
                    Description = course.Description,
                    MaxPoint = course.MaxPoint,
                    IsPremium = course.IsPremium,
                    IsActive = course.IsActive,
                    LevelId = course.LevelId,
                    Topics = course.Topics
                        .Select(t => new TopicDetailDTO
                        {
                            Id = t.Id,
                            TopicName = t.TopicName,
                            MaxPoint = t.MaxPoint,
                            IsActive = t.IsActive,
                            Exercises = t.Exercises
                                .Select(e => new ExerciseDetailDTO
                                {
                                    Id = e.Id,
                                    Content = e.Content,
                                    MaxPoint = e.MaxPoint,
                                    IsActive = e.IsActive
                                }).ToList()
                        }).ToList()
                };

                return new ResponseDTO("Course details retrieved successfully", StatusCodeEnum.OK, true, courseDetail);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", StatusCodeEnum.InteralServerError, false);
            }
        }

        public async Task<IEnumerable<Course>> SearchCourses(string keyword)
        {
            return await _unitOfWork.Course
                .FindAll(c =>
                    !c.IsDeleted &&
                    (c.CourseName.Contains(keyword) || c.Description.Contains(keyword)))
                .ToListAsync();
        }

        public async Task<ResponseDTO> CheckUserEnrollmentAsync(Guid userId, Guid courseId)
        {
            try
            {
                var enrollment = await _unitOfWork.EnrolledCourse
                    .GetByConditionAsync(ec =>
                        ec.UserId == userId &&
                        ec.CourseId == courseId);

                if (enrollment == null)
                    return new ResponseDTO("User has not enrolled in this course", StatusCodeEnum.NotFound, false);

                return new ResponseDTO("User is enrolled in this course", StatusCodeEnum.OK, true,
                    new { EnrolledCourseId = enrollment.Id });
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", StatusCodeEnum.InteralServerError, false);
            }
        }
    }
}