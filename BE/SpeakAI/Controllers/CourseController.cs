using BLL.Interface;
using Common.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SpeakAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

    
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourse(Guid id)
        {
            var response = await _courseService.GetCourseByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] UpdateCourseDTO courseDto)
        {
            var response = await _courseService.UpdateCourseAsync(id, courseDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var response = await _courseService.DeleteCourseAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("topic/{id}")]
        public async Task<IActionResult> GetTopic(Guid id)
        {
            var response = await _courseService.GetTopicByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("{courseId}/topic")]
        public async Task<IActionResult> AddTopic(Guid courseId, [FromBody] CreateTopicDTO topicDto)
        {
            var response = await _courseService.AddTopicToCourseAsync(courseId, topicDto);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDTO courseDto)
        {
            var response = await _courseService.CreateCourseWithTopicsAndExercisesAsync(courseDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("topic/{id}")]
        public async Task<IActionResult> UpdateTopic(Guid id, [FromBody] UpdateTopicDTO topicDto)
        {
            var response = await _courseService.UpdateTopicAsync(id, topicDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("topic/{id}")]
        public async Task<IActionResult> DeleteTopic(Guid id)
        {
            var response = await _courseService.DeleteTopicAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("exercise/{id}")]
        public async Task<IActionResult> GetExercise(Guid id)
        {
            var response = await _courseService.GetExerciseByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("{topicId}/exercise")]
        public async Task<IActionResult> AddExercise(Guid topicId, [FromBody] CreateExerciseDTO exerciseDto)
        {
            var response = await _courseService.AddExerciseToTopicAsync(topicId, exerciseDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("exercise/{id}")]
        public async Task<IActionResult> UpdateExercise(Guid id, [FromBody] UpdateExerciseDTO exerciseDto)
        {
            var response = await _courseService.UpdateExerciseAsync(id, exerciseDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("exercise/{id}")]
        public async Task<IActionResult> DeleteExercise(Guid id)
        {
            var response = await _courseService.DeleteExerciseAsync(id);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost("{courseId}/enroll")]
        public async Task<IActionResult> Enroll(Guid courseId, [FromBody] Guid userId)
        {
            var result = await _courseService.EnrollCourseAsync(userId, courseId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("enrolled/{enrolledCourseId}")]
        public async Task<IActionResult> GetEnrolledDetails(Guid enrolledCourseId)
        {
            var result = await _courseService.GetEnrolledCourseDetailsAsync(enrolledCourseId);
            return StatusCode(result.StatusCode, result);
        }

    }
}
