using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

public class UdpNetwork
{
    IPEndPoint ipp;

    public UdpNetwork(string host, int port)
    {
        ipp = new IPEndPoint(Dns.GetHostAddresses(host)[0], port);
    }
    public UdpNetwork(IPAddress ip, int port)
    {
        ipp = new IPEndPoint(ip, port);
    }
    public byte[] Send(byte[] send, int timeOut=4000, int tryTimes=4)
    {
        byte[] receive = null;
        UdpClient client = new UdpClient();
        client.Client.SendTimeout = client.Client.ReceiveTimeout = timeOut;
        int times = 0;
        bool done = false;
        do
        {

            try
            {
                client.Send(send, send.Length, ipp);
                receive = client.Receive(ref ipp);
                done = true;
            }
            catch
            {
                times++;
            }
        } while (times < tryTimes && !done);

        return receive ?? null;
    }
    public byte[] Send(string send, int timeOut=4000, int tryTimes=4)
    {
        return Send(Encoding.UTF8.GetBytes(send), timeOut, tryTimes);
    }
    /*public byte[] UDPSend(string send, int timeOut)
    {
        return UDPSend(Encoding.UTF8.GetBytes(send), timeOut, 4);
    }
    public byte[] UDPSend(string send)
    {
        return UDPSend(Encoding.UTF8.GetBytes(send), 4000, 4);
    }*/
}
public class TcpNetwork
{
    TcpClient tcp = new TcpClient();
    public NetworkStream StreamToServer { get; set; }
    public TcpNetwork(string host, int port):this(Dns.GetHostAddresses(host)[0], port){}
    public TcpNetwork(IPAddress ip, int port)
    {
        tcp.Connect(ip, port);
        if (tcp.Connected)
        {
            StreamToServer = tcp.GetStream();
        } else
        {
            throw new Exception("TCP didn't connect successfuly");
        }
    }
    public byte[] Send(byte[] buffer)
    {
        lock(StreamToServer)
        {
            StreamToServer.Write(buffer, 0, buffer.Length);
            int bufferSize = 2048000;
            byte[] receiveBuffer=new byte[bufferSize];
            StreamToServer.Read(receiveBuffer,0, bufferSize);
            int endPointer = 0;
            for(int i=0;i<receiveBuffer.Length;i++)
            {
                if (receiveBuffer[i] != 0)
                    endPointer = i;
            }
            byte[] resultBuffer = new byte[endPointer];
            for(int i = 0; i < resultBuffer.Length; i++)
            {
                resultBuffer[i] = receiveBuffer[i];
            }
            return resultBuffer;
        }
    }
}
