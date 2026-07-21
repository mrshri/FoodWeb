# FoodWeb Secrets Setup Guide

## ⚠️ CRITICAL: Exposed Secrets Rotation Required

The following secrets were recently exposed and have been removed from source code:
- **Azure Service Bus Connection String** (from MessageBus.cs)
- **Stripe Test API Secret Key** (from appsettings.json)

**YOU MUST IMMEDIATELY ROTATE THESE CREDENTIALS** in their respective Azure and Stripe accounts.

---

## Development Setup: User Secrets

### 1. Initialize User Secrets for Each Project

User Secrets store configuration locally on your machine, separate from source control.

#### For Food.Services.OrderAPI
```powershell
cd Food.Services.OrderAPI
dotnet user-secrets init
```

#### For Food.Services.ShopingCartAPI
```powershell
cd Food.Services.ShopingCartAPI
dotnet user-secrets init
```

#### For Food.Services.AuthAPI (if needed)
```powershell
cd Food.Services.AuthAPI
dotnet user-secrets init
```

### 2. Store the Azure Service Bus Connection String

For **OrderAPI**:
```powershell
cd Food.Services.OrderAPI
dotnet user-secrets set "ConnectionStrings:ServiceBusConnection" "Endpoint=sb://YOUR-NAMESPACE.servicebus.windows.net/;..."
```

For **ShoppingCartAPI**:
```powershell
cd Food.Services.ShopingCartAPI
dotnet user-secrets set "ConnectionStrings:ServiceBusConnection" "Endpoint=sb://YOUR-NAMESPACE.servicebus.windows.net/;..."
```

### 3. Store the Stripe Secret Key

For **OrderAPI**:
```powershell
cd Food.Services.OrderAPI
dotnet user-secrets set "Stripe:SecretKey" "sk_test_YOUR_NEW_KEY_HERE"
```

### 4. Verify Secrets Are Set

On Windows, secrets are stored in:
```
%APPDATA%\Microsoft\UserSecrets\{ProjectGuid}\secrets.json
```

List all secrets:
```powershell
dotnet user-secrets list
```

---

## Production Setup: Environment Variables

For production deployment (Azure App Service, Docker, etc.):

### Azure App Service Configuration

1. Navigate to your App Service in Azure Portal
2. Go to **Settings > Configuration**
3. Add the following Application Settings:

| Name | Value |
|------|-------|
| `ConnectionStrings__ServiceBusConnection` | Your Azure Service Bus connection string |
| `Stripe__SecretKey` | Your Stripe Secret Key |

**Note:** Use double underscores (`__`) for nested configuration keys in Azure.

### Docker Environment
```dockerfile
ENV ConnectionStrings__ServiceBusConnection="your-connection-string"
ENV Stripe__SecretKey="your-stripe-key"
```

### Kubernetes Secrets
```yaml
apiVersion: v1
kind: Secret
metadata:
  name: foodweb-secrets
type: Opaque
stringData:
  ConnectionStrings__ServiceBusConnection: your-connection-string
  Stripe__SecretKey: your-stripe-key
```

---

## Configuration Priority (Highest to Lowest)

1. Environment Variables
2. User Secrets (Development only)
3. appsettings.json (for non-sensitive settings)
4. appsettings.{Environment}.json

---

## Files Modified

✅ **MessageBus/MessageBus.cs**
- Removed hardcoded Azure Service Bus connection string
- Now accepts connection string via constructor dependency injection

✅ **Food.Services.OrderAPI/appsettings.json**
- Removed Stripe secret key
- Added empty `ServiceBusConnection` placeholder

✅ **Food.Services.ShopingCartAPI/appsettings.json**
- Added empty `ServiceBusConnection` placeholder

✅ **Food.Services.OrderAPI/Program.cs**
- Updated MessageBus registration to inject connection string from configuration

✅ **Food.Services.ShopingCartAPI/Program.cs**
- Updated MessageBus registration to inject connection string from configuration

---

## Testing Your Setup

### Local Development
```powershell
# Run from project directory
dotnet run

# Should not show any configuration errors related to secrets
```

### Verify Configuration Loading
Add a debug endpoint to see which configuration is being used:
```csharp
app.MapGet("/config-test", (IConfiguration config) =>
{
	return new
	{
		hasServiceBusConnection = !string.IsNullOrEmpty(config.GetConnectionString("ServiceBusConnection")),
		hasStripeKey = !string.IsNullOrEmpty(config["Stripe:SecretKey"])
	};
});
```

---

## Troubleshooting

### ❌ "ServiceBusConnection connection string not found"
- Ensure you've initialized user secrets: `dotnet user-secrets init`
- Ensure you've set the secret: `dotnet user-secrets set "ConnectionStrings:ServiceBusConnection" "..."`
- Verify the exact key name matches: `ConnectionStrings:ServiceBusConnection`

### ❌ Stripe API errors in production
- Verify `Stripe:SecretKey` is set in App Service Configuration
- Check it's not empty in appsettings.json (should be `""`)
- Verify the key hasn't been rotated (old key will fail)

### ❌ Message Bus not publishing
- Check Azure Service Bus connection string is valid and accessible
- Verify namespace name and shared access keys are current

---

## Security Best Practices

✅ **DO:**
- Store secrets in user-secrets, environment variables, or Azure Key Vault
- Rotate secrets regularly
- Use different keys for different environments (dev/staging/production)
- Keep secrets out of version control
- Log secret changes in administration records

❌ **DON'T:**
- Commit secrets to git
- Share secrets via email or chat
- Use the same secret across environments
- Hardcode secrets in production code
- Check secrets into any branch

---

## Next Steps

1. **Delete exposed keys** from Azure and Stripe accounts
2. **Generate new keys** in respective services
3. **Set up user secrets** locally using the commands above
4. **Update deployment configuration** for production environments
5. **Test thoroughly** before deploying

---

**Date Created:** 2024
**Last Updated:** Today
**Status:** ✅ Secrets remediated and removed from source code
