using inmobiliaria_airbnb;
using inmobiliaria_airbnb.Models;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria.Controllers;

public class TiposInmuebleController : Controller
{
    private readonly IRepositorioTiposInmuebles repositorio;
    private readonly IConfiguration config;
    private readonly ILogger<TiposInmuebleController> logger;

    public TiposInmuebleController(IRepositorioTiposInmuebles repo, IConfiguration config, ILogger<TiposInmuebleController> logger)
    {
        this.repositorio = repo;
        this.config = config;
        this.logger = logger;
    }

    // GET: TiposInmuebles/Index
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
				logger.LogError(ex, "Error en Index de TipoInmuble");
				throw;
			}
		}

        // GET: TipoInmueble/Create
		public ActionResult Create()
		{
			try
			{
				return View();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error en Create de TipoInmueble");
				throw;
			}
		}
        
        // POST: TipoInmueble/Create
        [HttpPost]
        public ActionResult Create(TipoInmueble tipo)
        {
            try
            {
                repositorio.Alta(tipo);
                TempData["Id"] = tipo.IdTipoInmueble;
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error en Create de TipoInmueble");
                TempData["Error"] = "Error al crear el tipo de inmueble";
                throw;
            }
        }

        //GET: TiposInmuebles/Edit
        public ActionResult Edit(int id)
        {
            try
            {
                var entidad = repositorio.ObtenerPorId(id);
                return View(entidad);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Edit de TipoInmueble");
				throw;
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, TipoInmueble entidad)
        {
            try
            {
                var t = repositorio.ObtenerPorId(id);
                if(t == null)
                    return NotFound();
                    
                t.Descripcion = entidad.Descripcion;
				repositorio.Modificacion(t);
				TempData["Mensaje"] = "Tipo de inmueble modificado exitosamente";
				return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Edit de TipoInmuble");
                TempData["Error"] = "Error al editar tipo de inmueble";
				throw;
            }
        }

        //GET: TiposInmubles/Delete/id
        public ActionResult Delete(int id)
        {
            try
            {
                var entidad = repositorio.ObtenerPorId(id);
                return View(entidad);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Delete de TipoInmuebles");
                throw;
            }
        }

        [HttpPost]
        public ActionResult Delete(int id, TipoInmueble entidad)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Mensaje"] = "tipo de inmueble eliminado correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Delete de TipoInmueble");
                TempData["Error"] = "Error al borrar tipo de inmueble";
                throw;
            }
        }
}