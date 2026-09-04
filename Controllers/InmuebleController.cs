using inmobiliaria_airbnb.Models;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria_airbnb.Controllers
{
    public class InmueblesController : Controller
    {
        private readonly IRepositorioInmueble repositorio;
        private readonly IConfiguration config;
        private readonly ILogger<InmueblesController> logger;

        public InmueblesController(IRepositorioInmueble repo, IConfiguration config, ILogger<InmueblesController> logger)
        {
            this.repositorio = repo;
            this.config = config;
            this.logger = logger;
        }

        //GET: Inmuebles/index
        public ActionResult Index(int pagina = 1)
        {
            try
            {
                var tamaño = 5;
                var lista = repositorio.ObtenerLista(Math.Max(pagina, 1), tamaño);
                ViewBag.pagina = pagina;
                var total = repositorio.ObtenerCantidad();
                ViewBag.TotalPaginas = total % tamaño == 0 ? total / tamaño : total / tamaño +1;
                ViewBag.id = TempData["id"];

                if (TempData.ContainsKey("Mensaje"))
                {
                    ViewBag.Mensaje = TempData["Mensaje"];
                }
                return View(lista);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Index de Inmuebles");
                throw;
            }
        }

        //GET: Inmuebles/Create
        public ActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Create de Inmuebles");
                throw;
            }
        }

        [HttpPost]
        public ActionResult Create(Inmueble i)
        {
            try
            {
                repositorio.Alta(i);
                TempData["Id"] = i.IdInmueble;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Create de Inmuebles");
                TempData["Error"] = "Error al crear inmueble";
                throw;
            }
        }

        //GET: Inmuebles/Edit
        public ActionResult Edit(int id)
        {
            try
            {
                var entidad = repositorio.ObtenerPorId(id);
                return View(entidad);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Edit de Inquilinos");
                throw;
            }
        }

        //POST: Inmuebles/Edit
        [HttpPost]
        public ActionResult Edit(int id, Inmueble inmueble)
        {
            try
            {
                var i = repositorio.ObtenerPorId(id);
                if(i == null)
                {
                    return NotFound();
                }
                i.Direccion = inmueble.Direccion;
                i.Cupo = inmueble.Cupo;
                i.PrecioPorDia = inmueble.PrecioPorDia;
                i.PorcentajeReserva = inmueble.PorcentajeReserva;
                i.Latitud = inmueble.Latitud;
                i.Longitud = inmueble.Longitud;
                i.Tipo = inmueble.Tipo;
                i.PropietarioId = inmueble.PropietarioId;
                i.Habilitado = inmueble.Habilitado;
                repositorio.Modificacion(i);
                TempData["Mensaje"] = "Inmueble editado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Edit de Inmuebles");
                TempData["Error"] = "Error al editar inmueble";
                throw;
            }
        }

        //GET: Inmuebles/Delete
        public ActionResult Delete(int id)
        {
            try
            {
                var entidad = repositorio.ObtenerPorId(id);
                return View(entidad);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Delete de Inmuebles");
                throw;
            }
        }

        //POST: Inmuebles/Delete
        [HttpPost]
        public ActionResult Delete(int id, Inquilino entidad)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Mensaje"] = "Inmueble eliminado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Delete de Inmuebles");
                TempData["Error"] = "Error al borrar inmueble";
                throw;
            }
        }

        // GET: Inmuebles/Imagenes/5
		public ActionResult Imagenes(int id, [FromServices] IRepositorioImagen repoImagen)
		{
			var entidad = repositorio.ObtenerPorId(id);
			if (entidad == null)
				return NotFound();
			entidad.Imagenes = repoImagen.BuscarPorInmueble(id);
			return View(entidad);
		}

		// POST: Inmuebles/Portada
		[HttpPost]
		public ActionResult Portada(Imagen entidad, [FromServices] IWebHostEnvironment environment)
		{
			try
			{
				//Recuperar el inmueble y eliminar la imagen anterior
				var inmueble = repositorio.ObtenerPorId(entidad.InmuebleId);
				if (inmueble != null && inmueble.Portada != null)
				{
					string rutaEliminar = Path.Combine(environment.WebRootPath, "Uploads", "Inmuebles", Path.GetFileName(inmueble.Portada));
					System.IO.File.Delete(rutaEliminar);
				}
				if (entidad.Archivo != null)
				{
					string wwwPath = environment.WebRootPath;
					string path = Path.Combine(wwwPath, "Uploads");
					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}
					path = Path.Combine(path, "Inmuebles");
					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}
					//string fileName = Path.GetFileName(entidad.Archivo.FileName);//este nombre se puede repetir
					string fileName = "portada_" + entidad.InmuebleId + Path.GetExtension(entidad.Archivo.FileName);
					string rutaFisicaCompleta = Path.Combine(path, fileName);
					using (var stream = new FileStream(rutaFisicaCompleta, FileMode.Create))
					{
						entidad.Archivo.CopyTo(stream);
					}
					entidad.Url = Path.Combine("/Uploads/Inmuebles", fileName);
				}
				else //sin imagen
				{
					entidad.Url = string.Empty;
				}
				repositorio.ModificarPortada(entidad.InmuebleId, entidad.Url);
				TempData["Mensaje"] = "Portada actualizada correctamente";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				TempData["Error"] = ex.Message;
				return RedirectToAction(nameof(Imagenes), new { id = entidad.InmuebleId });
			}
		}

        [HttpGet]
        public ActionResult PorPropietario(int id, int pagina = 1)
        {
            try
            {
                var tamaño = 5;
                var lista = repositorio.BuscarPorPropietario(id, Math.Max(pagina, 1), tamaño);
                ViewBag.pagina = pagina;
                var total = repositorio.ObtenerCantidadPorPropietario(id);
                ViewBag.TotalPaginas = total % tamaño == 0 ? total / tamaño : total / tamaño +1;

                ViewBag.PropietarioId = id;
                
                if (TempData.ContainsKey("Mensaje"))
                {
                    ViewBag.Mensaje = TempData["Mensaje"];
                }
                return View(lista);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en PorPropietario de Inmuebles");
                TempData["Error"] = "Error al buscar inmuebles por dueño";
                throw;
            }            
        }
    }
}