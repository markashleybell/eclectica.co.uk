using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eclectica.co.uk.Web.Abstract;

namespace eclectica.co.uk.Web.Concrete
{
    public class ConfigurationInfo : IConfigurationInfo
    {
        public string CDN1 { get; private set; }
        public string CDN2 { get; private set; }
        public string ImageLibraryFolder { get; private set; }
        public int IndexPageSize { get; private set; }
        public string FacebookApplicationID { get; private set; }
        public string FacebookPageID { get; private set; }
        public string TwitterConsumerKey { get; private set; }
        public string TwitterConsumerSecret { get; private set; }
        public string TwitterAccessToken { get; private set; }
        public string TwitterAccessTokenSecret { get; private set; }
        public string TwitterAnywhereAPIKey { get; private set; }

        public string ErrorDigestKey { get; private set; }

        // HttpException message strings
        public string Error403Message { get; private set; }
        public string Error404Message { get; private set; }
        public string Error500Message { get; private set; }

        public ConfigurationInfo(string cdn1, 
                                 string cdn2, 
                                 string imageLibraryFolder, 
                                 int indexPageSize, 
                                 string facebookApplicationID, 
                                 string facebookPageID,
                                 string twitterConsumerKey,
                                 string twitterConsumerSecret,
                                 string twitterAccessToken,
                                 string twitterAccessTokenSecret,
                                 string twitterAnywhereAPIKey,
                                 string errorDigestKey,
                                 string error403Message,
                                 string error404Message,
                                 string error500Message)
        {
            CDN1 = cdn1;
            CDN2 = cdn2;
            ImageLibraryFolder = imageLibraryFolder;
            IndexPageSize = indexPageSize;
            FacebookApplicationID = facebookApplicationID;
            FacebookPageID = facebookPageID;
            TwitterConsumerKey = twitterConsumerKey;
            TwitterConsumerSecret = twitterConsumerSecret;
            TwitterAccessToken = twitterAccessToken;
            TwitterAccessTokenSecret = twitterAccessTokenSecret;
            TwitterAnywhereAPIKey = twitterAnywhereAPIKey;
            ErrorDigestKey = errorDigestKey;
            Error403Message = error403Message;
            Error404Message = error404Message;
            Error500Message = error500Message;
        }
    }
}