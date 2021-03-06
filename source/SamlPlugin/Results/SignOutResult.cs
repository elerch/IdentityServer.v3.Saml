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

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Thinktecture.IdentityServer.Core.Logging;

namespace IdentityServer.v3.Saml.Results
{
    public class SignOutResult : IHttpActionResult
    {
        private readonly static ILog Logger = LogProvider.GetCurrentClassLogger();
        private readonly IEnumerable<string> _urls;

        public SignOutResult(IEnumerable<string> urls)
        {
            _urls = urls ?? Enumerable.Empty<string>();
        }

        public Task<HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        HttpResponseMessage Execute()
        {
            const string format = "<iframe style=\"visibility: hidden; width: 1px; height: 1px\" src=\"{0}?wa=wsignoutcleanup1.0\"></iframe>";
            var sb = new StringBuilder(128);

            foreach (var url in _urls)
            {
                sb.AppendFormat(format, url);
            }

            var content = new StringContent(sb.ToString(), Encoding.UTF8, "text/html");

            Logger.Debug("Returning Saml signout response");
            return new HttpResponseMessage { Content = content };
        }
    }
}