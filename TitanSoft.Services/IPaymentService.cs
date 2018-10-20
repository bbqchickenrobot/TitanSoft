using System;
using System.Threading.Tasks;

namespace TitanSoft.Services
{
    public interface IPaymentService
    {
        Task AcceptPayment(object info);
    }
}
