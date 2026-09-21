# Deployment

The application runs on .NET 10. Local development uses SQLite. The Azure
deployment provisions a Linux App Service and an Azure SQL Database with Bicep.

## Local development

Install the .NET 10 SDK, then run:

```bash
dotnet restore ContosoUniversity/ContosoUniversity.sln
dotnet test ContosoUniversity/ContosoUniversity.sln
dotnet run --project ContosoUniversity/ContosoUniversity.csproj
```

The local database is `ContosoUniversity/contoso-university.db`. It is created
and seeded automatically and is ignored by Git.

## Configure GitHub OIDC trust

The workflows use OpenID Connect, so no Azure client secret or publish profile
is stored in GitHub.

1. Sign in with Azure CLI and create an Entra application and service principal:

   ```bash
   az login
   SUBSCRIPTION_ID=$(az account show --query id -o tsv)
   TENANT_ID=$(az account show --query tenantId -o tsv)
   APP_ID=$(az ad app create --display-name contoso-university-github \
     --query appId -o tsv)
   az ad sp create --id "$APP_ID"
   az role assignment create \
     --assignee "$APP_ID" \
     --role Contributor \
     --scope "/subscriptions/$SUBSCRIPTION_ID"
   ```

   Subscription-level Contributor is required because `infra/main.bicep`
   creates the resource group. Use a dedicated subscription or replace this
   with a custom least-privilege role in shared environments.

2. Create `credential.json`, replacing the organization/repository and
   environment if necessary:

   ```json
   {
     "name": "contoso-university-prod",
     "issuer": "https://token.actions.githubusercontent.com",
     "subject": "repo:markharrison/ContosoUniversity:environment:prod",
     "description": "GitHub Actions production environment",
     "audiences": ["api://AzureADTokenExchange"]
   }
   ```

   Register it and remove the temporary file:

   ```bash
   az ad app federated-credential create \
     --id "$APP_ID" \
     --parameters credential.json
   rm credential.json
   ```

   Create one federated credential per GitHub environment used by the
   workflows. The subject must exactly match
   `repo:OWNER/REPOSITORY:environment:ENVIRONMENT`.

3. In **Repository settings → Environments**, create `prod`. Add approval rules
   if required. Add these environment secrets:

   - `AZURE_CLIENT_ID`: the value of `APP_ID`
   - `AZURE_TENANT_ID`: the value of `TENANT_ID`
   - `AZURE_SUBSCRIPTION_ID`: the value of `SUBSCRIPTION_ID`
   - `SQL_ADMIN_PASSWORD`: a strong Azure SQL administrator password

## Deploy

Run **Actions → Deploy → Run workflow**. Choose the environment and Azure
region. The workflow:

1. builds and runs the endpoint tests;
2. publishes the .NET application;
3. signs in to Azure using OIDC;
4. deploys `infra/main.bicep`; and
5. deploys the published application to App Service.

The Bicep deployment output includes the application URL. Azure supplies the
production SQL connection string through the
`ConnectionStrings__DefaultConnection` App Service setting; it is not stored
in source control.

## Delete

Run **Actions → Delete → Run workflow** and enter the same environment name.
Deletion is asynchronous and removes the entire
`contoso-university-ENVIRONMENT` resource group, including its database.
