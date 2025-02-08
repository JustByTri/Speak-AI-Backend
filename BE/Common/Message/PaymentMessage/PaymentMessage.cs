using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Message.PaymentMessage
{
    public class PaymentMessage
    {
        public const string OrderNotExist = "Order does not exist";

        public const string RequestPaymentVnPaySuccessfully = "Create request payment VN Pay successfully";

        public const string RequestPaymentVnPayFail = "Create request payment VN Pay failed";

        public const string RequestPaymentMomoSuccessfully = "Create request payment Momo successfully";

        public const string RequestPaymentMomoFail = "Create request payment Momo failed";

        public const string HandleResponseSuccessfully = "Handle response successfully";

        public const string HandleResponseFail = "Handle response failed";

    }
}
