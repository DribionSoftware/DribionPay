using DribionPayExemplos.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DribionPayExemplos
{
    public class Pix
    {
        private HttpClient _httpClient;

        public Pix()
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 | (SecurityProtocolType)768;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.dribionpay.com.br/");
            // Url Produção: https://api.dribionpay.com.br/
            // Url Homologação: https://web.dribion.com/dribionpayapi/
            ProcessarPix();
        }

        public void  ProcessarPix()
        {
            var cultureInfo = new CultureInfo("pt-BR");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            Console.WriteLine("Gerando um Pix, via DribionPay...");


            // Quando seu cliente, fazer o pagamento da sua "venda" via Pix.

            // Informações do usuário e empresa
            var usuario = ""; // Usuario da DribionPay
            var senha = ""; // Senha do usuario
            var empresaKey = ""; //Chave da empresa

            var dadosPix = new PixDTO
            {
                IdPagamento = "1254124",
                ReferenciaPagamento = "Venda ID 4512",
                Observacao = "Venda de Produto X",
                Valor = 10.00m,
                DuracaoEmMinutos = 30, // Duração do Pix em minutos
                Ambiente = "Homologacao", // Ambiente de operação (Homologacao ou Producao)
                Plataforma = "Birdix", // Plataforma de pagamento utilizada para gerar o Pix
                EmpresaKey = empresaKey // Chave da empresa emissora do Pix
            };

            try
            {
                var accessToken = AutenticarUsuarioDribionPay(usuario, senha, out string erroLogin);
                if (string.IsNullOrEmpty(accessToken))
                {
                    Console.WriteLine("Erro ao autenticar usuário na DribionPay: " + erroLogin);
                    return;
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var content = new StringContent(JsonConvert.SerializeObject(dadosPix), Encoding.UTF8, "application/json");
                var response = _httpClient.PostAsync("Pix/GerarPix", content).Result;
                if (!response.IsSuccessStatusCode)
                {
                    string erro = "";
                    var errors = JsonConvert.DeserializeObject<List<ModelErrors>>(response.Content.ReadAsStringAsync().Result);
                    foreach (var item in errors)
                    {
                        erro += " " + item.Mensagem;
                    }
                    Console.WriteLine("Erro ao gerar o Pix: " + erro);
                    return;
                }
                else
                {
                    var responseDribionPay = JsonConvert.DeserializeObject<GeraPixRetornoDTO>(response.Content.ReadAsStringAsync().Result);
                    Console.WriteLine("Pix gerado com sucesso!");
                    Console.WriteLine("transaction ID: " + responseDribionPay.transactionId);
                    Console.WriteLine("QRCode: " + responseDribionPay.imageContentPNG);
                    Console.WriteLine("Pix Cola: " + responseDribionPay.pixCola);
                    Console.WriteLine("Valor Cobrado: " + dadosPix.Valor.ToString("C", cultureInfo));
                    Console.WriteLine("Observação: " + responseDribionPay.recipientDescription);
                    Console.WriteLine("Key: " + responseDribionPay.Key);
                }


            }
            catch(Exception ex)
            {
                Console.WriteLine("Erro ao gerar o Pix: " + ex.Message);
                return;
            }
            Console.ReadLine();
        }

        public string AutenticarUsuarioDribionPay(string usuario, string senha, out string erro)
        {
            erro = "";
            var content = new StringContent(JsonConvert.SerializeObject(new { email = usuario, senha = senha }), Encoding.UTF8, "application/json");
            try
            {
                var response = _httpClient.PostAsync("Autenticacao/Login", content).Result;
                var responseBody = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<AutenticarDribionPay>(responseBody).accessToken;
                }
                return "";
            }
            catch (Exception ex)
            {
                erro = "Erro ao autorizar o usuário na DribionPay." + ex.Message;
                return "";
            }
        }

    }
}
