using Azure.Core;
using BLL.Interface;
using Common.DTO;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Hubs
{
   public class ChatHub : Hub
    {
        private readonly IAIService _aiService;
        private readonly IUnitOfWork _unitOfWork;

        public ChatHub(IAIService aiIService, IUnitOfWork unitOfWork)
        {
            _aiService = aiIService;
            _unitOfWork = unitOfWork;
        }
        public async Task<string> SendMessage(ChatHubDTO chatHubDTO)
        { 
           
            var userId = await _unitOfWork.User.GetByIdAsync(chatHubDTO.UserId);
            _aiService.SetCurrentTopic(chatHubDTO.TopicId);
            var response = await _aiService.ProcessConversationAsync(chatHubDTO.Message);
            return response.BotResponse;
        }
    }
}
