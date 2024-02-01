﻿// Copyright (c) 2021 @Olivier Lefebvre. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using Aguacongas.IdentityServer.Admin.Models;
using Aguacongas.IdentityServer.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TheIdServer.Areas.Identity.Services
{
    public class EmailApiSender : IEmailSender
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<EmailOptions> _options;

        public EmailApiSender(HttpClient httpClient, IOptions<EmailOptions> options)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using var content = new StringContent(JsonSerializer.Serialize(new Email
            {
                Addresses = new string[] { email },
                Message = htmlMessage,
                Subject = subject
            }), Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync(_options.Value.ApiUrl, content)
                .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }
    }
}
