resource "azurerm_application_insights" "insights" {
  name                = local.application_insights_name
  location            = var.location
  resource_group_name = azurerm_resource_group.resource_group.name
  application_type    = "web"
}

