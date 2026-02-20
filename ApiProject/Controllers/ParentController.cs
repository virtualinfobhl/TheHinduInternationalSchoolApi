using ApiProject.Models.Request;
using ApiProject.Service.Parent_Login;
using ApiProject.Service.Parents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ParentController : BaseController
    {
        private readonly IParentsService _ParentsService;
        public ParentController(IParentsService parentsService)
        {
            _ParentsService = parentsService;
        }


        [HttpGet("GetStudentList")]
        public async Task<IActionResult> GetStudentList()
        {
            try
            {
                var res = await _ParentsService.GetStudentList();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        [HttpGet("GetStudentToken")]
        public async Task<IActionResult> GetStudentToken(int StudentId)
        {
            try
            {
                var result = await _ParentsService.GetStudentToken(StudentId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        // Student details 
        [HttpGet("GetStudentParentsDetails")]
        public async Task<IActionResult> GetStudentParentsDetails()
        {
            try
            {
                var res = await _ParentsService.GetStudentParentsDetail();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        // student installment fee 
        [HttpGet("GetStudentInstallmentFee")]
        public async Task<IActionResult> GetStudentInstallmentFee()
        {
            try
            {
                var res = await _ParentsService.GetStudentInstallmentFee();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        [HttpPost("AddStudentInstallmentFee")]
        public async Task<IActionResult> AddStudentInstallmentFee(AddStudentinstallReq req)
        {
            try
            {
                var res = await _ParentsService.AddStudentInstallmentFee(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


//        [HttpPost("WebhookStatus")]
//        public async Task<IActionResult> WebhookStatus()
//        {
//            try
//            {
//                // 1️⃣ Read Request Body
//                string body;
//                using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
//                {
//                    body = await reader.ReadToEndAsync();
//                }

//                Console.WriteLine($"PhonePe Webhook Body: {body}");

//                // 2️⃣ Verify Authorization Header
//                var authHeader = Request.Headers["Authorization"].FirstOrDefault();

//                if (string.IsNullOrEmpty(authHeader))
//                {
//                    Console.WriteLine("Authorization header missing");
//                    return Unauthorized("Unauthorized - Missing Authorization");
//                }

//                if (!VerifyPhonePeAuthorization(authHeader))
//                {
//                    Console.WriteLine("Authorization verification failed");
//                    return Unauthorized("Unauthorized - Invalid Credentials");
//                }

//                // 3️⃣ Parse JSON
//                JObject json = JObject.Parse(body);

//                string eventType = json["event"]?.ToString();

//                if (string.IsNullOrEmpty(eventType))
//                    return Ok(); // prevent retry

//                var payload = json["payload"];
//                if (payload == null)
//                    return Ok();

//                // 4️⃣ Extract Data
//                string orderId = payload["orderId"]?.ToString();
//                string merchantId = payload["merchantId"]?.ToString();
//                string merchantOrderId = payload["merchantOrderId"]?.ToString();
//                string paymentState = payload["state"]?.ToString();

//                long? amountInPaisa = payload["amount"]?.Value<long>();
//                decimal amount = amountInPaisa.HasValue ? amountInPaisa.Value / 100m : 0;

//                // 5️⃣ Payment Details
//                var paymentDetailsArray = payload["paymentDetails"] as JArray;

//                string paymentMode = "";
//                string transactionId = "";
//                string errorCode = "";
//                string detailedErrorCode = "";
//                DateTime? paymentTimestamp = null;
//                decimal paidAmount = 0;

//                if (paymentDetailsArray != null && paymentDetailsArray.Count > 0)
//                {
//                    var firstPayment = paymentDetailsArray[0];

//                    transactionId = firstPayment["transactionId"]?.ToString();
//                    paymentMode = firstPayment["paymentMode"]?.ToString();
//                    errorCode = firstPayment["errorCode"]?.ToString();
//                    detailedErrorCode = firstPayment["detailedErrorCode"]?.ToString();

//                    // ✅ Convert Epoch to DateTime (UTC)
//                    long? timestampEpoch = firstPayment["timestamp"]?.Value<long>();
//                    paymentTimestamp = timestampEpoch.HasValue
//                        ? DateTimeOffset.FromUnixTimeMilliseconds(timestampEpoch.Value).UtcDateTime
//                        : null;

//                    long? paidAmountPaisa = firstPayment["amount"]?.Value<long>();
//                    paidAmount = paidAmountPaisa.HasValue ? paidAmountPaisa.Value / 100m : 0;
//                }

//                // 6️⃣ Meta Info
//                var metaInfo = payload["metaInfo"];
//                string udf1 = metaInfo?["udf1"]?.ToString();
//                string udf2 = metaInfo?["udf2"]?.ToString();

//                // 7️⃣ Find Fee Record
//                var fee = await _db.M_FeeDetail .FirstOrDefaultAsync(p => p.OrderNo == merchantOrderId);

//                if (fee == null)
//                {
//                    Console.WriteLine($"Fee not found: {merchantOrderId}");
//                    return Ok(); // prevent retry
//                }

//                // 8️⃣ Process Payment
//                switch (eventType)
//                {
//                    case "checkout.order.completed":
//                        if (paymentState == "COMPLETED")
//                        {
//                            await ProcessSuccessfulPayment(fee, transactionId, paymentMode, paymentTimestamp, paidAmount);
//                        }
//                        break;

//                    case "checkout.order.failed":
//                        if (paymentState == "FAILED")
//                        {
//                            await ProcessFailedPayment(fee, transactionId, paymentMode, errorCode, detailedErrorCode, paymentTimestamp);
//                        }
//                        break;

//                    default:
//                        await ProcessPendingPayment(fee);
//                        break;
//                }

//                return Ok(new
//                {
//                    success = true,
//                    merchantTransactionId = merchantOrderId,
//                    status = paymentState
//                });
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Webhook Error: {ex.Message}");
//                return Ok(); // prevent retry
//            }
//        }

//        // 🔐 Authorization Verification Method
//        private bool VerifyPhonePeAuthorization(string authHeader)
//        {
//            // TODO: Add real signature verification logic here
//            return true;
//        }

//        // 💰 Success Payment
//        private async Task ProcessSuccessfulPayment(dynamic fee, string txnId, string mode, DateTime? time, decimal amount)
//        {
//            fee.Status = "PAID";
//            fee.TransactionId = txnId;
//            fee.PaymentMode = mode;
//            fee.PaymentDate = time ?? DateTime.UtcNow;
//            fee.PaidAmount = amount;

//            await _db.SaveChangesAsync();
//        }

//        // ❌ Failed Payment
//        private async Task ProcessFailedPayment(dynamic fee, string txnId, string mode, string error, string detail, DateTime? time)
//        {
//            fee.Status = "FAILED";
//            fee.TransactionId = txnId;
//            fee.PaymentMode = mode;
//            fee.ErrorCode = error;
//            fee.ErrorMessage = detail;
//            fee.PaymentDate = time ?? DateTime.UtcNow;

//            await _db.SaveChangesAsync();
//        }

//        // ⏳ Pending
//        private async Task ProcessPendingPayment(dynamic fee)
//        {
//            fee.Status = "PENDING";
//            await _db.SaveChangesAsync();
//        }
//}


























        [HttpGet("UpdateStudentPaymentSuccessfully")]
        public async Task<IActionResult> UpdateStudentPaymentSuccessfully(int StudentId, int ReceiptId, string orderno)
        {
            try
            {
                var res = await _ParentsService.UpdateStudentPaymentSuccessfully(StudentId, ReceiptId, orderno);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        // transport fee 
        [HttpGet("GetTransportInstallFee")]
        public async Task<IActionResult> GetTransportInstallFee()
        {
            try
            {
                var res = await _ParentsService.GetTransportInstallFee();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        [HttpPost("AddStudentTransportFee")]
        public async Task<IActionResult> AddStudentTransportFee(AddTransportMonthFeeReq req)
        {
            try
            {
                var res = await _ParentsService.AddStudentTransportFee(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("UpdateTransportPaymentSuccessfully")]
        public async Task<IActionResult> UpdateTransportPaymentSuccessfully(int StudentId, int ReceiptId)
        {
            try
            {
                var res = await _ParentsService.UpdateTransportPaymentSuccessfully(StudentId, ReceiptId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }


        // Other 
        [HttpGet("GetStudentFee")]
        public async Task<IActionResult> GetStudentFee()
        {
            try
            {
                var res = await _ParentsService.GetStudentFee();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        [HttpGet("GetStudentFeeInstallment")]
        public async Task<IActionResult> GetStudentFeeInstallment()
        {
            try
            {
                var res = await _ParentsService.GetStudentFeeInstallment();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        [HttpGet("GetStudentDueInstallment")]
        public async Task<IActionResult> GetStudentDueInstallment()
        {
            try
            {
                var res = await _ParentsService.GetStudentDueInstallment();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }


    }
}
