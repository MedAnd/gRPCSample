using Grpc.Core;
using Proto.Messages;
using System;
using System.Threading.Tasks;
using static Proto.Remoting.Service;

namespace Client
{
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            StreamMessages().Wait();
        }

        public static async Task StreamMessages()
        {
            try
            {
                var channel = new Channel("127.0.0.1", 81, ChannelCredentials.Insecure);
                ServiceClient client = new ServiceClient(channel);
                client.Stream().RequestStream.WriteOptions = new WriteOptions(WriteFlags.NoCompress);

                var messageCount = 1000000;
                Ping ping = new Ping();

                Console.WriteLine("Test Client sending on port 81");
               
                var start = DateTime.Now;
                var call = client.Stream();
                for (var i = 0; i < messageCount; i++)
                {
                    //fire and forget, response not required as we want to test max throughput
                    await call.RequestStream.WriteAsync(ping);
                }

                await client.Stream().RequestStream.CompleteAsync();

                var elapsed = DateTime.Now - start;
                Console.WriteLine("StreamMessages Elapsed {0}", elapsed);
                var t = messageCount * 2.0 / elapsed.TotalMilliseconds * 1000;
                Console.WriteLine("StreamMessages Throughput {0} msg / sec", t);

                channel.ShutdownAsync().Wait();

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
