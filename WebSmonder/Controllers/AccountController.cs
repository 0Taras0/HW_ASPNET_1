using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebSmonder.Constants;
using WebSmonder.Data.Entities.Identity;
using WebSmonder.Interfaces;
using WebSmonder.Models.Account;
using WebSmonder.SMTP;

namespace WebSmonder.Controllers
{
    public class AccountController(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, IMapper mapper, IImageService imageService, ISMTPService smtpService) : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var res = await signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (res.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return Redirect("/");
                }
            }
            ModelState.AddModelError("", "Дані вказані не вірно");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Redirect("/");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = mapper.Map<UserEntity>(model);
            user.Image = await imageService.SaveImageAsync(model.Image);

            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                await userManager.AddToRoleAsync(user, Roles.User);
                return Redirect("/");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var user = userManager.GetUserAsync(User).Result;
            var model = mapper.Map<AccountProfileViewModel>(user);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(AccountProfileViewModel model)
        {
            ModelState.Remove(nameof(model.Image));
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            string image = user.Image;
            user = mapper.Map(model, user);
            user.Image = image;
            if (model.Image != null)
            {
                await imageService.DeleteImageAsync(user.Image);
                user.Image = await imageService.SaveImageAsync(model.Image);
            }
            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Profile");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Користувача з таким email не знайдено");
                return View(model);
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var resetUrl = Url.Action("ResetPassword", "Account", new { email = user.Email, token }, Request.Scheme);

            var message = new Message
            {
                Subject = "Скидання паролю",
                To = user.Email,
                Body = $"Для скидання паролю перейдіть за посиланням: <a href='{resetUrl}'>Скинути</a>"
            };

            var result = await smtpService.SendMessageAsync(message);

            if (!result)
            {
                ModelState.AddModelError("", "Не вдалося надіслати листа. Спробуйте ще раз");
                return View(model);
            }

            return RedirectToAction(nameof(ForgotPasswordSend));
        }

        //Відновлення паролю. Надіслали лист
        [HttpGet]
        public IActionResult ForgotPasswordSend()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Невірне посилання для скидання пароля.");
            }

            var model = new ResetPasswordViewModel
            {
                Email = email,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Користувача не знайдено.");
                return View(model);
            }

            var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

    }
}
