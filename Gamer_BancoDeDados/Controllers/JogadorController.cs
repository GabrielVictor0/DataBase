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
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            //nova mochila para guardar informações 
            ViewBag.Jogador = c.Jogador.ToList();
            ViewBag.Equipe = c.Equipe.ToList();
            return View();
        }

        [Route("Cadastrar")]
        public IActionResult Cadastrar(IFormCollection form)
        {
            Jogador novoJogador = new Jogador();
            
            //recebendo o que é digitado no formulário
            novoJogador.Nome = form["Nome"].ToString();

            novoJogador.Email = form["Email"].ToString();

            novoJogador.Senha = form["Senha"].ToString();

            novoJogador.IdEquipe = int.Parse(form["IdEquipe"].ToString()); 
            
            c.Jogador.Add(novoJogador);

            c.SaveChanges();

            return LocalRedirect("~/Jogador/Listar");
        }

        [Route("Excluir/{id}")]
        public IActionResult Excluir(int id)
        {
            Jogador jogadorBuscado = c.Jogador.First(j => j.IdJogador == id);

            c.Jogador.Remove(jogadorBuscado);

            c.SaveChanges();

            return LocalRedirect("~/Jogador/Listar");
        }

        [Route("Editar/{id}")]
        public IActionResult Editar(int id)
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            Jogador jogador = c.Jogador.First(x => x.IdJogador == id);

            ViewBag.Jogador = jogador;
            ViewBag.Equipe = c.Equipe.ToList();

            return View("Edit");
        }

        [Route("Atualizar")]

        public IActionResult Atualizar(IFormCollection form)
        {
            Jogador jogador = new Jogador();

            jogador.IdJogador = int.Parse(form["IdJogador"].ToString());

            jogador.IdEquipe = int.Parse(form["IdEquipe"].ToString());

            jogador.Nome = form["Nome"].ToString();

            jogador.Email = form["Email"].ToString();

            Jogador jogadorBuscado = c.Jogador.First(x => x.IdJogador == jogador.IdJogador);

            jogadorBuscado.Nome = jogador.Nome;
            jogadorBuscado.IdEquipe = jogador.IdEquipe;
            jogadorBuscado.Email = jogador.Email;

            c.Jogador.Update(jogadorBuscado);

            c.SaveChanges();

            return LocalRedirect("~/Jogador/Listar");
        }
    

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}