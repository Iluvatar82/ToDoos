// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Framework.DomainModels.Common.Enums;
using Framework.Services;
using Framework.Services.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace UI.Web.Areas.Identity.Pages.Account.Manage
{
    [AllowAnonymous]
    public class InvitationModel : PageModel
    {
        internal const ScheduleTimeUnit DefaultReminderUnit = ScheduleTimeUnit.Minute;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly EmailBuilderService _builderService;
        private readonly SignInManager<IdentityUser> _signInManager;

        public InvitationModel(
             UserManager<IdentityUser> userManager,
             IUserStore<IdentityUser> userStore,
             IEmailSender emailSender,
             EmailBuilderService builderService,
             SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _builderService = builderService;
            _signInManager = signInManager;
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

        public async Task<IActionResult> AcceptInvitation(string token, string email)
        {
            var invitedUser = await _userManager.FindByEmailAsync(email);
            var result = Redirect("~/");
            if (invitedUser == null)
                return result;

            var authToken = await _userManager.GetAuthenticationTokenAsync(invitedUser, "[AspNetUserStore]", "Invite");
            if (authToken == null || authToken != token)
                return result;

            return RedirectToPage("./SetPassword", new { email });
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (Request.Query.ContainsKey("token"))
                return await AcceptInvitation(Request.Query["token"], Request.Query["email"]);

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
            var userByEmail = await _userManager.FindByEmailAsync(normalizedEmail);
            if (userByEmail != null)
            {
                StatusMessage = $"Error: Unable to invite user with mail-address '{Input.MailAddress}' because the User already exists.";
                return RedirectToPage();
            }

            await _userManager.CreateAsync(new IdentityUser(Input.MailAddress) {
                Email = Input.MailAddress,
                NormalizedEmail = normalizedEmail
            });

            var newUser = await _userManager.FindByEmailAsync(Input.MailAddress);
            var token = await _userManager.GenerateUserTokenAsync(newUser, TokenOptions.DefaultProvider, "invite");
            await _userManager.SetAuthenticationTokenAsync(newUser, "[AspNetUserStore]", "Invite", token);

            var url = AppResources.Application_Url + Url.Action(null, null, new { token, email = Input.MailAddress });
            await _emailSender.SendEmailAsync(Input.MailAddress, "Einladungs-Mail: todoos.net", _builderService.BuildInvitationMailMessage(url));

            StatusMessage = "Your invitation has been sent to " + Input.MailAddress;
            return RedirectToPage();
        }
    }
}
