// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Core.Common;
using Framework.Converter;
using Framework.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace UI.Web.Areas.Identity.Pages.Account.Manage
{
    public class SettingsModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SettingRepository _settingRepository;

        public SettingsModel(
             UserManager<IdentityUser> userManager,
             SettingRepository settingRepository)
        {
            _userManager = userManager;
            _settingRepository = settingRepository;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }


        public class InputModel
        {
            [Display(Name = "Two Columns")]
            public bool TwoColumns { get; set; }
        }


        private async Task LoadAsync(IdentityUser user)
        {
            var settings = await _settingRepository.GetAllSettingsAsync(Guid.Parse(user.Id));

            Input = new InputModel
            {
                TwoColumns = (settings.FirstOrDefault(s => s.Key == Settings.TwoColumns)?.Value ?? "false").GetBool()
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var userIdGuid = Guid.Parse(user.Id);
            var twoColumnSetting = await _settingRepository.GetSettingAsync(userIdGuid, Settings.TwoColumns);
            if (twoColumnSetting == null)
            {
                twoColumnSetting = _settingRepository.Create(userIdGuid, Settings.TwoColumns, Input.TwoColumns.ToString());
                await _settingRepository.AddAndSaveAsync(twoColumnSetting);
            }
            else
            {
                twoColumnSetting.Value = Input.TwoColumns.ToString();
                await _settingRepository.UpdateAndSaveAsync(twoColumnSetting);
            }

            StatusMessage = "Your settings have been updated";
            return RedirectToPage();
        }
    }
}
