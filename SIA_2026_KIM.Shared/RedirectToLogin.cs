using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIA_2026_KIM.Shared
{
    internal class RedirectToLogin : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override void OnInitialized() =>
            NavigationManager.NavigateTo("/login", replace: true, forceLoad: true);
    }
}
