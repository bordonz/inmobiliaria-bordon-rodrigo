using inmobiliaria_airbnb.Models;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria_airbnb.Controllers
{
    public class ReservasController : Controller
    {
        private readonly IRepositorioReserva repositorio;
        private readonly IConfiguration config;
        private readonly ILogger<ReservasController> logger;

        public ReservasController(IRepositorioReserva repo, IConfiguration config, ILogger<ReservasController> logger)
        {
            this.repositorio = repo;
            this.config = config;
            this.logger = logger;
        }

    //GET: Reservas/index
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
                logger.LogError(ex, "Error en index de Reservas");
                throw;
            }
        }

        //GET: Reservas/Create
        public ActionResult Create()
        {
            try
            {
                if (TempData.ContainsKey("Error"))
                {
                    ViewBag.Mensaje = TempData["Error"];
                }
                return View();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Create de Reserva");
                throw;
            }
        }

        [HttpPost]
        public ActionResult Create(Reserva r)
        {
            try
            {
                repositorio.Alta(r);
                TempData["Id"] = r.IdReserva;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Create de Reservas");
                TempData["Error"] = "Error al crear Reserva";
                return RedirectToAction(nameof(Create));
            }
        }

        //GET: Reservas/Edit
        public ActionResult Edit(int id)
        {
            try
            {
                var entidad = repositorio.ObtenerPorId(id);
                if (TempData.ContainsKey("Error"))
                {
                    ViewBag.Mensaje = TempData["Error"];
                }
                return View(entidad);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Edit de Reservas");
                throw;
            }
        }

        //POST: Reservas/Edit
        [HttpPost]
        public ActionResult Edit(int id, Reserva reserva)
        {
            try
            {
                var r = repositorio.ObtenerPorId(id);
                if(r == null)
                {
                    return NotFound();
                }
                r.Estado = reserva.Estado;
                r.Monto = reserva.Monto;
                r.FechaDesde = reserva.FechaDesde;
                r.FechaHasta = reserva.FechaHasta;
                r.InmuebleId = reserva.InmuebleId;
                r.InquilinoId = reserva.InquilinoId;
                repositorio.Modificacion(r);
                TempData["Mensaje"] = "Reserva editada exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Delete de Reservas");
                TempData["Error"] = "Error al editar Reserva";
                return RedirectToAction(nameof(Edit));
            }
        }

        //GET: Reservas/Delete
        public ActionResult Delete(int id)
        {
            try
            {
                var entidad = repositorio.ObtenerPorId(id);
                if (TempData.ContainsKey("Error"))
                {
                    ViewBag.Mensaje = TempData["Error"];
                }
                return View(entidad);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Delete de Reservas");
                throw;
            }
        }

        //POST: Reservas
        [HttpPost]
        public ActionResult Delete(int id, Reserva reserva)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Mensaje"] = "Reserva eliminado exitosamente";
                return RedirectToAction(nameof(Delete));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Delete de Reservas");
                TempData["Error"] = "Error al borrar reserva";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}