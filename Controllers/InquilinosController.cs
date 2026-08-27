using inmobiliaria_airbnb.Models;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria_airbnb.Controllers
{
    public class InquilinosController : Controller
    {
        private readonly IRepositorioInquilino repositorio;
        private readonly IConfiguration config;
        private readonly ILogger<InquilinosController> logger;

        public InquilinosController(IRepositorioInquilino repo, IConfiguration config, ILogger<InquilinosController> logger)
        {
            this.repositorio = repo;
            this.config = config;
            this.logger = logger;
        }

        //GET: Inquilinos/Index
        public ActionResult Index(int pagina = 1)
        {
            try
            {
                var tamaño = 5;
                var lista = repositorio.ObtenerLista(Math.Max(pagina, 1), tamaño);
                ViewBag.Pagina = pagina;
                var total = repositorio.ObtenerCantidad();
                ViewBag.TotalPaginas = total % tamaño == 0 ? total / tamaño : total / tamaño + 1;
                ViewBag.id = TempData["id"];
                if (TempData.ContainsKey("Mensaje"))
					ViewBag.Mensaje = TempData["Mensaje"];
				return View(lista);
            }   
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Index de inquilinos");
                throw;
            }
        }

        // GET: Inquilinos/Create
		public ActionResult Create()
		{
			try
			{
				return View();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error en Create de inquilinos");
				throw;
			}
		}
        
        // POST: Inquilinos/Create
        [HttpPost]
        public ActionResult Create(Inquilino inquilino)
        {
            try
            {
                repositorio.Alta(inquilino);
                TempData["Id"] = inquilino.IdInquilino;
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error en Create de inquilinos");
                TempData["Error"] = "Error al crear inquilino";
                throw;
            }
        }

        //GET: Inquilinos/Edit
        public ActionResult Edit(int id)
        {
            try
            {
                var entidad = repositorio.ObtenerPorId(id);
                return View(entidad);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Edit de inquilinos");
				throw;
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, Inquilino entidad)
        {
            try
            {
                var i = repositorio.ObtenerPorId(id);
                if(i == null)
                    return NotFound();
                    
                i.Nombre = entidad.Nombre;
				i.Apellido = entidad.Apellido;
				i.Dni = entidad.Dni;
				i.Email = entidad.Email;
				i.Telefono = entidad.Telefono;
				repositorio.Modificacion(i);
				TempData["Mensaje"] = "Inquilino modificado exitosamente";
				return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Edit de inquilinos");
                TempData["Error"] = "Error al editar inquilino";
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
                logger.LogError(ex, "Error en Delete de inquilino");
                throw;
            }
        }

        [HttpPost]
        public ActionResult Delete(int id, Inquilino entidad)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Mensaje"] = "Inquilino eliminado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Delete de inquilino");
                TempData["Error"] = "Error al borrar inquilino";
                throw;
            }
        }
    }
}