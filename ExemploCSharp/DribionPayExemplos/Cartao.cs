using DribionPayExemplos.Model;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DribionPayExemplos
{
    public class Cartao
    {
        private  HttpClient _httpClient;
        public  HttpClient client;


        public Cartao()
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 | (SecurityProtocolType)768;
            _httpClient = new HttpClient();
            client = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.dribionpay.com.br/");
            // Url Produção: https://api.dribionpay.com.br/
            // Url Homologação: https://web.dribion.com/dribionpayapi/

            ProcessarCartao();
        }

        public void ProcessarCartao()
        {
            var cultureInfo = new CultureInfo("pt-BR");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            Console.WriteLine("Gerando um processamento de cartão, via DribionPay...");

            // Quando seu cliente, fazer o pagamento da sua "venda" via cartão de crédito.

            // Informações do usuário e empresa
            var usuario = ""; // Usuario da DribionPay
            var senha = ""; // Senha do usuario
            var empresaKey = ""; //Chave da empresa

            Console.WriteLine("Iniciando a geração do token para o cartão...");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Loadkey();

            var enc = new EncryptingCredentials(key, SecurityAlgorithms.RsaOAEP, SecurityAlgorithms.Aes128CbcHmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "DribionPay",
                Audience = "DribionPay",
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("CardNumber", "5458382364609501"),
                    new Claim("Holder", "Marcos Accept"),
                    new Claim("ExpirationDate", "05/2025"), //MM/yyyy
                    new Claim("SecurityCode", "157")

                }),
                EncryptingCredentials = enc
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            Console.WriteLine($"Key: {enc.Key.KeyId}");
            Console.WriteLine($"Token:\n{tokenHandler.WriteToken(token)}");

            var dadosPagamento = new ProcessaPagamentoDTO()
            {
                CardToken = tokenHandler.WriteToken(token),
                IdPagamento = "1234567890",
                ReferenciaPagamento = "Venda de Produto X",
                Observacao = "Venda de Produto X - Pagamento via Cartão de Crédito",
                Documento = "12345678909", // CPF ou CNPJ do pagador
                Titular = "Marcos Accept",
                Email = "marcosAccept@teste.com.br",
                Telefone = "(43)99999-9999",
                Endereco = "Rua Exemplo",
                EnderecoNumero = "123",
                EnderecoComplemento = "Apto 456",
                EnderecoBairro = "Centro",
                EnderecoCEP = "86300-000", // CEP do pagador
                EnderecoCidade = "Cidade Exemplo",
                EnderecoUF = "PR", // Estado do endereço com 2 letras, por exemplo: PR
                EnderecoPais = "BRA", // País do Endereço com 3 letras, por exemplo: BRA
                Valor = 10.00m, // Valor total da cobrança, com duas casas decimais
                Parcela = 1, // Quantidade de parcelas
                Ambiente = "Homologacao", // Ambiente de operação (Homologacao ou Producao)
                Plataforma = "Cielo", // Plataforma de pagamento (PagSeguro, Cielo, Pagarme, ERede)
                EmpresaKey = empresaKey // Chave da empresa emissora do pagamento
            };

            try
            {
                var accessToken = AutenticarUsuarioDribionPay(usuario, senha, out string erroLogin);
                if (string.IsNullOrEmpty(accessToken))
                {
                    Console.WriteLine("Erro ao autenticar usuário na DribionPay: " + erroLogin);
                    return;
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var content = new StringContent(JsonConvert.SerializeObject(dadosPagamento), Encoding.UTF8, "application/json");
                var response = _httpClient.PostAsync("Cartao/ProcessaPagamento", content).Result;
                if (!response.IsSuccessStatusCode)
                {
                    string erro = "";
                    var errors = JsonConvert.DeserializeObject<List<ModelErrors>>(response.Content.ReadAsStringAsync().Result);
                    foreach (var item in errors)
                    {
                        erro += " " + item.Mensagem;
                    }

                    Console.WriteLine("Erro ao processar o cartão: " + erro);
                    return;
                }
                else
                {
                    var responseDribionPay = JsonConvert.DeserializeObject<ProcessaPagamentoResponseDTO>(response.Content.ReadAsStringAsync().Result);
                    Console.WriteLine("Pagamento processado com sucesso!");
                    Console.WriteLine("Ticket Cartão: " + responseDribionPay.AutorizationCode);
                    Console.WriteLine("Valor Pago: " + dadosPagamento.Valor.ToString("C", cultureInfo));
                    Console.WriteLine("Status: " + responseDribionPay.ReturnMessage);
                    Console.WriteLine("Transação ID: " + responseDribionPay.PaymentId);
                    Console.WriteLine("Código de Autorização: " + responseDribionPay.AutorizationCode);
                    Console.WriteLine("Código de Resposta: " + responseDribionPay.ReturnCode);
                    Console.WriteLine("Mensagem de Retorno: " + responseDribionPay.ReturnMessage);
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine("Erro ao processar o cartão: " + ex.Message);
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

        private SecurityKey Loadkey()
        {
            //var publicKey = await client.GetStringAsync("https://api.dribionpay.com.br/jwks"); // https://web.dribion.com/dribionpayapi/jwks");
            var publicKey = client.GetStringAsync("https://api.dribionpay.com.br/jwks").Result; // https://web.dribion.com/dribionpayapi/jwks");
            var key = JsonWebKeySet.Create(publicKey);
            return key.Keys.FirstOrDefault();
        }


    }
}
