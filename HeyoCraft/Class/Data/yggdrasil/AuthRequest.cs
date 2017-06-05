using Heyo.Class.Data;
using System.Runtime.Serialization;

namespace HeyoCraft.Class.Data.Yggdrasil
{
    [DataContract, KnownType(typeof(AuthRequest))]
	public class AuthRequest : Json
	{
        [DataMember]
		public Agent agent { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string password { get; set; }
        [DataMember]
        public string clientToken { get; set; }
        [DataMember]
        public string requestUser { get; set; }
        public AuthRequest(string username, string password,string clientToken)
		{
			requestUser = "true";
			agent = Agent.MINECRAFT;
			this.username = username;
			this.clientToken = clientToken;
			this.password = password;
		}
	}
}
