output "app_service" {
    value = azurerm_app_service.app_service.name
}

output "storage_account" {
    value = azurerm_storage_account.storage_account.name
}