﻿using Microsoft.AspNetCore.Localization;

namespace DaprIdentity.Extensions
{
    public class RouteDataRequestCultureProvider : RequestCultureProvider
    {
        public int IndexOfCulture;
        public int IndexofUiCulture;

        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));
            string uiCulture;

            string culture = uiCulture = httpContext.Request.Path.Value.Split('/')[IndexOfCulture];

            var providerResultCulture = new ProviderCultureResult(culture, uiCulture);

            return Task.FromResult(providerResultCulture);
        }
    }
}
