provider "azurerm" {

}

resource "azurerm_resource_group" "OldAppDemoRG" {
  name = "__OldAppDemoRG__"
  location = "East US"
}

resource "azurerm_sql_server" "OldAppDemosqlserver" {
  name                         = "__oldappdemo-sqlserver__"
  resource_group_name          = "${azurerm_resource_group.OldAppDemoRG.name}"
  location                     = "${azurerm_resource_group.OldAppDemoRG.location}"
  version                      = "12.0"
  administrator_login          = "nsadmin"
  administrator_login_password = "NewSignature2020"

  tags = {
    environment = "production"
  }

}
output "fully_qualified_domain_name" {
  value = "${azurerm_sql_server.OldAppDemosqlserver.fully_qualified_domain_name}"
}

resource "azurerm_app_service_plan" "OldAppDemoPlan" {
  name                = "__OldAppDemoRG-appserviceplan__"
  location            = "${azurerm_resource_group.OldAppDemoRG.location}"
  resource_group_name = "${azurerm_resource_group.OldAppDemoRG.name}"

  sku {
    tier = "Standard"
    size = "S1"
  }
}

resource "azurerm_app_service" "OldAppDemoRG" {
  name                = "__OldAppDemoRG-app-service__"
  location            = "${azurerm_resource_group.OldAppDemoRG.location}"
  resource_group_name = "${azurerm_resource_group.OldAppDemoRG.name}"
  app_service_plan_id = "${azurerm_app_service_plan.OldAppDemoPlan.id}"

  site_config {
    dotnet_framework_version = "v4.0"
  }

  app_settings = {
    "SOME_KEY" = "some-value"
  }

  connection_string {
    name  = "Database"
    type  = "SQLServer"
    value = "Server=some-server.mydomain.com;Integrated Security=SSPI"
  }
}

resource "azurerm_application_insights" "OldAppDemoInsights" {
  name                = "__OldAppDemo-appinsights__"
  location            = "East US"
  resource_group_name = "${azurerm_resource_group.OldAppDemoRG.name}"
  application_type    = "web"
}

output "instrumentation_key" {
  value = "${azurerm_application_insights.OldAppDemoInsights.instrumentation_key}"
}

output "app_id" {
  value = "${azurerm_application_insights.OldAppDemoInsights.app_id}"
}
resource "azurerm_redis_cache" "OldAppDemoRedis" {
  name                = "__OldAppDemo-cache__"
  location            = "${azurerm_resource_group.OldAppDemoRG.location}"
  resource_group_name = "${azurerm_resource_group.OldAppDemoRG.name}"
  capacity            = 1
  family              = "C"
  sku_name            = "Standard"
  enable_non_ssl_port = false
  minimum_tls_version = "1.2"

  redis_configuration {}
}