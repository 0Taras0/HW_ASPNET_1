using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSmonder.Constants;

namespace WebSmonder.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = Roles.Admin)]
public class ExtendedUiController : Controller
{
  public IActionResult PerfectScrollbar() => View();
  public IActionResult TextDivider() => View();
}
