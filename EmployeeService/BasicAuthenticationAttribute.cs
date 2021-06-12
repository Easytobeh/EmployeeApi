using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using EmloyeeSecurity;

namespace EmployeeService
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
           if(actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.Unauthorized);
            }

            else
            {
                //A base64 encoded string passed from the client to the server
                string authenticationToken = actionContext.Request.Headers
                    .Authorization.Parameter;

                //decode to retrieve string and then encode it
                string decodedAuthenticationToken =  Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));

                string[] usernameAndPassword =  decodedAuthenticationToken.Split(':');
                string username = usernameAndPassword[0];
                string password = usernameAndPassword[1];

                //set current principal of the executing thread to this parameters
                if(new EmLogin(username, password))



            }
        }
    }
}