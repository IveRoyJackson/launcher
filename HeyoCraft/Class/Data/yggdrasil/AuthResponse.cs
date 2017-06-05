using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HeyoCraft.Class.Data.Yggdrasil
{
	
	public class AuthResponse : Response
	{
		
		public class User
		{
			
			public string id { get; set; }
            public List<Properties> properties { get; set; }

		}

		
		public class Properties
		{
			
			public string name { get; set; }

            public string value { get; set; }

        }

		
		public string accessToken { get; set; }


        public string clientToken { get; set; }


        public GameProfile selectedProfile { get; set; }


        public List<GameProfile> availableProfiles { get; set; }


        public User user { get; set; }

	}
}
