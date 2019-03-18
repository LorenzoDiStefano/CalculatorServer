using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using System.IO;
using CalculatorServer.Test.ServerCalculator.Test;
using NUnit.Framework.Constraints;

namespace CalculatorServer.Test
{
    public class CalculatorServerTests
    {
        public CalculatorServer server;
        public FakeEndPoint client;
        public FakeTransport transport;

        [SetUp]
        public void SetUp()
        {
            client= new FakeEndPoint("1.1.1.1",5555);
            transport= new FakeTransport();
            server= new CalculatorServer(transport);
        }

        [Test]
        public void SingleStep()
        {
            Assert.That(() => server.SingleStep(), Throws.Exception);
        }

        [Test]
        public void ClientDequeue()
        {
            Assert.That(() => transport.ClientDequeue(), Throws.Exception);
        }

        [Test]
        public void SendingMoreBytes()
        {
            float firstNumber = 5f;
            float secondNumber = 5f;
            float notNecessaryNumber = 5f;
            byte sumCommand = 0;

            FakeData packet = new FakeData();
            packet.data = new FakePacket(sumCommand, firstNumber, secondNumber, notNecessaryNumber).GetData();
            packet.endPoint = client;

            transport.ClientEnqueue(packet);

            Assert.That(() => server.SingleStep(), Throws.Exception);
        }

        [Test]
        public void SendingLessBytes()
        {
            float firstNumber = 5f;
            byte sumCommand = 0;

            FakeData packet = new FakeData();
            packet.data = new FakePacket(sumCommand, firstNumber).GetData();
            packet.endPoint = client;

            transport.ClientEnqueue(packet);

            Assert.That(() => server.SingleStep(), Throws.Exception);
        }

        //SumTesting
        [Test]
        public void CheckSumWithPositiveNumbers()
        {
            float firstNumber = 5f;
            float secondNumber = 5f;
            byte sumCommand = 0;

            FakeData packet = new FakeData();
            packet.data = new FakePacket(sumCommand, firstNumber,secondNumber).GetData();
            packet.endPoint = client;

            transport.ClientEnqueue(packet);
            server.SingleStep();

            float result = BitConverter.ToSingle(transport.ClientDequeue().data, 0);

            Assert.That(result, Is.EqualTo(10f));
        }

        [Test]
        public void CheckSumWithNegativeNumbers()
        {
            float firstNumber = -5f;
            float secondNumber = -5f;
            byte sumCommand = 0;

            FakeData packet = new FakeData();
            packet.data = new FakePacket(sumCommand, firstNumber, secondNumber).GetData();
            packet.endPoint = client;

            transport.ClientEnqueue(packet);
            server.SingleStep();

            float result = BitConverter.ToSingle(transport.ClientDequeue().data, 0);

            Assert.That(result, Is.EqualTo(-10f));
        }

        [Test]
        public void CheckSumWithMaxValue()
        {
            float firstNumber = float.MaxValue;
            float secondNumber = float.MaxValue;
            byte sumCommand = 0;

            FakeData packet = new FakeData();
            packet.data = new FakePacket(sumCommand, firstNumber, secondNumber).GetData();
            packet.endPoint = client;

            transport.ClientEnqueue(packet);
            server.SingleStep();

            float result = BitConverter.ToSingle(transport.ClientDequeue().data, 0);

            Assert.That(result, Is.EqualTo(float.PositiveInfinity));
        }

        [Test]
        public void CheckSumWithMinValue()
        {
            float firstNumber = float.MinValue;
            float secondNumber = float.MinValue;
            byte sumCommand = 0;

            FakeData packet = new FakeData();
            packet.data = new FakePacket(sumCommand, firstNumber, secondNumber).GetData();
            packet.endPoint = client;

            transport.ClientEnqueue(packet);
            server.SingleStep();

            float result = BitConverter.ToSingle(transport.ClientDequeue().data, 0);

            Assert.That(result, Is.EqualTo(float.NegativeInfinity));
        }

        //Subtractions
        [Test]
        public void CheckSubtractionWithPositiveNumbers()
        {
            float firstNumber = 5f;
            float secondNumber = 5f;
            byte subtractionCommand = 1;

            FakeData packet = new FakeData();
            packet.data = new FakePacket(subtractionCommand, firstNumber,secondNumber).GetData();
            packet.endPoint = client;

            transport.ClientEnqueue(packet);
            server.SingleStep();

            float result = BitConverter.ToSingle(transport.ClientDequeue().data, 0);

            Assert.That(result, Is.EqualTo(0f));
        }

        [Test]
        public void CheckSubtractionWithNegativeNumbers()
        {
            float firstNumber = -5f;
            float secondNumber = -5f;
            byte subtractionCommand = 1;

            FakeData packet = new FakeData();
            packet.data = new FakePacket(subtractionCommand, firstNumber, secondNumber).GetData();
            packet.endPoint = client;

            transport.ClientEnqueue(packet);
            server.SingleStep();

            float result = BitConverter.ToSingle(transport.ClientDequeue().data, 0);

            Assert.That(result, Is.EqualTo(0f));
        }

