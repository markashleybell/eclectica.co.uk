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

        public string CDN1 { get { return _cdn1; } }
        public string CDN2 { get { return _cdn2; } }
        public string ImageLibraryFolder { get { return _imageLibraryFolder; } }

        public ConfigurationInfo(string cdn1, string cdn2, string imageLibraryFolder)
        {
            _cdn1 = cdn1;
            _cdn2 = cdn2;
            _imageLibraryFolder = imageLibraryFolder;
        }
    }
}