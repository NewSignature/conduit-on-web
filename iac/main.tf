  
provider "azurerm" {
  version = "~>1.43"
}

terraform {
  backend "azurerm" {
  }
}

locals {
  app_name_alphanumeric = replace(replace(var.app_name, "-", ""), "_", "")
  resource_group_name = "rg-${var.app_name}-${var.environment}"
  application_insights_name = "ai-${var.app_name}-${var.environment}"
  app_service_name = "azapp-${var.app_name}-${var.environment}"
  app_service_plan_name = "azappsp-${var.app_name}-${var.environment}"
  storage_account_name = "st${local.app_name_alphanumeric}"
}

resource "azurerm_resource_group" "resource_group" {
  name     = local.resource_group_name
  location = var.location
}