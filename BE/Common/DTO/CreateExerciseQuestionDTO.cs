using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class CreateExerciseQuestionDTO
    {
        public string Content { get; set; }
        public List<CreateExerciseAnswerDTO> Answers { get; set; } 
    }
}
