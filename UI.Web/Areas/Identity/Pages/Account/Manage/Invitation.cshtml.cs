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
using Framework.DomainModels.Common.Enums;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace UI.Web.Areas.Identity.Pages.Account.Manage
{
    public class InvitationModel : PageModel
    {
        internal const ScheduleTimeUnit DefaultReminderUnit = ScheduleTimeUnit.Minute;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly IEmailSender _emailSender;

        public InvitationModel(
             UserManager<IdentityUser> userManager,
             IUserStore<IdentityUser> userStore,
             IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _emailSender = emailSender;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }


        public class InputModel
        {
            [Display(Name = "Mail Adresse")]
            public string MailAddress { get; set; }
        }


        private async Task LoadAsync()
        {
            Input = new InputModel
            {
                MailAddress = string.Empty
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadAsync();
                return Page();
            }

            var normalizedEmail = _userManager.NormalizeEmail(Input.MailAddress);
            var userByEmail = await _emailStore.FindByEmailAsync(normalizedEmail, CancellationToken.None);
            if (userByEmail != null)
            {
                StatusMessage = $"Error: Unable to invite user with mail-address '{Input.MailAddress}' because the User already exists.";
                return RedirectToPage();
            }

            var userResult = await _userManager.CreateAsync(new IdentityUser(Input.MailAddress) {  Email = Input.MailAddress, NormalizedEmail = normalizedEmail });
            //TODO: hier Token generieren, LInk für email generieren (oder lassen per Token)
            //Url Action -> Passwort setzen -> fertig -> login!

            await _emailSender.SendEmailAsync(Input.MailAddress, "Sie wurden eingeladen", "Test Einladungs-Mail....");

            StatusMessage = "Your invitation has been sent to " + Input.MailAddress;
            return RedirectToPage();
        }


        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
                throw new NotSupportedException("The default UI requires a user store with email support.");

            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
