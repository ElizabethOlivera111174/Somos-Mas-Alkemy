using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Helper.Pagination
{
    public class UriService : IUriService
    {
        #region Object and Constructor
        private readonly string _baseUri;
        public UriService(string baseUri)
        {
            this._baseUri = baseUri;
        } 
        #endregion
        public string GetPage(string route, int? page)
        {
            if (page.HasValue)
            {
                var endpoint = new Uri(string.Concat(_baseUri, route));
                var modUri = QueryHelpers.AddQueryString(endpoint.ToString(), "page", page.ToString());

                return new Uri(modUri).ToString();
            }
            return null;
        }
    }
}
