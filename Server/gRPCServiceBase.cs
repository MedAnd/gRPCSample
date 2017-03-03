using Grpc.Core;
using Grpc.Core.Utils;
using Proto.Messages;
using System.Threading.Tasks;
using static Proto.Remoting.Service;

namespace Server
{
    public class gRPCServiceBase : ServiceBase
    {
        private Pong _pong = null;

        public gRPCServiceBase() 
        {
            _pong = new Pong();
        }

        public override async Task Stream(IAsyncStreamReader<Ping> requestStream, IServerStreamWriter<Pong> responseStream, ServerCallContext context)
        {
            responseStream.WriteOptions = new WriteOptions(WriteFlags.NoCompress);
            await requestStream.ForEachAsync(async ping => await responseStream.WriteAsync(_pong));
        }

        public override Task<Pong> Request(Ping request, ServerCallContext context)
        {
            return Task.FromResult(_pong);
        }
    }
}
