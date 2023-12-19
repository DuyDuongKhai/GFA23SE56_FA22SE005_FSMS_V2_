using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Entity.Repositories.FruitDiscountRepositories;
using FSMS.Entity.Repositories.FruitRepositories;
using FSMS.Entity.Repositories.OrderDetailRepositories;
using FSMS.Entity.Repositories.OrderRepositories;
using FSMS.Entity.Repositories.PaymentRepositories;
using FSMS.Entity.Repositories.UserRepositories;
using FSMS.Service.Enums;
using FSMS.Service.ViewModels.OrderDetails;
using FSMS.Service.ViewModels.Payments;

namespace FSMS.Service.Services.PaymentServices
{
    public class PaymentService : IPaymentService
    {
        private IPaymentRepository _paymentRepository;
        private IOrderRepository _orderRepository;
        private IOrderDetailRepository _orderDetailRepository;
        private IUserRepository _userRepository;
        private IFruitRepository _fruitRepository;
        private IFruitDiscountRepository _fruitDiscountRepository;
        private IMapper _mapper;
        public PaymentService(IPaymentRepository paymentRepository, IMapper mapper, IOrderRepository orderRepository, IUserRepository userRepository, IFruitRepository fruitRepository, IOrderDetailRepository orderDetailRepository, IFruitDiscountRepository fruitDiscountRepository)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _fruitRepository = fruitRepository;
            _orderDetailRepository = orderDetailRepository;
            _fruitDiscountRepository = fruitDiscountRepository;
        }

