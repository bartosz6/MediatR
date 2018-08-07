using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Tests.DummyClasses
{
    public class VoidNullPingHandler : IRequestHandler<VoidNullPing, Unit>
    {
        public Task<Unit> Handle(VoidNullPing request, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }
    }
}