namespace FSMS.Service.ViewModels.Payments
{
    public class PaymentResponse
    {
        public string SellerImageMomoUrl { get; set; }
        public decimal DepositAmount { get; set; }
        public decimal RemainingAmount { get; set; }
    }
}
