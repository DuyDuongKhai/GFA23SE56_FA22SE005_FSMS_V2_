using FSMS.Service.Services.PaymentServices;
using FSMS.Service.Utility;
using FSMS.Service.Utility.Exceptions;
using FSMS.Service.Validations.Payment;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.Payments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private IPaymentService _paymentService;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public PaymentsController(IPaymentService paymentService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _paymentService = paymentService;
            _jwtAuthOptions = jwtAuthOptions;
        }

        [HttpGet]
        //[Cache(1000)]
        [PermissionAuthorize("Customer", "Supplier", "Farmer")]
        public async Task<IActionResult> GetAllPayments(int? userId = null, int? orderId = null)
        {
            try
            {
                List<GetPayment> payments = await _paymentService.GetAllAsync(userId, orderId);

                return Ok(new
                {
                    Data = payments
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        [PermissionAuthorize("Customer", "Supplier", "Farmer")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            try
            {
                GetPayment payment = await _paymentService.GetAsync(id);
                return Ok(new
                {
                    Data = payment
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }


        [HttpPost]
        [PermissionAuthorize("Customer", "Supplier", "Farmer")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePayment createPayment)
        {
            try
            {
                var validator = new PaymentValidator();
                var validationResult = validator.Validate(createPayment);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }

                PaymentResponse paymentResponse = await _paymentService.CreatePaymentAsync(createPayment);


                return Ok(new
                {
                    Message = "Payment created successfully",
                    SellerImageMomoUrl = paymentResponse.SellerImageMomoUrl,
                    DepositAmount = paymentResponse.DepositAmount,
                    RemainingAmount = paymentResponse.RemainingAmount
                });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }
        [HttpPut("{id}/process")]
        [PermissionAuthorize("Supplier", "Farmer")]
        public async Task<IActionResult> ProcessPayment(int id, [FromBody] ProcessPaymentRequest processPaymentRequest)
        {
            try
            {
                await _paymentService.ProcessPayment(id, processPaymentRequest);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }


        [HttpDelete("{id}")]
        [PermissionAuthorize("Supplier", "Farmer")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            try
            {
                await _paymentService.DeletePaymentAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetByOrderIdAsync(int orderId)
        {
            try
            {
                var payment = await _paymentService.GetByOrderIdAsync(orderId);

                return Ok(new
                {
                    Data = payment
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

    }
}
