// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using Microsoft.Extensions.Hosting;

namespace Nozomi.Auth.Controllers.Account
{
    public class AccountOptions
    {
        public static bool AllowLocalLogin = true;
        public static bool AllowRememberLogin = true;
        public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);

        public static string[] AllowedRedirectDomains =
            Environments.Development == Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                ? new[]
                {
                    "https://localhost:5001"
                }
                : new[]
                {
                    "https://nozomi.one"
                };

    public static bool ShowLogoutPrompt = true;
        public static bool AutomaticRedirectAfterSignOut = false;

        // specify the Windows authentication scheme being used
        public static readonly string WindowsAuthenticationSchemeName =
            Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme;

        // if user uses windows auth, should we load the groups from windows
        public static bool IncludeWindowsGroups = false;

        public static string InvalidCredentialsErrorMessage = "Invalid username or password";

        public static string CredentialsAlreadyTaken =
            "These credentials have been used for another account. Please try again.";

        public static string EmailNotConfirmed = "Please verify your email before signing in.";

        public static string FailedToJoinRole = "Your account's role failed to propagate.";

        public static string EmptyFormSubmitted = "Please provide valid credentials before proceeding";
    }
}