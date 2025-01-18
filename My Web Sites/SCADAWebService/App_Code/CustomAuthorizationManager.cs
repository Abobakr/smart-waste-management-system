using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;

/// <summary>
/// Summary description for CustomUserNameValidator
/// </summary>
public class CustomAuthorizationManager : ServiceAuthorizationManager
{
    protected override bool CheckAccessCore(OperationContext operationContext)
    {

        //Extract the Authorization header, and parse out the credentials converting the Base64 string:
        string authorizationHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
        if ((authorizationHeader != null) && (authorizationHeader != string.Empty))
        {
            //var serviceCredentials = System.Text.ASCIIEncoding.ASCII
            //        .GetString(Convert.FromBase64String(authorizationHeader.Substring(6)))
            //        .Split(':');
            string[] serviceCredentials = authorizationHeader.Split(':');
            var user = new { Name = serviceCredentials[0], Password = serviceCredentials[1] };
            if ((user.Name == "user1" && user.Password == "test1") || (user.Name == "user2" && user.Password == "test2"))
            {
                //User is authorized and originating call will proceed
                
                return true;
            }
            else
            {
                //not authorized

                WebOperationContext.Current.OutgoingResponse.Headers.Add(String.Format("WWW-Authenticate: Basic realm=\" Invalid username:{0}  or password:{1}\"",user.Name,user.Password));
                //Throw an exception with the associated HTTP status code equivalent to HTTP status 401
                throw new WebFaultException(HttpStatusCode.Unauthorized);

                //return false;
            }
        }
        else
        {
            //No authorization header was provided, so challenge the client to provide before proceeding:
            WebOperationContext.Current.OutgoingResponse.Headers.Add("WWW-Authenticate: Basic realm=\" Please provide valid username and password. \"");
            //Throw an exception with the associated HTTP status code equivalent to HTTP status 401
            throw new WebFaultException(HttpStatusCode.Unauthorized);
        }
    }
}

