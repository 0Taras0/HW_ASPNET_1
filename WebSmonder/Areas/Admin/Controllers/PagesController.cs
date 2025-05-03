using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSmonder.Constants;

namespace WebSmonder.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = Roles.Admin)]
public class PagesController : Controller
{
  public IActionResult AccountSettings() => View();
  public IActionResult AccountSettingsConnections() => View();
  public IActionResult AccountSettingsNotifications() => View();
  public IActionResult MiscError() => View();
  public IActionResult MiscUnderMaintenance() => View();
}
