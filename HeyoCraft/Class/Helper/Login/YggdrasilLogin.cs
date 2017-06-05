using Heyo.Class.Helper;
using HeyoCraft.Class.Data.Yggdrasil;
using System.IO;
using System.Net;
using System.Text;

namespace HeyoCraft.Class.Helper.Login
{
    public class YggdrasilLogin
	{

		//private string BaseUrl = "https://authserver.mojang.com/";

		private static string RouteAuthenticate = "https://authserver.mojang.com/authenticate";

		//private string RouteRefresh = "https://authserver.mojang.com/refresh";

		//private string RouteValidate = "https://authserver.mojang.com/validate";

		//private string RouteInvalidate = "https://authserver.mojang.com/invalidate";

		//private string RouteSignout = "https://authserver.mojang.com/signout";

        /// <summary>
        /// ��¼Mojang�˻�
        /// </summary>
        /// <param name="UserEmail">�û���</param>
        /// <param name="Password">����</param>
        /// <param name="Client_identifier">�ͻ��˱�ʶ��</param>
        /// <param name="Language">���ԣ�Ĭ��Ϊzh-cn</param>
        /// <returns></returns>
		public static User Login(string UserEmail, string Password, string Client_identifier = "", string Language = "zh-cn")
		{
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(RouteAuthenticate);
            httpWebRequest.Method = "POST";
            AuthRequest graph = new AuthRequest(UserEmail, Password,"");
            byte[] bytes = Encoding.UTF8.GetBytes(graph.ToString());
            httpWebRequest.ContentLength = bytes.LongLength;
            Stream requestStream = httpWebRequest.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string returnData = streamReader.ReadToEnd();
            AuthResponse authResponse = JsonHelper.JsonDeserializeBySingleData<AuthResponse>(returnData);
            User u = new User() {
                Email = UserEmail,
                Name = authResponse.selectedProfile.name,
                Suc = true,
                UUID = authResponse.user.id,
                SID = authResponse.selectedProfile.id,
                accessToken = authResponse.accessToken,
                Errinfo= authResponse.errorMessage
            };
            return u;
		}
	}
}
