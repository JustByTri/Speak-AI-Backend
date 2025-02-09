using SpeakAI.Services.DTO;
using SpeakAI.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakAI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDTO> SignUpAsCustomer(UserDTO userDTO);
    }
}
