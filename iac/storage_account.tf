resource "azurerm_storage_account" "storage_account" {
  name                = local.storage_account_name
  resource_group_name = azurerm_resource_group.resource_group.name

  location                 = var.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  account_kind             = "StorageV2"

  static_website {

  }
}
