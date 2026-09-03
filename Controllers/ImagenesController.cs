using inmobiliaria_airbnb.Models;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria_airbnb.Controllers
{
	public class ImagenController : Controller
	{
		private readonly IRepositorioImagen repositorio;

		public ImagenController(IRepositorioImagen repositorio)
		{
			this.repositorio = repositorio;
		}
		[HttpPost]
		[Route("Imagenes/Alta/{id}")] 
		public async Task<IActionResult> Alta(int id, List<IFormFile> imagenes, [FromServices] IWebHostEnvironment environment)
		{
			if (imagenes == null || imagenes.Count == 0)
				return BadRequest("No se recibieron archivos.");
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
			path = Path.Combine(path, id.ToString());
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			foreach (var file in imagenes)
			{
				if (file.Length > 0)
				{
					var extension = Path.GetExtension(file.FileName);
					var nombreArchivo = $"{Guid.NewGuid()}{extension}";
					var rutaArchivo = Path.Combine(path, nombreArchivo);

					using (var stream = new FileStream(rutaArchivo, FileMode.Create))
					{
						await file.CopyToAsync(stream);
					}
					Imagen imagen = new Imagen
					{
						InmuebleId = id,
						Url = $"/Uploads/Inmuebles/{id}/{nombreArchivo}",
					};
					repositorio.Alta(imagen);
				}
			}
			return Ok(repositorio.BuscarPorInmueble(id));
		}

		//TODO: La primera vez toma el id, la seguna vez no y 400
		// POST: Inmueble/Eliminar/5
		[HttpPost]
		[Route("Imagenes/Eliminar/{id}")] 
		public ActionResult Eliminar(int id, [FromServices] IWebHostEnvironment environment)
		{
			try
			{
				//TODO: Eliminar el archivo físico
				var entidad = repositorio.ObtenerPorId(id);
				if(entidad != null)
				{
					string rutaEliminar = Path.Combine(environment.WebRootPath, entidad.Url.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
					if(System.IO.File.Exists(rutaEliminar))
					{
						System.IO.File.Delete(rutaEliminar);					
					}
				}
				repositorio.Baja(id);
				return Ok(repositorio.BuscarPorInmueble(entidad.InmuebleId));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}