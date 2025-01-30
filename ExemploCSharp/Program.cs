using ExemploCSharp.DTO;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;

namespace ExemploCSharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var usuario = "SeuUsuario";
            var senha = "SuaSenha";
            var empresaKey = "SuaEmpresaKey";
            var dribionPayAPI = new DribionPayAPI.DribionPayHelper(usuario, senha, empresaKey,"Homologacao");

            if(!dribionPayAPI.Login())
            {
                Console.WriteLine("Erro ao logar: " + dribionPayAPI.MensagemErro);
                return;
            }

            // Enviar Boleto
            var boleto = new BoletoDTO
            {
                
                dataEmissao = DateTime.Now,
                dataVencimento = DateTime.Now.AddDays(5),
                valorNominal = 1000,
                valorAbatimento = 0,
                seuNumero = "123456",
                diasProtesto = 0,
                diasNegativacao = 0,
                numeroDiasLimiteRecebimento = 0,
                Desconto = new EnviarBoletoDTO_Desconto
                {
                    data = DateTime.Now,
                    taxa = 0,
                    tipo = "Percentual",
                    valor = 0
                },
                Multa = new EnviarBoletoDTO_Multa
                {
                    data = DateTime.Now.AddDays(4),
                    porcentagem = 5,
                    tipo = "Percentual",
                    valor = 0
                },
                Juros = new EnviarBoletoDTO_Juros
                {
                    data = DateTime.Now.AddDays(10),
                    taxa = 10,
                    tipo = "Percentual",
                    valor = 0
                },
                informativos = new string[] { "Informativo 1", "Informativo 2" },
                mensagens = new string[] { "Mensagem 1", "Mensagem 2" },
                Pagador = new EnviarBoletoDTO_Pagador
                {
                    cpfCnpj = "96050176876",
                    nome = "VALERIO DE AGUIAR ZORZATO",
                    endereco = "Av. Alberto Carazzai, 762",
                    cidade = "CORNELIO PROCOPIO",
                    uf = "PR",
                    cep = "86303048",
                    email = "manoel@suaempresa.com",
                    ddd = "43",
                    telefone = "",
                    numero = "15",
                    complemento = "casa",
                    bairro = "Centro"
                },
                banco = "Itau",
                empresaKey = empresaKey
            };

            var boletoResultDTO = dribionPayAPI.EnviarBoleto(boleto);
            if (boletoResultDTO == null)
            {
                Console.WriteLine("Erro ao Enviar Boleto: " + dribionPayAPI.MensagemErro);
                return;
            }
            Console.WriteLine("-- Boleto Enviado --");
            Console.WriteLine($"    Nosso Numero: {boletoResultDTO.nossoNumero}");
            Console.WriteLine($" Linha Digitavel: {boletoResultDTO.linhaDigitavel}");
            Console.WriteLine($"Codigo de Barras: {boletoResultDTO.codigoBarras}");
            Console.WriteLine($"            Link: {boletoResultDTO.link}");
            Console.WriteLine($"             Key: {boletoResultDTO.key}");
            if (!string.IsNullOrEmpty(boletoResultDTO.pdf))
            {
                var boletoPDF = $"{boletoResultDTO.nossoNumero}.pdf";
                if(File.Exists(boletoPDF))
                {
                    File.Delete(boletoPDF);
                }
                var pdf = Convert.FromBase64String(boletoResultDTO.pdf);
                File.WriteAllBytes(boletoPDF, pdf);
            }

            // Consulta Boleto
            var boletoConsulta = dribionPayAPI.ConsultarBoleto(boletoResultDTO.key);
            if (boletoConsulta == null)
            {
                Console.WriteLine("Erro ao Consultar Boleto: " + dribionPayAPI.MensagemErro);
                return;
            }
            Console.WriteLine("-- Consulta Boleto --");
            Console.WriteLine($"    Nosso Numero: {boletoConsulta.nossoNumero}");
            Console.WriteLine($" Linha Digitavel: {boletoConsulta.linhaDigitavel}");
            Console.WriteLine($"Codigo de Barras: {boletoConsulta.codigoBarras}");
            Console.WriteLine($"          Status: {boletoConsulta.status}");
            Console.WriteLine($"           Valor: {boletoConsulta.valor}");
            Console.WriteLine($"      Vencimento: {boletoConsulta.vencimento}");
            Console.WriteLine($"       Pagamento: {boletoConsulta.dataPagamento}");
            Console.WriteLine($"      Valor Pago: {boletoConsulta.valorPago}");
            Console.WriteLine($"  Valor Desconto: {boletoConsulta.valorDesconto}");
            Console.WriteLine($"     Valor Juros: {boletoConsulta.valorJuros}");
            Console.WriteLine($"     Valor Multa: {boletoConsulta.valorMulta}");


            //// Retorna Boletos por Data e Banco
            //var boletosConsulta = dribionPayAPI.RetornarBoletos(DateTime.Today,"Itau");
            //if (boletosConsulta == null)
            //{
            //    Console.WriteLine("Erro ao Consultar Boleto: " + dribionPayAPI.MensagemErro);
            //    return;
            //}
            //Console.WriteLine("-- Retorna Boletos --");
            //foreach (var b in boletosConsulta)
            //{
            //    Console.WriteLine($"    Nosso Numero: {b.nossoNumero}");
            //    Console.WriteLine($" Linha Digitavel: {b.linhaDigitavel}");
            //    Console.WriteLine($"Codigo de Barras: {b.codigoBarras}");
            //    Console.WriteLine($"          Status: {b.status}");
            //    Console.WriteLine($"           Valor: {b.valor}");
            //    Console.WriteLine($"      Vencimento: {b.vencimento}");
            //    Console.WriteLine($"       Pagamento: {b.dataPagamento}");
            //    Console.WriteLine($"      Valor Pago: {b.valorPago}");
            //    Console.WriteLine($"  Valor Desconto: {b.valorDesconto}");
            //    Console.WriteLine($"     Valor Juros: {b.valorJuros}");
            //    Console.WriteLine($"     Valor Multa: {b.valorMulta}");
            //    Console.WriteLine("--");    
            //}

        }
    }
}
