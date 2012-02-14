using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eclectica.co.uk.Web.Abstract
{
    public interface IConfigurationInfo
    {
        string CDN1 { get; }
        string CDN2 { get; }
        string ImageLibraryFolder { get; }
        int IndexPageSize { get; }
        string FacebookApplicationID { get; }
        string FacebookPageID { get; }
        string TwitterConsumerKey { get; }
        string TwitterConsumerSecret { get; }
        string TwitterAccessToken { get; }
        string TwitterAccessTokenSecret { get; }
        string TwitterAnywhereAPIKey { get; }

        // HttpException message strings
        string Error403Message { get; }
        string Error404Message { get; }
        string Error500Message { get; }
    }
}