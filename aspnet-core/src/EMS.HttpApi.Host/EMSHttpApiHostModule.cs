using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EMS.EntityFrameworkCore;
using EMS.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Swashbuckle;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;
using System.Security.Claims;
using EMS.Permissions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EMS;

[DependsOn(
    typeof(EMSHttpApiModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(EMSApplicationModule),
    typeof(EMSEntityFrameworkCoreModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
)]
public class EMSHttpApiHostModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("EMS");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        ConfigureAuthentication(context);
        ConfigurePolicies(context);
        ConfigureBundles();
        ConfigureUrls(configuration);
        ConfigureConventionalControllers();
        ConfigureVirtualFileSystem(context);
        ConfigureCors(context, configuration);
        ConfigureSwaggerServices(context, configuration);
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        //// Retrieve the JWT signing key from your configuration.
        var jwtKey = configuration["ApiAccessToken:ApiAccessTokenKey"];
        var issuer = configuration["ApiAccessToken:ApiAccessTokenIssuer"];
        var audience = configuration["ApiAccessToken:ApiAccessTokenAudience"];

        context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // In a production environment, this should be true.
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Configure the key used to validate the token's signature.
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(jwtKey)),
                    ValidateIssuer = true,
                    ValidIssuer = issuer, // Replace with the actual issuer.
                    ValidateAudience = true,
                    ValidAudience = audience, // Replace with the actual audience.
                    ValidateLifetime = true,
                    RoleClaimType = ClaimTypes.Role
                };
            });
    }
    private void ConfigurePolicies(ServiceConfigurationContext context)
    {
        context.Services.AddAuthorization(options =>
        {
            // User module policies
            options.AddPolicy(EMSPermissions.Admin.Default, policy =>
            {
                policy.RequireClaim(ClaimTypes.Role, "Admin");
            });
            options.AddPolicy(EMSPermissions.Admin.Create, policy =>
            {
                policy.RequireClaim(ClaimTypes.Role, "Admin");
            });
            options.AddPolicy(EMSPermissions.Admin.Edit, policy =>
            {
                policy.RequireClaim(ClaimTypes.Role, "Admin");
            });
            options.AddPolicy(EMSPermissions.Admin.Delete, policy =>
            {
                policy.RequireClaim(ClaimTypes.Role, "Admin");
            });

            // Hr module policies
            options.AddPolicy(EMSPermissions.Hr.Default, policy =>
            {
                policy.RequireClaim(ClaimTypes.Role, "Admin");
            });
            options.AddPolicy(EMSPermissions.Hr.Create, policy =>
            {
                policy.RequireClaim(ClaimTypes.Role, "Admin");
            });
            options.AddPolicy(EMSPermissions.Hr.Edit, policy =>
            {
                policy.RequireClaim(ClaimTypes.Role, "Admin");
            });
            options.AddPolicy(EMSPermissions.Hr.Delete, policy =>
            {
                policy.RequireClaim(ClaimTypes.Role, "Admin");
            });


            // Employee module policies
            options.AddPolicy(EMSPermissions.Employee.Default, policy =>
            {
                policy.RequireClaim(ClaimTypes.Role, "Hr");
            });
            options.AddPolicy(EMSPermissions.Employee.Create, policy =>
            {
                policy.RequireClaim(ClaimTypes.Role, "Hr");
            });
            options.AddPolicy(EMSPermissions.Employee.Edit, policy =>
            {
                policy.RequireClaim(ClaimTypes.Role, "Hr");
            });
            options.AddPolicy(EMSPermissions.Employee.Delete, policy =>
            {
                policy.RequireClaim(ClaimTypes.Role, "Hr");
            });




        });

    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                LeptonXLiteThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );
        });
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"]?.Split(',') ?? Array.Empty<string>());

            options.Applications["Angular"].RootUrl = configuration["App:ClientUrl"];
            options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "account/reset-password";
        });
    }

    private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<EMSDomainSharedModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}EMS.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<EMSDomainModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}EMS.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<EMSApplicationContractsModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}EMS.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<EMSApplicationModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}EMS.Application"));
            });
        }
    }

    private void ConfigureConventionalControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(EMSApplicationModule).Assembly);
        });
    }

    private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAbpSwaggerGenWithOAuth(
            configuration["AuthServer:Authority"]!,
            new Dictionary<string, string>
            {
                    {"EMS", "EMS API"}
            },
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "EMS API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(configuration["App:CorsOrigins"]?
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(o => o.RemovePostFix("/"))
                        .ToArray() ?? Array.Empty<string>())
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }
        app.UseUnitOfWork();
        app.UseDynamicClaims();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseAbpSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "EMS API");

            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            c.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            c.OAuthScopes("EMS");
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
