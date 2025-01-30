using ExemploCSharp.DTO;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace DribionPayAPI
{
    public class DribionPayHelper
    {
        private readonly string _urlAPI = "https://web.dribion.com/dribionpayapi/";
        private readonly string _email;
        private readonly string _senha;
        private readonly string _empresaKey;
        private readonly string _ambiente;
        private string _tokenApi;
        private readonly HttpClient _client;
        public string MensagemErro;

        public DribionPayHelper(string email, string senha, string empresaKey, string ambiente)
        {
            _email = email;
            _senha = senha;
            _empresaKey = empresaKey;
            _ambiente = ambiente;
            _client = new HttpClient(); // em um ambiente de produção, é recomendado usar um HttpClientFactory
            _client.BaseAddress = new Uri(_urlAPI);
        }

        public bool Login()
        {
            var loginBody = new
            {
                Email = _email,
                Senha = _senha
            };
            MensagemErro = "";
            var loginRequest = new StringContent(JsonConvert.SerializeObject(loginBody), Encoding.UTF8, "application/json");
            var loginResult = _client.PostAsync("autenticacao/login", loginRequest).GetAwaiter().GetResult();
            if (!loginResult.IsSuccessStatusCode)
            {
                MensagemErro = "Erro ao logar: " + loginResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return false;
            }
            var loginResultDTO = JsonConvert.DeserializeObject<LoginResultDTO>(loginResult.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            _tokenApi = loginResultDTO.accessToken;
            return true;
        }

        public BoletoEnvioResultDTO EnviarBoleto(BoletoDTO boleto)
        {
            boleto.ambiente = _ambiente;
            MensagemErro = "";
            var boletoRequest = new StringContent(JsonConvert.SerializeObject(boleto), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenApi);
            var boletoResult = _client.PostAsync("Cobranca/EnviarBoletos", boletoRequest).GetAwaiter().GetResult();
            if (!boletoResult.IsSuccessStatusCode)
            {
                MensagemErro = "Erro ao enviar boleto: " + boletoResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return null;
            }
            return JsonConvert.DeserializeObject<BoletoEnvioResultDTO>(boletoResult.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }

        public BoletoConsultaResultDTO ConsultarBoleto(string key)
        {
            MensagemErro = "";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenApi);
            var boletoResult = _client.GetAsync("Cobranca/ConsultaBoleto?key=" + key).GetAwaiter().GetResult();
            if (!boletoResult.IsSuccessStatusCode)
            {
                MensagemErro = "Erro ao consultar boleto: " + boletoResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return null;
            }
            return JsonConvert.DeserializeObject<BoletoConsultaResultDTO>(boletoResult.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }

        public List<BoletoConsultaResultDTO> RetornarBoletos(DateTime data, string banco)
        {
            // TODO: Implementar a consulta de boletos por data e banco
            return null;

            var retornaBoletosBody = new
            {
                Data = data,
                Banco = banco
            };
            MensagemErro = "";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenApi);
            var boletoRequest = new StringContent(JsonConvert.SerializeObject(retornaBoletosBody), Encoding.UTF8, "application/json");
            var boletoResult = _client.PostAsync("Cobranca/RetornarBoleto", boletoRequest).GetAwaiter().GetResult();
            if (!boletoResult.IsSuccessStatusCode)
            {
                MensagemErro = "Erro ao consultar boletos: " + boletoResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return null;
            }
            return JsonConvert.DeserializeObject<List<BoletoConsultaResultDTO>>(boletoResult.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }
    }
}
