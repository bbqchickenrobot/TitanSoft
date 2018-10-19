using System.Threading.Tasks;

namespace TitanSoft.Api.Services
{
    public class FauxPaymentService : IPaymentService
    {
        public Task AcceptPayment(object info) => Task.CompletedTask;
    }
}
