using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WSCLP
{
    [ServiceContract]
    public interface ICLPService
    {
        [OperationContract]
        [WebInvoke (Method ="GET", UriTemplate ="/CLP/{id}",
         ResponseFormat = WebMessageFormat.Xml,
         RequestFormat = WebMessageFormat.Xml,
         BodyStyle = WebMessageBodyStyle.Wrapped)]
        bool CLP(string id);
    }
}
