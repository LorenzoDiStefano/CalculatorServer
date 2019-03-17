using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace CalculatorServer
{
    public class CalculatorServer
    {
        private delegate byte[] GameCommand(float firstNumber, float secondNumber, EndPoint sender);
        private Dictionary<byte, GameCommand> commandsTable;
        private ITransport transport;

        public CalculatorServer(ITransport gameTransport)
        {
            transport = gameTransport;
            commandsTable= new Dictionary<byte, GameCommand>();
            commandsTable[0] = Sum;
            commandsTable[1] = Subtraction;
            commandsTable[2] = Division;
            commandsTable[3] = Multiplication;
        }

        public void Run()
        {
            Console.WriteLine("server started");
            while (true)
            {
                SingleStep();
            }
        }

        public bool Send(byte[] data, EndPoint endPoint)
        {
            return transport.Send(data, endPoint);
        }

        public void SingleStep()
        {
            EndPoint sender = transport.CreateEndPoint();
            byte[] data = transport.Recv(256, ref sender);
            if (data != null)
            {
                byte gameCommand = data[0];
                if (commandsTable.ContainsKey(gameCommand))
                {
                    float firstNumber = BitConverter.ToSingle(data, 1);
                    float secondNumber = BitConverter.ToSingle(data, 5);
                    byte[] dataToSend = commandsTable[gameCommand](firstNumber,secondNumber, sender);
                    Send(dataToSend, sender);
                }
            }
        }

        private byte[] Sum(float firstNumber, float secondNumber, EndPoint sender)
        {
            float result = firstNumber + secondNumber;
            return BitConverter.GetBytes(result);
        }

        private byte[] Subtraction(float firstNumber, float secondNumber, EndPoint sender)
        {
            float result = firstNumber - secondNumber;
            return BitConverter.GetBytes(result);
        }

        private byte[] Division(float firstNumber, float secondNumber, EndPoint sender)
        {
            float result = firstNumber / secondNumber;
            return BitConverter.GetBytes(result);
        }

        private byte[] Multiplication(float firstNumber, float secondNumber, EndPoint sender)
        {
            float result = firstNumber * secondNumber;
            return BitConverter.GetBytes(result);
        }
    }
}
