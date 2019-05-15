using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetFrame
{
    public class ServerStart
    {
        Socket server;

        int maxClient; // 客户端最大连接数
        Semaphore acceptClients;
        UserTokenPool pool;

        public ServerStart(int max)
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            maxClient = max;
            pool = new UserTokenPool(max);
            acceptClients = new Semaphore(max, max);
            for (int i = 0; i < max; i++)
            {
                UserToken token = new UserToken();
                token.receiveSAEA.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                token.sendSAEA.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                pool.Push(token);
            }
        }

        public void Start(int port)
        {
            //监听当前服务器网卡所有可用的IP地址的port端口
            server.Bind(new IPEndPoint(IPAddress.Any, port));
            //置于监听状态
            server.Listen(10);
            StartAccept(null);
        }
        //开始客户端连接监听
        public void StartAccept(SocketAsyncEventArgs e)
        {
            if(e == null)
            {
                e = new SocketAsyncEventArgs();
                e.Completed += new EventHandler<SocketAsyncEventArgs>(Accept_Completed);
            }
            else{
                e.AcceptSocket = null;
            }
            acceptClients.WaitOne();
            bool result = server.AcceptAsync(e);
            // 判断异步事件是否挂起  没挂起说明立刻执行完成，直接处理事件,否则会在处理完后触发Accept_Completed事件
            if (! result)
            {
                ProcessAccept(e);
            }
        }
        public void ProcessAccept(SocketAsyncEventArgs e)
        {
            UserToken token = pool.Pop();
            token.conn = e.AcceptSocket;
            // 通知应用层 有客户端连接

            //开启消息到达监听
            StartReceive(token);
            //释放当前异步对象
            StartAccept(e);
        }
        public void Accept_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }
        public void StartReceive(UserToken token)
        {
            // 用户连接对象 开启异步数据接收
            bool res = token.conn.ReceiveAsync(token.receiveSAEA);
            if (! res)
            {
                ProcessReceive(token.receiveSAEA);
            }
        }
        public void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (e.LastOperation == SocketAsyncOperation.Receive)
            {
                ProcessReceive(e);
            }
            else
            {
                ProcessSend(e);
            }
        }
        public void ProcessReceive(SocketAsyncEventArgs e)
        {
            UserToken token = e.UserToken as UserToken;
            // 判断网络消息是否成功
            if (token.receiveSAEA.BytesTransferred > 0 && token.receiveSAEA.SocketError == SocketError.Success)
            {
                byte[] message = new byte[token.receiveSAEA.BytesTransferred];
                Buffer.BlockCopy(token.receiveSAEA.Buffer, 0, message, 0, token.receiveSAEA.BytesTransferred);
                token.receive(message);
                //处理接收到的消息
                StartReceive(token);
            }else
            {
                if (token.receiveSAEA.SocketError != SocketError.Success)
                {
                    ClientClose(token, token.receiveSAEA.SocketError.ToString());
                }else
                {
                    ClientClose(token, "客户端主动断开连接");
                }
            }
        }
        public void ProcessSend(SocketAsyncEventArgs e)
        {
            UserToken token = e.UserToken as UserToken;
            if (e.SocketError != SocketError.Success)
            {
                ClientClose(token, e.SocketError.ToString());
            }else
            {
                // 消息发送成功，回调成功
                token.writed();
            }
        }
        // 客户端断开连接 断开连接的错误编码
        public void ClientClose(UserToken token, string error)
        {
            if(token.conn != null)
            {
                lock (token)
                {
                    //通知应用层 客户端断开连接
                    token.Close();
                    pool.Push(token);
                    acceptClients.Release();
                }
            }
        }
    }
}
