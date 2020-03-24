resource "azurerm_app_service" "app_service" {
  name                = local.app_service_name
  location            = azurerm_resource_group.resource_group.location
  resource_group_name = azurerm_resource_group.resource_group.name
  app_service_plan_id = azurerm_app_service_plan.app_plan.id

  site_config {
    dotnet_framework_version = "v4.0"
  }

  app_settings = {
    APPINSIGHTS_INSTRUMENTATIONKEY  = "${azurerm_application_insights.insights.instrumentation_key}",
    APPINSIGHTS_PROFILERFEATURE_VERSION = "1.0.0",
    APPINSIGHTS_SNAPSHOTFEATURE_VERSION = "1.0.0",
    APPLICATIONINSIGHTS_CONNECTION_STRING = "InstrumentationKey=${azurerm_application_insights.insights.instrumentation_key}",
    ApplicationInsightsAgent_EXTENSION_VERSION = "~2",
    DiagnosticServices_EXTENSION_VERSION = "~3",
    InstrumentationEngine_EXTENSION_VERSION = "~1",
    SnapshotDebugger_EXTENSION_VERSION = "~1",
    XDT_MicrosoftApplicationInsights_BaseExtensions = "~1",
    XDT_MicrosoftApplicationInsights_Mode = "recommended"
  }
}

resource "azurerm_app_service_plan" "app_plan" {
  name                = local.app_service_plan_name
  location            = azurerm_resource_group.resource_group.location
  resource_group_name = azurerm_resource_group.resource_group.name

  sku {
    tier = "Standard"
    size = "S1"
  }
}