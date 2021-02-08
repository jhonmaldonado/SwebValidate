using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace SwebValidate.Models
{
    public class Administration
    {
    }

    public class CommonMethods
    {
        public static JToken ConsumirServicioWeb_GET_Token(string servicio, Int16 tipoTrans)
        {
            try
            {

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;//delegate { return true; }; //
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(servicio);
                httpWebRequest.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.ClientCertificates.Add(new System.Security.Cryptography.X509Certificates.X509Certificate());
                //var certificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(new byte[], pass, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var jsonResult = streamReader.ReadToEnd();
                    jsonResult = jsonResult.TrimStart(new char[] { '[' }).TrimEnd(new char[] { ']' });

                    //registrarConsumoServicios(servicio, httpWebRequest.Method, JObject.Parse(jsonResult).ToString(), "", "", tipoTrans, 0);

                    //Trace("FlypassServices", "CommonMethods.ConsumirServicioWeb_GET_Token", p, "Servicio: " + servicio + ", Respuesta: " + jsonResult);

                    return JObject.Parse(jsonResult);
                }
            }
            catch (Exception exception)
            {
                //Trace("FlypassServices", "CommonMethods.ConsumirServicioWeb_GET_Token", p, "Servicio: " + servicio + ", Exception: " + exception.Message);
                return "";
            }
        }
    }
}