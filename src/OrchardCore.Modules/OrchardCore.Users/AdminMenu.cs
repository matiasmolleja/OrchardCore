using Microsoft.Extensions.Localization;
using OrchardCore.Environment.Navigation;
using System;
using OrchardCore.Users.Drivers;
using OrchardCore.Modules;

namespace OrchardCore.Users
{
    public class AdminMenu : INavigationProvider
    {
        public AdminMenu(IStringLocalizer<AdminMenu> localizer)
        {
            T = localizer;
        }

        public IStringLocalizer T { get; set; }

        public void BuildNavigation(string name, NavigationBuilder builder)
        {
            if (!String.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            builder
                .Add(T["Configuration"], configuration => configuration
                    .Add(T["Security"], "5", security => security
                        .Add(T["Users"], "5", installed => installed
                            .Action("Index", "Admin", "OrchardCore.Users")
                            .Permission(Permissions.ManageUsers)
                            .LocalNav()
                         )));
        }
    }
    
    [Feature("OrchardCore.Users.Registration")]
    public class RegistrationAdminMenu : INavigationProvider
    {
        public RegistrationAdminMenu(IStringLocalizer<AdminMenu> localizer)
        {
            T = localizer;
        }

        public IStringLocalizer T { get; set; }

        public void BuildNavigation(string name, NavigationBuilder builder)
        {
            if (!String.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            builder
                .Add(T["Configuration"], configuration => configuration
                    .Add(T["Settings"], settings => settings
                        .Add(T["Registration"], T["Registration"], registration => registration
                            .Permission(Permissions.ManageUsers)
                            .Action("Index", "Admin", new { area = "OrchardCore.Settings", groupId = RegistrationSettingsDisplayDriver.GroupId })
                            .LocalNav()
                        )));
        }
    }

    [Feature("OrchardCore.Users.ResetPassword")]
    public class ResetPasswordAdminMenu : INavigationProvider
    {
        public ResetPasswordAdminMenu(IStringLocalizer<AdminMenu> localizer)
        {
            T = localizer;
        }

        public IStringLocalizer T { get; set; }

        public void BuildNavigation(string name, NavigationBuilder builder)
        {
            if (!String.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            builder
                .Add(T["Configuration"], configuration => configuration
                    .Add(T["Settings"], settings => settings
                        .Add(T["Reset password"], T["Reset password"], password => password
                            .Permission(Permissions.ManageUsers)
                            .Action("Index", "Admin", new { area = "OrchardCore.Settings", groupId = ResetPasswordSettingsDisplayDriver.GroupId })
                            .LocalNav()
                        )));
        }
    }
}
