using Authorization;
using Authorization.Models;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Server
{
    public class DataSeeder
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<IdentityContext>(
                options => options.UseSqlServer(connectionString));
            services
                .AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            services.AddOperationalDbContext(
                options =>
                {
                    options.ConfigureDbContext = db =>
                        db.UseSqlServer(
                            connectionString,
                            sql => sql.MigrationsAssembly(typeof(DataSeeder).Assembly.FullName));
                }
            );
            services.AddConfigurationDbContext(
                options =>
                {
                    options.ConfigureDbContext = db =>
                        db.UseSqlServer(
                            connectionString,
                            sql => sql.MigrationsAssembly(typeof(DataSeeder).Assembly.FullName));
                }
            );

            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var persistedGrantContex = scope.ServiceProvider
                    .GetService<PersistedGrantDbContext>();
                var configurationContext = scope.ServiceProvider
                    .GetService<ConfigurationDbContext>();
                var identityCotnext = scope.ServiceProvider
                    .GetService<IdentityContext>();

                persistedGrantContex?.Database.Migrate();
                configurationContext?.Database.Migrate();
                identityCotnext?.Database.Migrate();

                SeedData(configurationContext ?? throw new ArgumentNullException(nameof(configurationContext)));
                EnsureUsers(scope);
            }
 
        }

        private static void EnsureUsers(IServiceScope scope)
        {
            var userManagaer = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var testCustomer = userManagaer.FindByNameAsync("testCustomer").Result;
            if (testCustomer is null)
            {
                testCustomer = new IdentityUser
                {
                    UserName = "testCustomer",
                };
                var result = userManagaer.CreateAsync(testCustomer, "testCustomer1)").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result =
                    userManagaer.AddClaimsAsync(
                        testCustomer,
                        new Claim[]
                        {
                            new Claim(JwtClaimTypes.Role, "customer"),
                            new Claim(JwtClaimTypes.Scope, "BankAPI.own")
                        }
                    ).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }

            var testAdmin = userManagaer.FindByNameAsync("testAdmin").Result;
            if (testAdmin is null)
            {
                testAdmin = new IdentityUser
                {
                    UserName = "testAdmin",
                };
                var result = userManagaer.CreateAsync(testAdmin, "testAdmin1)").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result =
                    userManagaer.AddClaimsAsync(
                        testAdmin,
                        new Claim[]
                        {
                            new Claim(JwtClaimTypes.Role, "admin"),
                            new Claim(JwtClaimTypes.Scope, "BankAPI.admin")

                        }
                    ).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
        }

        private static void SeedData(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                foreach (var client in IdentityConfiguration.Clients.ToList())
                {
                    var  a = client.ToEntity();
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in IdentityConfiguration.IdentityResources.ToList())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var resource in IdentityConfiguration.ApiScopes.ToList())
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in IdentityConfiguration.ApiResources.ToList())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
        }
    }
}