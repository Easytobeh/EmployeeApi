using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EmployeeService
{
    public class RequireHttpsAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //If request is not issued using https, redirect scheme to https
            if(actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Found);
                actionContext.Response.Content = new StringContent("<p>Use HTTPS instead of HTTP</p>", Encoding.UTF8, "text/html");

                UriBuilder uriBuider = new UriBuilder(actionContext.Request.RequestUri);
                uriBuider.Scheme = Uri.UriSchemeHttps;
                uriBuider.Port = 44393;

                actionContext.Response.Headers.Location = uriBuider.Uri;
            }
            else
            {
                base.OnAuthorization(actionContext); 
            }
        }
    }
}