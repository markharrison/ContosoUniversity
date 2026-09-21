targetScope = 'subscription'

@description('Azure region for all resources.')
param location string

@description('Short environment name used in resource names.')
@minLength(2)
@maxLength(12)
param environmentName string = 'prod'

@description('Administrator login for the Azure SQL logical server.')
param sqlAdministratorLogin string

@secure()
@description('Administrator password for the Azure SQL logical server.')
param sqlAdministratorPassword string

var resourceGroupName = 'contoso-university-${environmentName}'

resource resourceGroup 'Microsoft.Resources/resourceGroups@2025-04-01' = {
  name: resourceGroupName
  location: location
}

module application 'modules/application.bicep' = {
  name: 'contoso-university-${environmentName}'
  scope: resourceGroup
  params: {
    environmentName: environmentName
    location: location
    sqlAdministratorLogin: sqlAdministratorLogin
    sqlAdministratorPassword: sqlAdministratorPassword
  }
}

output appName string = application.outputs.appName
output appUrl string = application.outputs.appUrl
output resourceGroupName string = resourceGroup.name
