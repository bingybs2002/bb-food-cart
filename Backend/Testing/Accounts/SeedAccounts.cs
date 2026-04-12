using Backend.Auth;
using Backend.Auth.DTOs;
using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Backend.Testing.Accounts
{
    public static class SeedAccounts
    {
        public static async Task SeedAdmin(IServiceProvider s)
        {
            await using var scope = s.CreateAsyncScope();

            var userManager = scope.ServiceProvider.
                GetRequiredService<UserManager<IdentityUser>>();

            var adminPhone = "9179";
            var adminPassword = "9179";

            var existAdmin = await userManager
                .Users.SingleOrDefaultAsync(x =>
                x.PhoneNumber == adminPhone || x.UserName == adminPhone);

            if (existAdmin != null)
            {

            }
            else
            {
                existAdmin = new IdentityUser
                {
                    UserName = adminPhone,
                    PhoneNumber = adminPassword
                };
                var result = await userManager.CreateAsync(existAdmin, adminPassword);
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create the admin user!!!");
                }
                if (!await userManager.IsInRoleAsync(existAdmin, "Admin"))
                {
                    await userManager.AddToRoleAsync(existAdmin, "Admin");
                }
            }
        }

        public static async Task SeedUser(AppDbContext db)
        {
            if (db.Customers.Any()) { return; }

            var json = await File.ReadAllTextAsync("Testing/Accounts/Customer.json");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var items = JsonSerializer.Deserialize<List<RegisterCustomerRequest>>(json, options);

            if (items == null)
            {
                return;
            }

            var httpclient = new HttpClient();

            foreach (var i in items)
            {
                // basic validation to avoid sending null phone numbers
                if (string.IsNullOrWhiteSpace(i?.PhoneNumber))
                {
                    Console.WriteLine("Skipping user with missing phone number.");
                    continue;
                }

                var response = await httpclient.PostAsJsonAsync("https://localhost:63196/account/register", i);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Successfully created user {i.PhoneNumber}");
                }
                else
                {
                    Console.WriteLine($"Failed to create user {i.PhoneNumber} - {response.StatusCode}");
                }
            }
        }
    }
}