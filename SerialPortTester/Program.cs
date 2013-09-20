using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace SerialPortTester
{
    class Program
    {
        string data = "";
        static SerialPort _serialPort;
        static bool _continue = true;
        static void Main(string[] args)
        {
            string name;
            string message;
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            Thread readThread = new Thread(Read);

            // Create a new SerialPort object with default settings.
            _serialPort = new SerialPort("COM1", 115200, Parity.None, 8, StopBits.One);

            // Allow the user to set the appropriate properties.
            _serialPort.Handshake = Handshake.RequestToSendXOnXOff;

            _serialPort.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);

            // Set the read/write timeouts
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;

            _serialPort.Open();
            Thread.Sleep(1000);
            readThread.Start();

            Console.Write("Name: ");
            name = Console.ReadLine();

            Console.WriteLine("Type QUIT to exit");

            while (_continue)
            {
                message = Console.ReadLine();

                if (stringComparer.Equals("quit", message))
                {
                    _continue = false;
                }
                else
                {
                    _serialPort.WriteLine(
                        String.Format("<{0}>: {1}", name, message));
                }
            }

            readThread.Join();
            _serialPort.Close();


        }

        public static void Read()
        {

            while (_continue)
            {
                try
                {
                    string message = _serialPort.ReadLine();
                    Console.WriteLine(message);
                }
                catch (TimeoutException) { }
            }
        }

        static void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string message = _serialPort.ReadLine();
            return;
        }


    }
}
