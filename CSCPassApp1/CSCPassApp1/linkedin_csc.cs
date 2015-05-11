using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace CSCPassApp1
{
    public class linkedin_csc
    {
        public const string RequestUrl = "https://api.linkedin.com/uas/oauth/requestToken";
        public const string AuthorizeUrl = "https://www.linkedin.com/uas/oauth/authorize";
        public const string AccessUrl = "https://api.linkedin.com/uas/oauth/accessToken";
        public const string RedirectUrl = "https://www.linkedin.com/uas/oauth/authorize/submit";
        public const string ReauthenticateUrl = "https://www.linkedin.com/uas/oauth/authorize/oob";

        public const string _linkedInRequestTokenUrl = "https://api.linkedin.com/uas/oauth/requestToken";
        public const string _linkedInAccessTokenUrl = "https://api.linkedin.com/uas/oauth/accessToken";

        public const string _requestPeopleUrl = "http://api.linkedin.com/v1/people/~";
        public const string _requestConnectionsUrl = "http://api.linkedin.com/v1/people/~/connections";
        public const string _requestPositionsUrl = "http://api.linkedin.com/v1/people/~:(positions)";

        public const string ConsumerKey = "78hx9p5dq0x17x";
        public const string ConsumerSecret = "emMVJyvDB7DfOonb";

        public static string tokensecret = "";
        public static string tokenvalue = "";

        public static string outhtoken = "";
        public static string outhverifier = "";
    }


}
