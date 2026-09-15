using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria_airbnb.Services
{
	public static class ControllerBaseExtensions
	{
		/// <summary>
		/// Id del usuario autenticado, tomado del claim NameIdentifier.
		/// Como el JWT se configura con NameClaimType = NameIdentifier,
		/// User.Identity.Name devuelve este mismo valor; se usa FindFirstValue
		/// para no depender de esa configuración y poder validar el parseo.
		/// Devuelve 0 si no hay claim o no es numérico.
		/// </summary>
		public static int UsuarioId(this ControllerBase controllerBase)
		{
			var valor = controllerBase.User.FindFirstValue(ClaimTypes.NameIdentifier);
			return int.TryParse(valor, out var id) ? id : 0;
		}
    }
}