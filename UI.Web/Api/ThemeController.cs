using Framework.Services;
using Microsoft.AspNetCore.Mvc;

namespace UI.Web.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ThemeController : ControllerBase
    {
        private readonly ThemeService _themeService;
        public ThemeController(ThemeService themeService)
        {
            _themeService = themeService;
        }

        [HttpPost]
        public void Set()
        {
            if (!Request.Form.ContainsKey("isDark"))
                return;

            var isDarkCollection = Request.Form["isDark"];
            if (bool.TryParse(isDarkCollection[0], out var isDark))
                _themeService.SetIsDark(isDark);
        }
    }
}
