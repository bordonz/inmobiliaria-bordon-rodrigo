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

        // GET: Propietarios/Index
		public ActionResult Index(int pagina=1)
		{
			try
			{
				var tamaño = 5;
				var lista = repositorio.ObtenerLista(Math.Max(pagina, 1), tamaño);
				ViewBag.Pagina = pagina;
				var total = repositorio.ObtenerCantidad();
				ViewBag.TotalPaginas = total % tamaño == 0 ? total / tamaño : total / tamaño + 1;
				ViewBag.Id = TempData["Id"];
				if (TempData.ContainsKey("Mensaje"))
					ViewBag.Mensaje = TempData["Mensaje"];
				return View(lista);
			}
			catch (Exception ex)
			{// Poner breakpoints para detectar errores
				logger.LogError(ex, "Error en Index de Propietarios");
				throw;
			}
		}

        // GET: Propietarios/Create
		public ActionResult Create()
		{
			try
			{
				return View();
			}
			catch (Exception ex)
			{//poner breakpoints para detectar errores
				logger.LogError(ex, "Error en Create de Propietarios");
				throw;
			}
		}
        
        // POST: Propietarios/Create
        [HttpPost]
        public ActionResult Create(Propietario propietario)
        {
            try
            {
                repositorio.Alta(propietario);
                TempData["Id"] = propietario.IdPropietario;
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error en Create de Propietarios");
                TempData["Error"] = "Error al crear el propietario";
                throw;
            }
        }

        //GET: Propietarios/Edit
        public ActionResult Edit(int id)
        {
            try
            {
                var entidad = repositorio.ObtenerPorId(id);
                return View(entidad);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Create de Propietarios");
				throw;
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, Propietario entidad)
        {
            try
            {
                var p = repositorio.ObtenerPorId(id);
                if(p == null)
                    return NotFound();
                    
                p.Nombre = entidad.Nombre;
				p.Apellido = entidad.Apellido;
				p.Dni = entidad.Dni;
				p.Email = entidad.Email;
				p.Telefono = entidad.Telefono;
				repositorio.Modificacion(p);
				TempData["Mensaje"] = "Propietario modificado exitosamente";
				return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Edit de Propietarios");
                TempData["Error"] = "Error al editar propietario";
				throw;
            }
        }

        //GET: Propietarios/Delete/id
        public ActionResult Delete(int id)
        {
            try
            {
                var entidad = repositorio.ObtenerPorId(id);
                return View(entidad);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Delete de Propietarios");
                throw;
            }
        }

        [HttpPost]
        public ActionResult Delete(int id, Propietario entidad)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Mensaje"] = "Propietario eliminado correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Delete de Propietarios");
                TempData["Error"] = "Error al borrar propietario";
                throw;
            }
        }

        //GET: Propietarios/Buscar/5
        [Route("[controller]/Buscar/{q}", Name = "Buscar")]
        public IActionResult Buscar(string q)
        {
            try
            {
                var res = repositorio.BuscarPorNombre(q);
                return Json( new { datos = res });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
    }
}