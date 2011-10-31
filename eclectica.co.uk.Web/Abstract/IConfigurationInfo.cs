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
    }
}