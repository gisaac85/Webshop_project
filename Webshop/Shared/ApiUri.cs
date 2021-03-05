using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop.Shared
{
    public static class ApiUri
    {
        public static string APIBaseUri = "https://localhost:5001/";
        public static string ProductBaseUri = $"{APIBaseUri}api/products/";
        public static string AccountBaseUri = $"{APIBaseUri}api/account/";
    }
}
