using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Tests.DummyClasses
{
    public class NullPingHandler : IRequestHandler<NullPing, Pong>
    {
        public Task<Pong> Handle(NullPing request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Pong());
        }
    }
}