using Heyo.Class.Data;
using System;
using System.Runtime.Serialization;
using System.Text;

namespace HeyoCraft.Class.Data.Yggdrasil
{
    [DataContract]
	public class GameProfile : Json
	{
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string name { get; set; }

        public GameProfile(string id, string name)
		{
			if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(name))
			{
				throw new ArgumentException("Name and ID cannot both be blank");
			}
			this.id = id;
			this.name = name;
		}


		public bool IsCmoplete()
		{
			return !string.IsNullOrEmpty(this.id) && !string.IsNullOrEmpty(this.name);
		}

		public int HashCode()
		{
			int hashCode = this.id.GetHashCode();
			return 31 * hashCode + this.name.GetHashCode();
		}
	}
}
