namespace ClothesDG.Core.Contracts
{
    using Stripe.Checkout;
    public interface IPaymentsService
    {
        Task<Session> CreateCheckoutSessionAsync(string userId);

        Task RefundAsync(string orderNumber);
    }
}
