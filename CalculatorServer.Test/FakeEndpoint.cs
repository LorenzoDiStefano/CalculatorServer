﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace CalculatorServer.Test
{
    public class FakeEndPoint:EndPoint
    {
        //fields
        private string address;
        private int port;

        //properties
        public string Address
        {
            get
            {
                return address;
            }
        }

        public int Port
        {
            get
            {
                return port;
            }
        }

        //constructors
        public FakeEndPoint()
        {

        }

        public FakeEndPoint(string address, int port)
        {
            this.address = address;
            this.port = port;
        }

        //methods
        public override int GetHashCode()
        {
            return Address.GetHashCode() + Port;
        }

        public override bool Equals(object obj)
        {
            FakeEndPoint other = obj as FakeEndPoint;
            if (other == null)
                return false;

            return (this.address == other.address) && (this.port == other.port);
        }
    }
}
