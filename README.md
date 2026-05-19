A simple test project to explore the current state of ASP.NET Api's services.

The last time I did this was with the release of .NET Core, and now we are in .NET 10, so I wanted to take a look on the current state of creating API with .NET.

I explored how to create JWT tokens and Auth with different policies and roles for users.

Also, I wanted to explore how to the testing of API has evolve since the last time I did it.

To deploy the project, create a `appsettings.json` inside the ApiDemo.Api and add the configuration for Jwt:

```
"Jwt": {
    "Key": "super_secret_key_12345_super_secret_key_12345",
    "Issuer": "ApiDemo",
    "Audience": "ApiDemoClient",
    "ExpiresHours": 1
}
```



