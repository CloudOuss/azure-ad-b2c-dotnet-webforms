# Azure B2C integration in Web Forms 

This sample shows how to integrate Azure B2C in web forms application using that performs identity management with Azure AD B2C. It assumes you have some familiarity with Azure AD B2C. If you'd like to learn all that B2C has to offer, start with b2c documentation at aka.ms/aadb2c.

The app is a simple web application that performs sign-in, displays user claims and calls an API through a b2c access token.  

## How To Run This Sample

Getting started is simple! To run this sample you will need:

- Visual Studio

### Step 1:  Clone or download this repository

From your shell or command line:

`git clone https://github.com/CloudOuss/azure-ad-b2c-dotnet-webforms.git` 

##### In the B2C-WebForms solution
### Step 2: Setup your SSL url in Visual Studio

Setup the SSL url in the B2C-WebForms project to match the `ida:RedirectUri` appsetting value (see Step3)

### Step 3: Configure the sample to use your Azure AD B2C tenant

Now you can replace the app's default configuration with your own.  


open the web.config file and replace the client, tenant and policy names

```xml
      <add key="ida:Tenant" value="<<tenant-name>>" />
	  <add key="ida:TenantId" value="<<tenant-id>>" />
	  <add key="ida:ClientId" value="<<applicationid>>" />
	  <add key="ida:ClientSecret" value="<<applicationSecret>>" />
	  <add key="ida:AadInstance" value="https://mvpb2cdev.b2clogin.com/tfp/{0}/{1}" />
	  <add key="ida:RedirectUri" value="https://localhost:44300/" /> => already configured in our b2c
	  <add key="ida:SignInPolicy" value="<<signin-policy-name>>" />
	  <add key="api:Url" value="<<api-url>>" />
	  <add key="api:Scope" value="<<api-scope>>" />
	  <add key="api:SubscriptionKey" value="<<SubscriptionKey>>" />
```
### Step 3:  Run the sample

open the B2C-WebForms.sln in visual studio. Right click on B2C-WebForms project and choose Set as Startup Project.

If there are issues with nuget package restore in B2C-WebForms project, open Package Manager Console and run

	Update-Package -Reinstall

Clean and rebuild the solution, and run it.  You can now sign in / see user claims and call the API using the accounts you configured in your respective policies in b2c.
