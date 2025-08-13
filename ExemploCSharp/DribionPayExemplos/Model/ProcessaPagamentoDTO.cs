using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DribionPayExemplos.Model
{
    public class ProcessaPagamentoDTO
    {
        public string CardToken { get; set; } //Token do cartão gerado para pagamento
        public string IdPagamento { get; set; } //Um Id para referência interna da cobrança
        public string ReferenciaPagamento { get; set; } // Comentário sobre a referência interna da cobrança
        public string Observacao { get; set; } // Observação sobre a referência interna da cobrança
        public string Documento { get; set; } // Somente os números do CPF ou CNPJ do pagador
        public string Email { get; set; } // E-mail do pagador
        public string Telefone { get; set; } // Telefone do pagador, com o codido de área (043)99999-9999
        public string Endereco { get; set; } // Endereço do pagador
        public string EnderecoNumero { get; set; } // Número do endereço do pagador
        public string EnderecoComplemento { get; set; } // Complemento do endereço do pagador
        public string EnderecoBairro { get; set; } // Bairro do endereço do pagador
        public string EnderecoCEP { get; set; } // CEP do endereço do pagador
        public string EnderecoCidade { get; set; } // Cidade do endereço do pagador
        public string EnderecoUF { get; set; } // Estado do endereço com 2 letras, por exemplo: PR
        public string EnderecoPais { get; set; } // País do Endereço com 3 letras, por exemplo: BRA
        public decimal Valor { get; set; } // Valor total da cobrança, com duas casas decimais
        public string Titular { get; set; } // Nome do titular do cartão
        public int Parcela { get; set; } // Quantidade de parcelas
        public string Ambiente { get; set; } // Ambiente de operação (Homologacao ou Producao)
        public string Plataforma { get; set; } // Plataforma de pagamento (PagSeguro, Cielo, Pagarme, ERede)
        public string EmpresaKey { get; set; }// Chave da empresa emissora do pagamento
    }
}
