using BLL.Interface;
using BLL.IService;
using Common.Constant.Payment;
using Common.DTO;
using Common.DTO.Payment;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Service.IService;

namespace Service.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly ITransactionService _transactionService;
        private readonly IVnPayService _vnpPayService;
        private readonly IUserService _userService;

        public PaymentService(ITransactionService transactionService,
                              IVnPayService vnpPayService,
                              IUserService userService)
        {
            _transactionService = transactionService;
            _vnpPayService = vnpPayService;
            _userService = userService;
        }

        public async Task<string> CreatePaymentRequest(PaymentRequestDTO paymentInfo, HttpContext context)
        {
            var unpaidTrans = _transactionService.GetLastTransOfUser(paymentInfo.UserId);

            if (unpaidTrans != null)
            {
                unpaidTrans.Status = PaymentConstant.CancelStatus;
                await _transactionService.UpdateTransaction(unpaidTrans.TransactionId, unpaidTrans);
            }

             await _transactionService.AddNewTransaction(new Transaction
            {
                PaymentMethod = paymentInfo.PaymentMethod,
                TransactionDate = DateTime.Now,
                Amount = paymentInfo.TotalPrice,
                TransactionInfo = PaymentConstant.UnSet,
                TransactionNumber = PaymentConstant.UnSet,
                Status = PaymentConstant.PendingStatus,
                UserId = paymentInfo.UserId
            });

        
                return _vnpPayService.CreatePaymentUrl(paymentInfo, context);
            

         
        }

        public async Task<bool> HandlePaymentResponse(PaymentResponseDTO response)
        {
            var unpaidTrans = _transactionService.GetLastTransOfUser(response.UserId);
                
            if (unpaidTrans != null && unpaidTrans.Status == PaymentConstant.PendingStatus)
            {
                string notifyDes;

                if (response.IsSuccess)
                {
                    // Cập nhật transaction
                    unpaidTrans.Status = PaymentConstant.PaidStatus;
                    unpaidTrans.TransactionInfo = response.TransactionInfo;
                    unpaidTrans.TransactionNumber = response.TransactionNumber;

                    // Nâng cấp người dùng
                    var user = await _userService.GetUserById(response.UserId);
                    if (user != null)
                    {
                        user.IsPremium = true;
                        user.PremiumExpiredTime = DateTime.Now.AddMonths(1); 
                        await _userService.UpdateUser(user);
                    }

                    notifyDes = "Nâng cấp Premium thành công!";
                }
                else
                {
                    unpaidTrans.TransactionInfo = response.TransactionInfo;
                    unpaidTrans.TransactionNumber = response.TransactionNumber;
                    unpaidTrans.Status = PaymentConstant.CancelStatus;

                    notifyDes = "Thanh toán thất bại!";
                }

                await _transactionService.UpdateTransaction(unpaidTrans.TransactionId, unpaidTrans);
                return true;
            }

            return false;
        }
    }
}
