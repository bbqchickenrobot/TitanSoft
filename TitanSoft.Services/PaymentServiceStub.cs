using System.Threading.Tasks;

namespace TitanSoft.Services
{
    public class PaymentServiceStub : IPaymentService
    {
        public Task AcceptPayment(object info) => Task.CompletedTask;
    }
}
