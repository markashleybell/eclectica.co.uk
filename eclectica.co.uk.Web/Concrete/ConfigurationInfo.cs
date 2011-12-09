using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eclectica.co.uk.Web.Abstract;

namespace eclectica.co.uk.Web.Concrete
{
    public class ConfigurationInfo : IConfigurationInfo
    {
        private string _cdn1;
        private string _cdn2;
        private string _imageLibraryFolder;
        private int _indexPageSize;
        private string _facebookApplicationID;
        private string _facebookPageID;
        private string _twitterConsumerKey;
        private string _twitterConsumerSecret;
        private string _twitterAccessToken;
        private string _twitterAccessTokenSecret;
        private string _twitterAnywhereAPIKey;

        public string CDN1 { get { return _cdn1; } }
        public string CDN2 { get { return _cdn2; } }
        public string ImageLibraryFolder { get { return _imageLibraryFolder; } }
        public int IndexPageSize { get { return _indexPageSize; } }
        public string FacebookApplicationID { get { return _facebookApplicationID; } }
        public string FacebookPageID { get { return _facebookPageID; } }
        public string TwitterConsumerKey { get { return _twitterConsumerKey; } }
        public string TwitterConsumerSecret { get { return _twitterConsumerSecret; } }
        public string TwitterAccessToken { get { return _twitterAccessToken; } }
        public string TwitterAccessTokenSecret { get { return _twitterAccessTokenSecret; } }
        public string TwitterAnywhereAPIKey { get { return _twitterAnywhereAPIKey; } }

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
                                 string twitterAnywhereAPIKey)
        {
            _cdn1 = cdn1;
            _cdn2 = cdn2;
            _imageLibraryFolder = imageLibraryFolder;
            _indexPageSize = indexPageSize;
            _facebookApplicationID = facebookApplicationID;
            _facebookPageID = facebookPageID;
            _twitterConsumerKey = twitterConsumerKey;
            _twitterConsumerSecret = twitterConsumerSecret;
            _twitterAccessToken = twitterAccessToken;
            _twitterAccessTokenSecret = twitterAccessTokenSecret;
            _twitterAnywhereAPIKey = twitterAnywhereAPIKey;
        }
    }
}