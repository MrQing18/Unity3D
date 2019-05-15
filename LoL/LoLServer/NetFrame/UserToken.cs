using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame
{
    public class UserToken
    {
        // 用户连接socket
        public Socket conn;
        //用户网络数据对象
        public SocketAsyncEventArgs receiveSAEA;
        public SocketAsyncEventArgs sendSAEA;

        public UserToken()
        {
            receiveSAEA = new SocketAsyncEventArgs();
            sendSAEA = new SocketAsyncEventArgs();
            receiveSAEA.UserToken = this;
            sendSAEA.UserToken = this;
        }

        public void receive(byte[] buff)
        {

        }

        public void writed()
        {

        }
        public void Close()
        {
            try {

                conn.Shutdown(SocketShutdown.Both);
                conn.Close();
                conn = null;
            }catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