        public async Task<PaymentResponse> CreatePaymentAsync(CreatePayment createPayment)
        {
            try
            {
                User existedUser = await _userRepository.GetByIDAsync(createPayment.UserId);
                if (existedUser == null)
                {
                    throw new Exception("User ID does not exist in the system.");
                }

                Order existedOrder = await _orderRepository.GetByIDAsync(createPayment.OrderId);
                if (existedOrder == null)
                {
                    throw new Exception("Order ID does not exist in the system.");
                }

                IEnumerable<OrderDetail> orderDetailsEnumerable = await _orderDetailRepository
                    .GetAsync(od => od.OrderId == createPayment.OrderId, includeProperties: "Fruit");

                OrderDetail orderDetail = orderDetailsEnumerable.FirstOrDefault();

                if (orderDetail == null)
                {
                    throw new Exception("Order ID does not exist in the system or does not have an associated OrderDetail.");
                }

                Fruit fruit = orderDetail.Fruit;
                if (fruit == null)
                {
                    throw new Exception("Fruit does not exist in the system.");
                }

                User seller = await _userRepository.GetByIDAsync(fruit.UserId);

                if (seller == null)
                {
                    throw new Exception("Seller (User) does not exist in the system.");
                }

                decimal depositAmount = 0;

                foreach (var orderDetail1 in orderDetailsEnumerable)
                {
                    if (orderDetail1.FruitDiscountId.HasValue)
                    {
                        FruitDiscount fruitDiscount = await _fruitDiscountRepository.GetByIDAsync(orderDetail1.FruitDiscountId.Value);
                        if (fruitDiscount != null)
                        {
                            decimal unitPrice = orderDetail1.Fruit.Price;

                            // Apply discount percentage
                            if (fruitDiscount.DiscountPercentage.HasValue)
                            {
                                decimal discountPercentage = fruitDiscount.DiscountPercentage.Value;
                                unitPrice = unitPrice * (1 - discountPercentage);
                            }

                            // Apply deposit amount percentage
                            if (fruitDiscount.DepositAmount.HasValue)
                            {
                                decimal depositPercentage = fruitDiscount.DepositAmount.Value;
                                depositAmount += (decimal)orderDetail1.Quantity * (decimal)unitPrice * depositPercentage;
                            }
                        }
                    }
                }
                decimal remainingAmount = existedOrder.TotalAmount - depositAmount;

                int lastId = (await _paymentRepository.GetAsync()).Max(x => x.PaymentId);
                Payment payment = new Payment()
                {
                    OrderId = createPayment.OrderId,
                    PaymentDate = DateTime.Now,
                    PaymentType = existedOrder.Type,
                    UserId = createPayment.UserId,
                    Amount = existedOrder.TotalAmount,
                    Status = PaymentEnum.Pending.ToString(),
                    CreatedDate = DateTime.Now,
                    PaymentId = lastId + 1
                };

                await _paymentRepository.InsertAsync(payment);
                await _paymentRepository.CommitAsync();

                PaymentResponse paymentResponse = new PaymentResponse
                {
                    SellerImageMomoUrl = seller.ImageMomoUrl,
                    DepositAmount = depositAmount,
                    RemainingAmount = remainingAmount
                };

                return paymentResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }







        public async Task DeletePaymentAsync(int key)
        {
            try
            {
                Payment existedPayment = await _paymentRepository.GetByIDAsync(key);

                if (existedPayment == null)
                {
                    throw new Exception("Payment ID does not exist in the system.");
                }

                existedPayment.Status = PaymentEnum.Cancelled.ToString();

                await _paymentRepository.UpdateAsync(existedPayment);
                await _paymentRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<GetPayment> GetAsync(int key)
        {
            try
            {
                Payment payment = await _paymentRepository.GetByIDAsync(key);

                if (payment == null)
                {
                    throw new Exception("Payment ID does not exist in the system.");
                }

                List<GetPayment> payments = _mapper.Map<List<GetPayment>>(await _paymentRepository.GetAsync(includeProperties: "User"));
                GetPayment result = _mapper.Map<GetPayment>(payment);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<GetPayment>> GetAllAsync(int? userId = null, int? orderId = null)
        {
            try
            {
                IEnumerable<Payment> payments = await _paymentRepository.GetAsync(includeProperties: "User");

                if (userId.HasValue)
                {
                    payments = payments.Where(payment => payment.UserId == userId.Value);
                }
                if (orderId.HasValue)
                {
                    payments = payments.Where(payment => payment.OrderId == orderId.Value);
                }

                List<GetPayment> result = _mapper.Map<List<GetPayment>>(payments);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }







        public async Task<PaymentWithOrderDetails> GetByOrderIdAsync(int orderId)
        {
            try
            {
                Payment payment = await _paymentRepository.GetByIDAsync(orderId);

                if (payment == null)
                {
                    throw new Exception("Payment with Order ID does not exist in the system.");
                }

                // Lấy thông tin chi tiết đơn hàng (Order Details) cho Payment
                IEnumerable<OrderDetail> orderDetailEnumerable = await _orderDetailRepository.GetAsync(od => od.OrderId == orderId);
                List<OrderDetail> orderDetails = orderDetailEnumerable.ToList();

                PaymentWithOrderDetails paymentWithOrderDetails = new PaymentWithOrderDetails()
                {
                    Payment = payment,
                    OrderDetails = _mapper.Map<List<GetOrderDetail>>(orderDetails)
                };

                return paymentWithOrderDetails;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task ProcessPayment(int paymentId, ProcessPaymentRequest processPaymentRequest)
        {
            try
            {
                Payment existedPayment = await _paymentRepository.GetByIDAsync(paymentId);

                if (existedPayment == null)
                {
                    throw new Exception("Payment does not exist for the given Payment ID.");
                }

                if (!string.IsNullOrEmpty(processPaymentRequest.Status))
                {
                    if (processPaymentRequest.Status != "Completed" && processPaymentRequest.Status != "Failed" && processPaymentRequest.Status != "Refunded")
                    {
                        throw new Exception("Status must be 'Completed' or 'Failed' or 'Refunded'.");
                    }

                    existedPayment.Status = processPaymentRequest.Status;
                    existedPayment.UpdateDate = DateTime.Now;

                    await _paymentRepository.UpdateAsync(existedPayment);
                    await _paymentRepository.CommitAsync();
                }
                else
                {
                    throw new Exception("Status is required.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }






    }
}