        [Test]
        public void CheckSubtractionMaxValue()
        {
            float firstNumber = float.MaxValue;
            float secondNumber = float.MaxValue;
            byte subtractionCommand = 1;

            FakeData packet = new FakeData();
            packet.data = new FakePacket(subtractionCommand, firstNumber, secondNumber).GetData();
            packet.endPoint = client;

            transport.ClientEnqueue(packet);
            server.SingleStep();

            float result = BitConverter.ToSingle(transport.ClientDequeue().data, 0);

            Assert.That(result, Is.EqualTo(0f));
        }

        [Test]
        public void CheckSubtractionMinValue()
        {
            float firstNumber = float.MinValue;
            float secondNumber = float.MinValue;
            byte subtractionCommand = 1;

            FakeData packet = new FakeData();
            packet.data = new FakePacket(subtractionCommand, firstNumber, secondNumber).GetData();
            packet.endPoint = client;

            transport.ClientEnqueue(packet);
            server.SingleStep();

            float result = BitConverter.ToSingle(transport.ClientDequeue().data, 0);

            Assert.That(result, Is.EqualTo(0f));
        }

        //Divisions
        [Test]
        public void CheckDivisionWithPositiveNumbers()
        {
            float firstNumber = 5f;
            float secondNumber = 5f;
            byte divisionCommand = 2;

            FakeData packet = new FakeData();
            packet.data = new FakePacket(divisionCommand, firstNumber, secondNumber).GetData();
            packet.endPoint = client;

            transport.ClientEnqueue(packet);
            server.SingleStep();

            float result = BitConverter.ToSingle(transport.ClientDequeue().data, 0);

            Assert.That(result, Is.EqualTo(1f));
        }

        [Test]
        public void CheckDivisionWithNegativeNumbers()
        {
            float firstNumber = -5f;
            float secondNumber = -5f;
            byte divisionCommand = 2;

            FakeData packet = new FakeData();
            packet.data = new FakePacket(divisionCommand, firstNumber, secondNumber).GetData();
            packet.endPoint = client;

            transport.ClientEnqueue(packet);
            server.SingleStep();

            float result = BitConverter.ToSingle(transport.ClientDequeue().data, 0);

            Assert.That(result, Is.EqualTo(1f));
        }

        [Test]
        public void CheckDivisionByZero()
        {
            float firstNumber = 0f;
            float secondNumber = 0f;
            byte divisionCommand = 2;

            FakeData packet = new FakeData();
            packet.data = new FakePacket(divisionCommand, firstNumber, secondNumber).GetData();
            packet.endPoint = client;

            transport.ClientEnqueue(packet);
            server.SingleStep();

            float result = BitConverter.ToSingle(transport.ClientDequeue().data, 0);

            Assert.That(result, Is.EqualTo(float.NaN));
        }

        //multiplications
        [Test]
        public void CheckMultiplicationWithPositiveNumbers()
        {
            float firstNumber = 5f;
            float secondNumber = 5f;
            byte divisionCommand = 3;

            FakeData packet = new FakeData();
            packet.data = new FakePacket(divisionCommand, firstNumber, secondNumber).GetData();
            packet.endPoint = client;

            transport.ClientEnqueue(packet);
            server.SingleStep();

            float result = BitConverter.ToSingle(transport.ClientDequeue().data, 0);

            Assert.That(result, Is.EqualTo(25f));
        }

        [Test]
        public void CheckMultiplicationWithNegativeNumbers()
        {
            float firstNumber = -5f;
            float secondNumber = -5f;
            byte multiplicationCommand = 3;

            FakeData packet = new FakeData();
            packet.data = new FakePacket(multiplicationCommand, firstNumber, secondNumber).GetData();
            packet.endPoint = client;

            transport.ClientEnqueue(packet);
            server.SingleStep();

            float result = BitConverter.ToSingle(transport.ClientDequeue().data, 0);

            Assert.That(result, Is.EqualTo(25f));
        }

        [Test]
        public void CheckMultiplicationWithFloatMaxValue()
        {
            float firstNumber = float.MaxValue;
            float secondNumber = float.MaxValue;
            byte multiplicationCommand = 3;

            FakeData packet = new FakeData();
            packet.data = new FakePacket(multiplicationCommand, firstNumber, secondNumber).GetData();
            packet.endPoint = client;

            transport.ClientEnqueue(packet);
            server.SingleStep();

            float result = BitConverter.ToSingle(transport.ClientDequeue().data, 0);
            Console.WriteLine(firstNumber);
            Console.WriteLine(result);
            Assert.That(result, Is.EqualTo(float.PositiveInfinity));
        }

        [Test]
        public void CheckMultiplicationWithFloatMinValue()
        {
            float firstNumber = float.MinValue;
            float secondNumber = float.MinValue;
            byte multiplicationCommand = 3;

            FakeData packet = new FakeData();
            packet.data = new FakePacket(multiplicationCommand, firstNumber, secondNumber).GetData();
            packet.endPoint = client;

            transport.ClientEnqueue(packet);
            server.SingleStep();

            float result = BitConverter.ToSingle(transport.ClientDequeue().data, 0);
            Console.WriteLine(firstNumber);
            Console.WriteLine(result);
            Assert.That(result, Is.EqualTo(float.PositiveInfinity));
        }


    }
}
