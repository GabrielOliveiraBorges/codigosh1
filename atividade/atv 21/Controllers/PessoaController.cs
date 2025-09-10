using Microsoft.AspNetCore.Mvc;
using ApiPessoas.Models;
using System.Collections.Generic;
using System.Linq;

namespace ApiPessoas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoaController : ControllerBase
    {
        private static List<Pessoa> pessoas = new List<Pessoa>();

        [HttpPost]
        public IActionResult AdicionarPessoa([FromBody] Pessoa pessoa)
        {
            if (pessoas.Any(p => p.Cpf == pessoa.Cpf))
                return BadRequest("CPF já cadastrado.");

            pessoas.Add(pessoa);
            return Ok("Pessoa adicionada com sucesso.");
        }

        [HttpPut("{cpf}")]
        public IActionResult AtualizarPessoa(string cpf, [FromBody] Pessoa novaPessoa)
        {
            var pessoa = pessoas.FirstOrDefault(p => p.Cpf == cpf);
            if (pessoa == null)
                return NotFound("Pessoa não encontrada.");

            pessoa.Nome = novaPessoa.Nome;
            pessoa.Peso = novaPessoa.Peso;
            pessoa.Altura = novaPessoa.Altura;

            return Ok("Pessoa atualizada com sucesso.");
        }

        [HttpDelete("{cpf}")]
        public IActionResult RemoverPessoa(string cpf)
        {
            var pessoa = pessoas.FirstOrDefault(p => p.Cpf == cpf);
            if (pessoa == null)
                return NotFound("Pessoa não encontrada.");

            pessoas.Remove(pessoa);
            return Ok("Pessoa removida com sucesso.");
        }


        [HttpGet]
        public IActionResult BuscarTodas()
        {
            return Ok(pessoas);
        }


        [HttpGet("{cpf}")]
        public IActionResult BuscarPorCpf(string cpf)
        {
            var pessoa = pessoas.FirstOrDefault(p => p.Cpf == cpf);
            if (pessoa == null)
                return NotFound("Pessoa não encontrada.");

            return Ok(pessoa);
        }

        [HttpGet("imc/bom")]
        public IActionResult BuscarComImcBom()
        {
            var resultado = pessoas.Where(p =>
            {
                double imc = p.CalcularImc();
                return imc >= 18 && imc <= 24;
            }).ToList();

            return Ok(resultado);
        }


        [HttpGet("buscar")]
        public IActionResult BuscarPorNome([FromQuery] string nome)
        {
            var resultado = pessoas
                .Where(p => p.Nome.Contains(nome, System.StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(resultado);
        }
    }
}