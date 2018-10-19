using System;
using System.Threading.Tasks;

namespace TitanSoft.Api.Services
{
    public interface IPaymentService
    {
        Task AcceptPayment(object info);
    }
}
