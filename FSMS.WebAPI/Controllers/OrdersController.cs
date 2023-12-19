using FSMS.Service.Services.OrderServices;
using FSMS.Service.Utility;
using FSMS.Service.Utility.Exceptions;
using FSMS.Service.Validations.Order;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.Orders;
using FSMS.WebAPI.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IOrderService _orderService;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public OrdersController(IOrderService orderService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _orderService = orderService;
            _jwtAuthOptions = jwtAuthOptions;
        }

        [HttpGet]
        [Cache(1000)]
        //[PermissionAuthorize("Customer", "Supplier", "Farmer")]
        public async Task<IActionResult> GetAllOrders(int? buyerUserId = null, int? sellerUserId = null, string? status = null)
        {
            try
            {
                List<GetOrder> orders = await _orderService.GetAllAsync(buyerUserId, sellerUserId, status);
                orders = orders.OrderByDescending(c => c.CreatedDate).ToList();

                return Ok(new
                {
                    Data = orders
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
        [Cache(1000)]

        //[PermissionAuthorize("Customer", "Supplier", "Farmer")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                GetOrder order = await _orderService.GetAsync(id);
                return Ok(new
                {
                    Data = order
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

        [PermissionAuthorize("Customer", "Supplier")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrder createOrder)
        {
            try
            {
                // Validate the incoming CreateOrder object
                var validator = new OrderValidator();
                var validationResult = validator.Validate(createOrder);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }

                // Call the service method to create the order
                await _orderService.CreateOrderAsync(createOrder);

                // If the order creation is successful, return a success response
                return Ok(new
                {
                    Message = "Order created successfully",
                    // Include additional information if needed
                });
            }
            catch (BadRequestException ex)
            {
                // Handle specific exceptions (e.g., validation errors)
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateOrder: {ex.Message}");

                return StatusCode(500, new
                {
                    Message = "An error occurred while processing the request."
                });
            }
        }






        [HttpPut("{id}")]

        [PermissionAuthorize("Customer", "Supplier")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrder updateOrder)
        {
            var validator = new UpdateOrderValidator();
            var validationResult = validator.Validate(updateOrder);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }
                await _orderService.UpdateOrderAsync(id, updateOrder);
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

        [PermissionAuthorize("Customer", "Supplier")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
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

        [HttpPut]
        [Route("{id}/process")]

        [PermissionAuthorize("Supplier", "Farmer")]
        public async Task<IActionResult> ProcessOrder(int id, string action)
        {
            try
            {
                if (action != "Accepted" && action != "Rejected" && action != "UserRefused" && action != "Shipping")
                {
                    return BadRequest(new
                    {
                        Message = "Invalid action. Please specify 'Accepted', 'Rejected', 'UserRefused', or 'Shipping' in the URL."
                    });
                }

                if (action == "Accepted" || action == "UserRefused" || action == "Rejected" || action == "Shipping")
                {
                    // Process the order for acceptance, user refusal, rejection, or shipping
                    await _orderService.ProcessOrderAsync(id, action);

                    return Ok(new
                    {
                        Message = $"Order {action.ToLower()} successfully processed."
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = "Invalid action provided."
                    });
                }
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
