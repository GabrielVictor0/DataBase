using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Gamer_BancoDeDados.Infra;
using Gamer_BancoDeDados.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Gamer_BancoDeDados.Controllers
{
    [Route("[controller]")]
    public class JogadorController : Controller
    {
        private readonly ILogger<JogadorController> _logger;

        public JogadorController(ILogger<JogadorController> logger)
        {
            _logger = logger;
        }

        //OBJETO C PARA OBTER ACESSO AO BANCO DE DADOS
        Context c = new Context();

        [Route("Listar")]
        public IActionResult Index()
        {
            //nova mochila para guardar informações 
            ViewBag.Jogador = c.Jogador.ToList();
            ViewBag.Equipe = c.Equipe.ToList();
            return View();
        }

        public IActionResult Cadastrar(IFormCollection form)
        {
            Jogador novoJogador = new Jogador();

            novoJogador.Nome = form["Nome"].ToString();

            novoJogador.Email = form["Email"].ToString();

            return LocalRedirect("~/Jogador/Listar");

        }
    

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}