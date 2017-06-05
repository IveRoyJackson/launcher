using Heyo.Class.Data;
using System;
using System.Runtime.Serialization;

namespace HeyoCraft.Class.Data.Yggdrasil
{
	public class Response : Json
    {
		public string error { get; set; }

        public string errorMessage { get; set; }

        public string cause { get; set; }
	}
}
