using inmobiliaria_airbnb.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria_airbnb.Controllers
{
    public class PropietariosController : Controller
    {
        private readonly IRepositorioPropietario repositorio;
        private readonly IConfiguration config;
        private readonly ILogger<PropietariosController> logger;
        public PropietariosController(IRepositorioPropietario repo, IConfiguration config, ILogger<PropietariosController> logger)
        {
            this.repositorio = repo;
            this.config = config;
            this.logger = logger;
        }
        
        // POST: Propietarios/Create
        public ActionResult Create(Propietario propietario)
        {
            try
            {
                /* if (ModelState.IsValid)
                {
                    
                } */
                repositorio.Alta(propietario);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error en Create de Propietarios");
                throw;
            }
        }
    }
}