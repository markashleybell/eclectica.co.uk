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
        public string _facebookApplicationID;
        public string _facebookPageID;

        public string CDN1 { get { return _cdn1; } }
        public string CDN2 { get { return _cdn2; } }
        public string ImageLibraryFolder { get { return _imageLibraryFolder; } }
        public int IndexPageSize { get { return _indexPageSize; } }
        public string FacebookApplicationID { get { return _facebookApplicationID; } }
        public string FacebookPageID { get { return _facebookPageID; } }

        public ConfigurationInfo(string cdn1, string cdn2, string imageLibraryFolder, int indexPageSize, string facebookApplicationID, string facebookPageID)
        {
            _cdn1 = cdn1;
            _cdn2 = cdn2;
            _imageLibraryFolder = imageLibraryFolder;
            _indexPageSize = indexPageSize;
            _facebookApplicationID = facebookApplicationID;
            _facebookPageID = facebookPageID;
        }
    }
}