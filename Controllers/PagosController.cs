using inmobiliaria_airbnb.Models;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria_airbnb.Controllers
{
    public class PagosController : Controller
    {
        private readonly IRepositorioPago repositorio;
        private readonly IConfiguration config;
        private readonly ILogger<PagosController> logger;

        public PagosController(IRepositorioPago repo, IConfiguration config, ILogger<PagosController> logger)
        {
            this.repositorio = repo;
            this.config = config;
            this.logger = logger;
        }

        //GET: Pagos/Index
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
                logger.LogError(ex, "Error en index de Pagos");
                throw;
            }
        }

        //GET: Pagos/Create
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
                logger.LogError(ex, "Error en Create de Pagos");
                throw;
            }
        }

        //POST: Pagos/Create
        [HttpPost]
        public ActionResult Create(Pago p)
        {
            try
            {
                repositorio.Alta(p);
                TempData["Id"] = p.IdPago;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Create de Pagos");
                TempData["Error"] = "Error al crear Pagos";
                return RedirectToAction(nameof(Create));
            }
        }

        //GET: Pagos/Edit
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
                logger.LogError(ex, "Error en Edit de PagosController");
                throw;
            }
        }

        //POST: Pagos/Edit
        [HttpPost]
        public ActionResult Edit(int id, Pago pago)
        {
            try
            {
                var p = repositorio.ObtenerPorId(id);
                if(p == null)
                {
                    return NotFound();
                }
                p.Concepto = pago.Concepto;
                repositorio.Modificacion(p);
                TempData["Mensaje"] = "Concepto de pago editado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Edit de PagosController");
                TempData["Error"] = "Error al editar el concepto del pago";
                return RedirectToAction(nameof(Edit));
            }
        }

        //GET: Pagos/Delete
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
                logger.LogError(ex, "Error en Delete de PagosController");
                throw;
            }
        }

        //POST: Pagos/Delete
        [HttpPost]
        public ActionResult Delete(int id, Pago pago)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Mensaje"] = "Cambio de estado del pago exitoso";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en Delete de PagosController");
                TempData["Error"] = "Error al cambiar estado de pago";
                return RedirectToAction(nameof(Delete));
            }
        }
    }
}