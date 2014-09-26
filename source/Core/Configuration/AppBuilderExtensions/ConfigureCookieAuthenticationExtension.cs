﻿/*
 * Copyright 2014 Dominick Baier, Brock Allen
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataHandler;
using System;
using Thinktecture.IdentityServer.Core;
using Thinktecture.IdentityServer.Core.Configuration;

namespace Owin
{
    static class UseCookieAuthenticationExtension
    {
        public static IAppBuilder ConfigureCookieAuthentication(this IAppBuilder app, CookieOptions options, IDataProtector dataProtector)
        {
            if (options == null) throw new ArgumentNullException("options");
            if (dataProtector == null) throw new ArgumentNullException("dataProtector");

            if (options.Prefix != null && options.Prefix.Length > 0)
            {
                options.Prefix += ".";
            }

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = Constants.PrimaryAuthenticationType,
                CookieName = options.Prefix + Constants.PrimaryAuthenticationType,
                ExpireTimeSpan = options.ExpireTimeSpan,
                SlidingExpiration = options.SlidingExpiration,
                TicketDataFormat = new TicketDataFormat(new DataProtectorAdapter(dataProtector, options.Prefix + Constants.PrimaryAuthenticationType))
            });

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = Constants.ExternalAuthenticationType,
                CookieName = options.Prefix + Constants.ExternalAuthenticationType,
                AuthenticationMode = AuthenticationMode.Passive,
                ExpireTimeSpan = Constants.ExternalCookieTimeSpan,
                SlidingExpiration = false,
                TicketDataFormat = new TicketDataFormat(new DataProtectorAdapter(dataProtector, options.Prefix + Constants.ExternalAuthenticationType))
            });

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = Constants.PartialSignInAuthenticationType,
                CookieName = options.Prefix + Constants.PartialSignInAuthenticationType,
                AuthenticationMode = AuthenticationMode.Passive,
                ExpireTimeSpan = options.ExpireTimeSpan,
                SlidingExpiration = options.SlidingExpiration,
                TicketDataFormat = new TicketDataFormat(new DataProtectorAdapter(dataProtector, options.Prefix + Constants.PartialSignInAuthenticationType))
            });
            return app;
        }
    }
}