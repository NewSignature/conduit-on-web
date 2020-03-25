output "app_service" {
    value = azurerm_app_service.app_service.name
}

output "storage_account" {
    value = azurerm_storage_account.storage_account.name
}

output "app_insights_instrumentation_key" {
    value = azurerm_application_insights.insights.instrumentation_key    
}
