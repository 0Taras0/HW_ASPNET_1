using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSmonder.Constants;

namespace WebSmonder.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = Roles.Admin)]
public class FormLayoutsController : Controller
{
public IActionResult Horizontal() => View();
public IActionResult Vertical() => View();
}
