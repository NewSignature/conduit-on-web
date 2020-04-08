  
provider "azurerm" {
  version = "~>2.2.0"
  features {
    key_vault {
      purge_soft_delete_on_destroy = true
    }
  }
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
  storage_account_name = "st${local.app_name_alphanumeric}${var.environment}"
  key_vault_name = "kv-${substr(var.app_name, 0, 12)}-${var.environment}"
}

resource "azurerm_resource_group" "resource_group" {
  name     = local.resource_group_name
  location = var.location

  tags = {
    projectName = "Demo"
    owner = "Facundo Gauna"
  }
}