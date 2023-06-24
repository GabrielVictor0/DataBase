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
    public class EquipeController : Controller
    {
        private readonly ILogger<EquipeController> _logger;

        public EquipeController(ILogger<EquipeController> logger)
        {
            _logger = logger;
        }

        //instância do contexto para acessar o banco de dados
        Context c = new Context();

        [Route("Listar")] // https://localhost/Equipe/Listar
        public IActionResult Index()
        {

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            //variável que armazena as equipes listadas do banco de dados
            ViewBag.Equipe = c.Equipe.ToList();
            //retorna a view de equipe (TELA)
            return View();
        }

        //Recebendo informações de um form
        [Route("Cadastrar")]
        public IActionResult Cadastrar(IFormCollection form)
        {
            Equipe novaEquipe = new Equipe();

            novaEquipe.Nome = form["Nome"].ToString();

            novaEquipe.Imagem = form["Imagem"].ToString();

            //início da lógica do upload da imagem 
            //criando pasta para guardar os arquivos da img
            if (form.Files.Count > 0)
            {
                var file = form.Files[0];

                //combinação de pasta
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Equipes");

                //se não existir, aqui esta o caminho para criar
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                //gerando caminho completo do arquivo(imagem - nome.extensão)
                var path = Path.Combine(folder, file.FileName);

                //pegando o arquivo da imagem e copiando para a pasta
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                novaEquipe.Imagem = file.FileName;
            }
            else
            {
                // se não existir uma imagem para ser cadastrada, utilizar imagem padrão já cadastrada
                novaEquipe.Imagem = "padrão.png";
            }

            // adicionando nova equipe no banco de dados
            c.Equipe.Add(novaEquipe);

            //salvando mudanças no banco de dados
            c.SaveChanges();

            return LocalRedirect("~/Equipe/Listar");
        }

        [Route("Excluir/{id}")]
        public IActionResult Excluir(int id)
        {
            Equipe equipeBuscada = c.Equipe.First(e => e.IdEquipe == id);

            c.Equipe.Remove(equipeBuscada);

            c.SaveChanges();

            return LocalRedirect("~/Equipe/Listar");
        }

        //método para levar dados de uma tela para outra
        [Route("Editar/{id}")]
        public IActionResult Editar(int id)
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            Equipe equipe = c.Equipe.First(x => x.IdEquipe == id );

            ViewBag.Equipe = equipe;

            return View("Edit");
        }

        [Route("Atualizar")]
        public IActionResult Atualizar(IFormCollection form)
        {
            Equipe equipe = new Equipe();

            equipe.IdEquipe = int.Parse(form["IdEquipe"].ToString());

            equipe.Nome = form["Nome"].ToString();

            if(form.Files.Count > 0)
            {
                var file = form.Files[0];

                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/BadImageFormatException/Equipes");

                if(!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                var path = Path.Combine(folder, file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                equipe.Imagem = file.FileName;
            }   
            else
            {
                equipe.Imagem = "padrão.png";
            }

            Equipe equipeBuscada = c.Equipe.First(x => x.IdEquipe == equipe.IdEquipe);

            equipeBuscada.Nome = equipe.Nome;
            equipeBuscada.Imagem = equipe.Imagem;

            c.Equipe.Update(equipeBuscada);

            c.SaveChanges();

            return LocalRedirect("~/Equipe/Listar");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}