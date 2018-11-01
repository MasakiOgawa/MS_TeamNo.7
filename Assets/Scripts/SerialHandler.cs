using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;

public class SerialHandler : MonoBehaviour
{
    public delegate void SerialDataReceivedEventHandler(string message);
    public event SerialDataReceivedEventHandler OnDataReceived;

    //ポート名
    //例
    //Linuxでは/dev/ttyUSB0
    //windowsではCOM1
    //Macでは/dev/tty.usbmodem1421など
    public string portName = "COM3";
    public int baudRate = 9600;

    private SerialPort serialPort_;
    private Thread thread_;
    private bool isRunning_ = false;

    private string message_;
    private bool isNewMessageReceived_ = false;

    void Awake()
    {
        Open();
    }

    void Update()
    {
        if (isNewMessageReceived_)
        {
            OnDataReceived(message_);
        }
        isNewMessageReceived_ = false;
    }

   // void OnDestroy()
   void OnApplicationQuit( )
    {
        Debug.Log( "デストロイ前");
        Debug.Log( "Close4" );
            serialPort_.Close();
            Debug.Log( "Close5" );
            serialPort_.Dispose();
            Debug.Log( "Close6" );
        Close();
    }

    private void Open()
    {
        //serialPort_ = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
        //または
        serialPort_ = new SerialPort(portName, baudRate);
        serialPort_.Open();

        isRunning_ = true;

        thread_ = new Thread(Read);
        thread_.Start();
    }

    private void Close()
    {
        Debug.Log( "Close1" );

        isNewMessageReceived_ = false;
        isRunning_ = false;

        if (thread_ != null && thread_.IsAlive)
        {
            Debug.Log( "Close2" );
            thread_.Join();
            Debug.Log( "Close3" );
        }

        if (serialPort_ != null && serialPort_.IsOpen)
        {
          /*  Debug.Log( "Close4" );
            serialPort_.Close();
            Debug.Log( "Close5" );
            serialPort_.Dispose();
            Debug.Log( "Close6" );*/
        }
        Debug.Log( "Close7" );
    }

    private void Read()
    {
        while (isRunning_ && serialPort_ != null && serialPort_.IsOpen)
        {
            try
            {
                message_ = serialPort_.ReadLine();
                isNewMessageReceived_ = true;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }
    }

    public void Write(string message)
    {
        try
        {
            serialPort_.Write(message);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }
}