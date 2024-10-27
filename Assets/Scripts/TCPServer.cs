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

    public event Action<string> OnDataReceived;

    private async void Start()
    {
        var ip = IPAddress.Parse(mIpAddress);
        mTcpListener = new TcpListener(ip, mPortNumber);
        mTcpListener.Start();
        Debug.Log($"Server started on {mIpAddress}:{mPortNumber}");
        await ListenForClientsAsync();
    }

    private async Task ListenForClientsAsync()
    {
        try
        {
            while (true)
            {
                Debug.Log("Waiting for client connection...");
                var client = await mTcpListener.AcceptTcpClientAsync();
                client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

                Debug.Log("Connected: " + client.Client.RemoteEndPoint);

                // 非同期でクライアントの処理を開始
                _ = HandleClientAsync(client);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error in ListenForClientsAsync: {e.Message}");
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        try
        {
            using (var stream = client.GetStream())
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                while (IsClientConnected(client))
                {
                    string data = await reader.ReadLineAsync();
                    if (data != null)
                    {
                        OnDataReceived?.Invoke(data);
                    }
                    else
                    {
                        Debug.Log("Client disconnected gracefully.");
                        break;
                    }
                }
            }
        }
        catch (IOException ioEx)
        {
            Debug.LogWarning($"Network error: {ioEx.Message}");
        }
        catch (SocketException sockEx)
        {
            Debug.LogWarning($"Socket error: {sockEx.Message}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Unexpected error in HandleClientAsync: {ex.Message}");
        }
        finally
        {
            Debug.Log("Disconnect: " + client.Client.RemoteEndPoint);
            client.Close(); // クライアント接続を閉じる
        }
    }


    private void OnApplicationQuit()
    {
        mTcpListener?.Stop();
        Debug.Log("Server stopped.");
    }

    private bool IsClientConnected(TcpClient client)
    {
        try
        {
            return !(client.Client.Poll(1, SelectMode.SelectRead) && client.Client.Available == 0);
        }
        catch (SocketException)
        {
            return false;
        }
    }

}
