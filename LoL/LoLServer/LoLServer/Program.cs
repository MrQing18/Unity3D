using NetFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LoLServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerStart ss = new ServerStart(9000);
            ss.Start(6666);
        }
    }
}
