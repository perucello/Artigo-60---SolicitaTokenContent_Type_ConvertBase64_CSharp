using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace SolicitaTokenToBase64String
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string usuario = ""; // insira o Usuario
                string senhaUsuario = ""; //Insira a senha - codigifcada
                string endpointRequestToken = ""; //insira o endpoint 

                var client = new HttpClient();
                String username = usuario;
                String password = senhaUsuario;

                //Convert to Base 64 da senha
                String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));

                //Validação
                var formData = "var1=val1&var2=val2";
                var encodedFormData = Encoding.ASCII.GetBytes(formData);

                //Criando request ao Endpoint
                var request = (HttpWebRequest)WebRequest.Create(endpointRequestToken);

                //Application
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";
                request.ContentLength = encodedFormData.Length;

                //Informando que Authorization é Basic Auth
                request.Headers.Add("Authorization", "Basic " + encoded);
                request.PreAuthenticate = true;

                var token = client + endpointRequestToken;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(encodedFormData, 0, encodedFormData.Length);
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                //Imprimindo no console nosso Token
                //Console.WriteLine("Token em Objeto - " + responseString);

                //Tratando o retorno do json do token para que fique descrito da maneira correta - neste cenário ficará "Bearer +token"
                string tratamento = responseString.Substring(0, 609);
                string tokenAcess = tratamento.Replace("access_token", "Bearer ").Replace("{", "").Replace(":", "").Replace(@"""", ""); 

                //Imprimindo no console nosso Token
                Console.WriteLine("Token em Objeto - " + tokenAcess);
                 
            }
            //Tratando Exceção genérica.
            catch (Exception e)
            {
                Console.WriteLine($"Exception Catch SolicitaToken :  {e} ");
                Console.WriteLine(e);
                Console.WriteLine("Http Status: " + HttpStatusCode.NotFound);

                throw new Exception($"Exception Catch SolicitaToken :  {e} ");
            }
        }
    }
}