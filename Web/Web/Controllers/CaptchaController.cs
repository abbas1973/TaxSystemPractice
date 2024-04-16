using Microsoft.AspNetCore.Mvc;
using SixLaborsCaptcha.Core;
using Application.SessionServices;

namespace Web.Controllers
{
    /// <summary>
    /// کد امنیتی Captcha
    /// </summary>
    public class CaptchaController : Controller
    {
        public ActionResult CaptchaImage(string prefix, bool noisy = true)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            string captcha = rand.Next(10000, 99999).ToString();

            #region ذخیره در سشن
            HttpContext.Session.RemoveCaptcha();
            HttpContext.Session.SetCaptcha(captcha);
            #endregion

            #region تولید کپچا
            var slc = new SixLaborsCaptchaModule(new SixLaborsCaptchaOptions
            {
                DrawLines = 4,
                NoiseRate = 50,
                //TextColor = new SixLabors.ImageSharp.Color[] { SixLabors.ImageSharp.Color.Red },
            });
            var result = slc.Generate(captcha);
            return File(result, "Image/Png");
            #endregion
        }
    }
}
