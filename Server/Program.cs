using Grpc.Core;
using System;
using static Proto.Remoting.Service;

namespace Server
{
    public class Program
    {
        private static Grpc.Core.Server _server;

        public static void Main(string[] args)
        {
            _server = new Grpc.Core.Server
            {
                Services = { BindService(new gRPCServiceBase()) },
                Ports = { new ServerPort("127.0.0.1", 81, ServerCredentials.Insecure) }
            };
            _server.Start();

            Console.WriteLine("Test Server listening on port 81");
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();
        }
    }
}
