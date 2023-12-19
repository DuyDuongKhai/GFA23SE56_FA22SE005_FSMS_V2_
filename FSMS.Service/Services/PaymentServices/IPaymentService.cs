using FSMS.Service.ViewModels.Payments;

namespace FSMS.Service.Services.PaymentServices
{
    public interface IPaymentService
    {
        Task<List<GetPayment>> GetAllAsync(int? userId = null, int? orderId = null);
        Task<GetPayment> GetAsync(int key);
        Task<PaymentResponse> CreatePaymentAsync(CreatePayment createPayment);
        Task DeletePaymentAsync(int key);
        Task<PaymentWithOrderDetails> GetByOrderIdAsync(int orderId);
        Task ProcessPayment(int paymentId, ProcessPaymentRequest processPaymentRequest);



    }
}
