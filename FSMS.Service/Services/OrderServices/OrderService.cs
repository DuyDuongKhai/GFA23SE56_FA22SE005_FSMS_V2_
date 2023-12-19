using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Entity.Repositories.FruitDiscountRepositories;
using FSMS.Entity.Repositories.FruitRepositories;
using FSMS.Entity.Repositories.OrderDetailRepositories;
using FSMS.Entity.Repositories.OrderRepositories;
using FSMS.Entity.Repositories.UserRepositories;
using FSMS.Service.Enums;
using FSMS.Service.ViewModels.OrderDetails;
using FSMS.Service.ViewModels.Orders;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace FSMS.Service.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private IOrderDetailRepository _orderDetailRepository;
        private IOrderRepository _orderRepository;
        private IUserRepository _userRepository;
        private IFruitRepository _fruitRepository;
        private IFruitDiscountRepository _fruitDiscountRepository;

        private IMapper _mapper;
        public OrderService(IOrderDetailRepository orderDetailRepository, IMapper mapper, IOrderRepository orderRepository, IUserRepository userRepository,
            IFruitRepository fruitRepository, IFruitDiscountRepository fruitDiscountRepository)
        {
            _orderDetailRepository = orderDetailRepository;
            _mapper = mapper;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _fruitRepository = fruitRepository;
            _fruitDiscountRepository = fruitDiscountRepository;
        }

        public async Task CreateOrderAsync(CreateOrder createOrder)
        {
            try
            {
                User existedUser = await _userRepository.GetByIDAsync(createOrder.UserId);
                if (existedUser == null)
                {
                    throw new Exception("User Id does not exist in the system.");
                }

                if (createOrder.OrderDetails == null || !createOrder.OrderDetails.Any())
                {
                    throw new Exception("OrderDetails list is empty or null.");
                }

                int lastOrderId = (await _orderRepository.GetAsync()).Max(x => x.OrderId);

                Order order = new Order()
                {
                    UserId = createOrder.UserId,
                    OrderDate = DateTime.Now,
                    DeliveryAddress = createOrder.DeliveryAddress,
                    PaymentMethod = "COD",
                    PhoneNumber = createOrder.PhoneNumber,
                    Type = "",
                    Status = OrderEnum.Pending.ToString(),
                    CreatedDate = DateTime.Now,
                    OrderId = lastOrderId + 1,
                    ParentOrderId = lastOrderId + 1
                };

                decimal totalAmount = 0;
                string orderType = "";

                foreach (var orderDetailViewModel in createOrder.OrderDetails)
                {
                    int fruitId = orderDetailViewModel.FruitId;
                    int fruitDiscountId = orderDetailViewModel.FruitDiscountId;
                    Fruit fruit = await _fruitRepository.GetByIDAsync(fruitId);

                    if (fruit == null)
                    {
                        throw new Exception("Fruit does not exist in the system.");
                    }

                    FruitDiscount fruitDiscount = await _fruitDiscountRepository.GetByIDAsync(fruitDiscountId);

                    decimal unitPrice = fruit.Price;
                    decimal orderDetailTotalAmount = orderDetailViewModel.Quantity * unitPrice;

                    if (fruitDiscount != null)
                    {
                        decimal discountPercentage = fruitDiscount.DiscountPercentage ?? 0;
                        decimal discountAmount = orderDetailTotalAmount * discountPercentage;
                        orderDetailTotalAmount -= discountAmount;
                    }
                    totalAmount += orderDetailTotalAmount;


                    if (!string.IsNullOrWhiteSpace(fruit.OrderType))
                    {
                        orderType += fruit.OrderType + " ";
                    }

                    OrderDetail newOrderDetail = new OrderDetail()
                    {
                        FruitId = fruitId,
                        Quantity = orderDetailViewModel.Quantity,
                        UnitPrice = unitPrice,
                        TotalAmount = orderDetailTotalAmount,
                        OderDetailType = fruit.OrderType,
                        CreatedDate = DateTime.Now,
                        Status = OrderEnum.Pending.ToString()
                    };

                    if (fruitDiscount != null)
                    {
                        newOrderDetail.FruitDiscountId = fruitDiscountId;
                    }

                    order.OrderDetails.Add(newOrderDetail);
                }

                order.Type = orderType.Trim();
                order.TotalAmount = totalAmount;

                await _orderRepository.InsertAsync(order);
                await _orderRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Inner Exception: " + ex.InnerException?.Message);
                throw new Exception(ex.Message);
            }
        }


        public async Task DeleteOrderAsync(int key)
        {
            try
            {

                Order existedOrder = await _orderRepository.GetByIDAsync(key);

                if (existedOrder == null)
                {
                    throw new Exception("Order ID does not exist in the system.");
                }

                existedOrder.Status = OrderEnum.Cancelled.ToString();

                await _orderRepository.UpdateAsync(existedOrder);
                await _orderRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<GetOrder> GetAsync(int key)
        {
            try
            {
                Order order = await _orderRepository.GetByIDAsync(key);

                if (order == null)
                {
                    throw new Exception("Order ID does not exist in the system.");
                }

                List<Order> orders = (await _orderRepository.GetAsync(includeProperties: "User")).ToList();

                // Map the list of orders to a list of GetOrder

                // Map the Order to GetOrder
                GetOrder result = _mapper.Map<GetOrder>(order);

                // Fetch order details for the specific order
                IEnumerable<OrderDetail> orderDetails = await _orderDetailRepository.GetAsync(x => x.OrderId == key, includeProperties: "Fruit,FruitDiscount,Fruit.User");

                if (orderDetails != null)
                {
                    // Map the OrderDetails to GetOrderDetail
                    result.OrderDetails = _mapper.Map<List<GetOrderDetail>>(orderDetails.ToList());
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }





        public async Task<List<GetOrder>> GetAllAsync(int? buyerUserId = null, int? sellerUserId = null, string? status = null)
        {
            try
            {
                // Define a predicate to filter orders based on userId and status
                Expression<Func<Order, bool>> filterPredicate = order =>
                    (!buyerUserId.HasValue || order.UserId == buyerUserId.Value) &&
                    (sellerUserId == null || order.OrderDetails.Any(od => od.Fruit.UserId == sellerUserId || (od.Fruit.UserId != null && od.Fruit.UserId == sellerUserId))) &&
                    (string.IsNullOrEmpty(status) || order.Status == status);

                // Fetch orders with optional filtering
                List<Order> orders = (await _orderRepository.GetAsync(filter: filterPredicate, includeProperties: "User")).ToList();

                // Map the list of orders to a list of GetOrder
                List<GetOrder> result = _mapper.Map<List<GetOrder>>(orders);

                // Now, you need to fetch and map order details for each order and add them to the result.
                foreach (var order in result)
                {
                    IEnumerable<OrderDetail> orderDetails = await _orderDetailRepository.GetAsync(x => x.OrderId == order.OrderId, includeProperties: "Fruit,FruitDiscount,Fruit.User");

                    if (orderDetails != null)
                    {
                        // Map the OrderDetails to GetOrderDetail
                        order.OrderDetails = _mapper.Map<List<GetOrderDetail>>(orderDetails.ToList());

                        // If sellerUserId is specified, filter OrderDetails based on it
                        if (sellerUserId != null)
                        {
                            // Fetch the corresponding Fruit for each OrderDetail
                            var filteredOrderDetails = orderDetails
                                .Where(od => od.Fruit.UserId == sellerUserId || (od.Fruit.UserId != null && od.Fruit.UserId == sellerUserId))
                                .ToList();

                            // Map the filtered OrderDetails to GetOrderDetail
                            order.OrderDetails = _mapper.Map<List<GetOrderDetail>>(filteredOrderDetails);
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }






        public async Task UpdateOrderAsync(int key, UpdateOrder updateOrder)
        {
            try
            {
                Order existedOrder = await _orderRepository.GetByIDAsync(key);

                if (existedOrder == null)
                {
                    throw new Exception("Order ID does not exist in the system.");
                }

                if (!string.IsNullOrEmpty(updateOrder.DeliveryAddress))
                {
                    existedOrder.DeliveryAddress = updateOrder.DeliveryAddress;
                }

                if (!string.IsNullOrEmpty(updateOrder.PaymentMethod))
                {
                    existedOrder.PaymentMethod = updateOrder.PaymentMethod;
                }
                if (!string.IsNullOrEmpty(updateOrder.PhoneNumber))
                {
                    existedOrder.PhoneNumber = updateOrder.PhoneNumber;
                }

                if (!string.IsNullOrEmpty(updateOrder.Status))
                {
                    if (updateOrder.Status != "Pending" && updateOrder.Status != "Cancelled")
                    {
                        throw new Exception("Status must be 'Pending' or 'Cancelled'.");
                    }
                    existedOrder.Status = updateOrder.Status;
                }
                existedOrder.UpdateDate = DateTime.Now;

                await _orderRepository.UpdateAsync(existedOrder);
                await _orderRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Order> ProcessOrderAsync(int orderId, string action)
        {
            try
            {
                IEnumerable<Order> orderList = await _orderRepository.GetAsync(
                    filter: o => o.OrderId == orderId,
                    includeProperties: "OrderDetails,User"
                );

                Order existedOrder = orderList.FirstOrDefault();

                if (existedOrder == null)
                {
                    throw new Exception("Order ID does not exist in the system.");
                }

                switch (action)
                {
                    case "Accepted":
                        // Xử lý khi đơn hàng được chấp nhận
                        if (existedOrder.Status == OrderEnum.Shipping.ToString())
                        {
                            // Cập nhật thông tin liên quan đến chi tiết đơn hàng và sản phẩm
                            foreach (var orderDetail in existedOrder.OrderDetails)
                            {
                                // Cập nhật thông tin chiết khấu trái cây
                                FruitDiscount fruitDiscount = await _fruitDiscountRepository.GetByIDAsync(orderDetail.FruitDiscountId);
                                if (fruitDiscount != null)
                                {
                                    fruitDiscount.DiscountThreshold -= (int)orderDetail.Quantity;
                                    if (fruitDiscount.DiscountThreshold < 0)
                                    {
                                        throw new InvalidOperationException("DiscountThreshold cannot be negative.");
                                    }
                                    await _fruitDiscountRepository.UpdateAsync(fruitDiscount);
                                }

                                // Cập nhật thông tin sản phẩm trái cây
                                Fruit fruit = await _fruitRepository.GetByIDAsync(orderDetail.FruitId);

                                if (fruit != null)
                                {
                                    double orderDetailQuantity = orderDetail.Quantity;
                                    double fruitQuantityInTransit = fruit.QuantityInTransit ?? 0;

                                    double proposedQuantity = fruitQuantityInTransit * orderDetailQuantity;

                                    if (fruit.QuantityAvailable - proposedQuantity < 0)
                                    {
                                        throw new InvalidOperationException("QuantityAvailable cannot be negative.");
                                    }

                                    fruit.QuantityAvailable -= proposedQuantity;

                                    await _fruitRepository.UpdateAsync(fruit);
                                }
                            }

                            existedOrder.Status = OrderEnum.Accepted.ToString();
                            existedOrder.UpdateDate = DateTime.Now;
                            await _orderRepository.UpdateAsync(existedOrder);
                            await _orderRepository.CommitAsync();
                            await _orderDetailRepository.CommitAsync();
                            await _fruitDiscountRepository.CommitAsync();
                            await _fruitRepository.CommitAsync();

                            // Gửi email thông báo về việc chấp nhận đơn hàng
                            await SendOrderNotificationEmail(existedOrder, "Accepted");
                            return existedOrder;
                        }
                        else
                        {
                            throw new Exception("The order cannot be processed as its status is not 'Shipping'.");
                        }

                    case "UserRefused":
                        if (existedOrder.Status == OrderEnum.Shipping.ToString())
                        {
                            existedOrder.Status = OrderEnum.UserRefused.ToString();
                            existedOrder.UpdateDate = DateTime.Now;

                            // Convert ImageMomoUrl to int and increment the value
                            if (int.TryParse(existedOrder.User.ImageMomoUrl, out int imageMomoUrlCount))
                            {
                                imageMomoUrlCount++;

                                if (imageMomoUrlCount > 10)
                                {
                                    existedOrder.User.Status = "Ban";
                                    await SendUserInactiveNotificationEmail(existedOrder.User);
                                }

                                // Convert the updated value back to string and assign it to ImageMomoUrl
                                existedOrder.User.ImageMomoUrl = imageMomoUrlCount.ToString();

                                await _orderRepository.UpdateAsync(existedOrder);
                                await _userRepository.UpdateAsync(existedOrder.User);
                                await _orderRepository.CommitAsync();
                                await _userRepository.CommitAsync();

                                await SendOrderNotificationEmail(existedOrder, "UserRefused");
                                return existedOrder;
                            }
                            else
                            {
                                throw new Exception("Failed to parse ImageMomoUrl as an integer.");
                            }
                        }
                        else
                        {
                            throw new Exception("The order cannot be processed as its status is not 'Shipping'.");
                        }

                    case "Rejected":
                        // Xử lý khi đơn hàng bị từ chối
                        if (existedOrder.Status == OrderEnum.Pending.ToString())
                        {
                            // Existing code to update FruitDiscount and Fruit quantities

                            // Cập nhật trạng thái của đơn hàng và chi tiết đơn hàng
                            existedOrder.Status = OrderEnum.Rejected.ToString();
                            existedOrder.UpdateDate = DateTime.Now;
                            await _orderRepository.UpdateAsync(existedOrder);

                            foreach (var orderDetail in existedOrder.OrderDetails)
                            {
                                // Existing code to update FruitDiscount and Fruit quantities

                                orderDetail.Status = OrderEnum.Rejected.ToString();
                                await _orderDetailRepository.UpdateAsync(orderDetail);
                            }

                            await _orderRepository.CommitAsync();
                            await _orderDetailRepository.CommitAsync();
                            await _fruitDiscountRepository.CommitAsync();
                            await _fruitRepository.CommitAsync();

                            // Send email notification
                            await SendOrderNotificationEmail(existedOrder, "Rejected");
                        }
                        else
                        {
                            throw new Exception("The order cannot be processed as its status is not 'Pending'.");
                        }

                        break;

                    case "Shipping":
                        if (existedOrder.Status == OrderEnum.Pending.ToString())
                        {
                            // Cập nhật thông tin liên quan đến chi tiết đơn hàng và sản phẩm
                            foreach (var orderDetail in existedOrder.OrderDetails)
                            {
                                // Cập nhật thông tin chiết khấu trái cây
                                FruitDiscount fruitDiscount = await _fruitDiscountRepository.GetByIDAsync(orderDetail.FruitDiscountId);
                                if (fruitDiscount != null)
                                {
                                    fruitDiscount.DiscountThreshold -= (int)orderDetail.Quantity;
                                    if (fruitDiscount.DiscountThreshold < 0)
                                    {
                                        throw new InvalidOperationException("DiscountThreshold cannot be negative.");
                                    }
                                }

                                // Cập nhật thông tin sản phẩm trái cây
                                Fruit fruit = await _fruitRepository.GetByIDAsync(orderDetail.FruitId);

                                if (fruit != null)
                                {
                                    double orderDetailQuantity = orderDetail.Quantity;
                                    double fruitQuantityInTransit = fruit.QuantityInTransit ?? 0;

                                    double proposedQuantity = fruitQuantityInTransit * orderDetailQuantity;

                                    if (fruit.QuantityAvailable - proposedQuantity < 0)
                                    {
                                        throw new InvalidOperationException("QuantityAvailable cannot be negative.");
                                    }

                                    fruit.QuantityAvailable -= proposedQuantity;
                                }
                            }

                            existedOrder.Status = OrderEnum.Shipping.ToString();
                            existedOrder.UpdateDate = DateTime.Now;
                            await _orderRepository.UpdateAsync(existedOrder);
                            await _orderRepository.CommitAsync();
                            await SendOrderNotificationEmail(existedOrder, "Shipping");
                            return existedOrder;
                        }
                        else
                        {
                            throw new Exception("The order cannot be processed as its status is not 'Shipping'.");
                        }
                    default:
                        throw new Exception("Invalid action provided.");
                }
                return existedOrder;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task SendOrderNotificationEmail(Order order, string status)
        {
            try
            {
                if (order.User != null)
                {
                    string userEmail = order.User.Email;
                    string subject = $"Thông báo Đơn hàng {status}";

                    // Tạo nội dung email với toàn bộ thông tin của đơn hàng
                    StringBuilder bodyBuilder = new StringBuilder();
                    bodyBuilder.AppendLine($"Fruit Season Management xin chào {order.User.FullName},\n\n");
                    bodyBuilder.AppendLine($"Đơn hàng của bạn có ID {order.OrderId}\n");
                    bodyBuilder.AppendLine("Chi tiết đơn hàng:\n");

                    // Fetch OrderDetails with Fruit and FruitDiscount
                    var orderDetails = await _orderDetailRepository.GetAsync(x => x.OrderId == order.OrderId, includeProperties: "Fruit,FruitDiscount");

                    foreach (var orderDetail in orderDetails)
                    {
                        bodyBuilder.AppendLine($"Tên trái cây: {orderDetail.Fruit.FruitName}\n  Số lượng: {orderDetail.Quantity}\n Mã giảm giá: {orderDetail.FruitDiscount?.DiscountName ?? "Không có"}\n  Giá {orderDetail.UnitPrice} VND");
                    }

                    bodyBuilder.AppendLine($"\nTổng cộng: {order.TotalAmount} VND\n");

                    if (status == "Shipping")
                    {
                        bodyBuilder.AppendLine("Đơn hàng đã được chấp nhận và dự kiến sẽ được giao trong vòng 5-10 ngày.\n");
                        bodyBuilder.AppendLine("Bạn vui lòng đăng nhập vào hệ thống để xem chi tiết đơn hàng hơn\n");
                    }
                    else if (status == "Rejected")
                    {
                        bodyBuilder.AppendLine("Rất tiếc, đơn hàng của bạn đã bị từ chối.\n");
                        bodyBuilder.AppendLine("Bạn vui lòng đăng nhập vào hệ thống để xem chi tiết đơn hàng");
                    }
                    else if (status == "UserRefused")
                    {
                        bodyBuilder.AppendLine("Bạn đã từ chối nhận hàng.\n");
                        bodyBuilder.AppendLine($"Hiện tại bạn đã từ chối {order.User.ImageMomoUrl} lần \n");
                        bodyBuilder.AppendLine("Nếu bạn tiếp tục từ chối nhận hàng quá 10 lần, tài khoản của bạn sẽ bị khóa.\n");
                    }
                    else if (status == "Accepted")
                    {
                        bodyBuilder.AppendLine("Cảm ơn bạn đã nhận hàng\n");
                    }

                    await SendEmailAsync(userEmail, subject, bodyBuilder.ToString());
                }
                else
                {
                    throw new Exception("Thông tin người dùng không khả dụng cho đơn hàng.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Gửi email thông báo {status} thất bại. {ex.Message}");
            }
        }


        private async Task SendUserInactiveNotificationEmail(User user)
        {
            try
            {
                string userEmail = user.Email;
                string subject = "Thông báo Tài khoản không hoạt động";

                // Tạo nội dung email
                StringBuilder bodyBuilder = new StringBuilder();
                bodyBuilder.AppendLine($"Fruit Season Management xin chào {user.FullName},\n\n");
                bodyBuilder.AppendLine("Tài khoản của bạn đã bị đánh dấu là không hoạt động do vượt quá số lần từ chối đơn hàng cho phép.\n");
                bodyBuilder.AppendLine("Nếu bạn có bất kỳ câu hỏi hoặc cần hỗ trợ, vui lòng liên hệ với chúng tôi.\n");

                await SendEmailAsync(userEmail, subject, bodyBuilder.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception($"Gửi email thông báo Tài khoản không hoạt động thất bại. {ex.Message}");
            }
        }


        private async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("duongkhaiduy5@gmail.com", "vdob zizq mrvj ravs"),
                    EnableSsl = true,
                };

                var from = new MailAddress("duongkhaiduy5@gmail.com", "Fruit Season Management");
                var toAddress = new MailAddress(to);
                var message = new MailMessage(from, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false, // Set to true if your email body is HTML
                };

                await smtpClient.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send email. {ex.Message}");
            }
        }


    }
}
