﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Net.Http;


namespace _62413___Project
{
    public class GptPrompt
    {
        private readonly HttpClient? client;

        public GptPrompt()
        {
            client = new HttpClient();
        }

        public async Task<string> GptRapidApiAsync(string prompt)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://simple-chatgpt-api.p.rapidapi.com/ask"),
                Headers =
                {
                    { "X-RapidAPI-Key", "your-key" },
                    { "X-RapidAPI-Host", "simple-chatgpt-api.p.rapidapi.com" },
                },
                Content = new StringContent($"{{\"question\": \"{prompt}\"}}", Encoding.UTF8, "application/json") // Interpolated string with correct quotation
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
                return body;
            }
        }
    }
}