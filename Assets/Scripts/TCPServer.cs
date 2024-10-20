using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;

public class TCPServer : MonoBehaviour
{
    public string mIpAddress = "127.0.0.1";
    public int mPortNumber = 2001;

    private TcpListener mTcpListener;
    private TcpClient mClient;

    public event Action<string> OnDataReceived;

    private async void Start()
    {
        var ip = IPAddress.Parse(mIpAddress);
        mTcpListener = new TcpListener(ip, mPortNumber);
        mTcpListener.Start();
        await AcceptClientAsync();
    }

    private async Task AcceptClientAsync()
    {
        try
        {
            mClient = await mTcpListener.AcceptTcpClientAsync();
            Debug.Log("Connect: " + mClient.Client.RemoteEndPoint);

            using (var stream = mClient.GetStream())
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                while (mClient.Connected)
                {
                    string data = await reader.ReadLineAsync();
                    if (data != null)
                    {
                        OnDataReceived?.Invoke(data);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error in AcceptClientAsync: {e.Message}");
        }
        finally
        {
            Debug.Log("Disconnect: " + mClient?.Client.RemoteEndPoint);
            mClient?.Close();
        }
    }

    private void OnApplicationQuit()
    {
        mTcpListener?.Stop();
        mClient?.Close();
    }
}