using Microsoft.Identity.Client;
using backend.api.Credentials;
using backend.api.Dtos;
using System;

namespace backend.api.Authentication;

public class Authentication{
    public string GetAccessToken(){
        string clientId = Secrets.clientId!;
        string authority = $"https://login.microsoftonline.com/{Secrets.tenantId}/v2.0";
        string[] scopes = new string[] { "User.Read", "Calendars.Read", "Calendars.ReadWrite", "Calendars.ReadBasic", "Calendars.Read.Shared",
        "Calendars.ReadWrite.Shared", "email", "offline_access", "openid", "profile", "Mail.Read", "Mail.ReadBasic", 
        "Mail.ReadWrite", "Mail.Send"}; // Use the appropriate scope for Graph API
        
         var app = PublicClientApplicationBuilder
                .Create(clientId)
                .WithAuthority(authority)
                .Build();

        var result =  app.AcquireTokenWithDeviceCode(scopes, callback =>
                {
                    Console.WriteLine(callback.Message);
                    return Task.FromResult(0);
                }).ExecuteAsync().Result;

                // Print the access token
        Console.WriteLine(result.AccessToken);
        return result.AccessToken;
    }
}