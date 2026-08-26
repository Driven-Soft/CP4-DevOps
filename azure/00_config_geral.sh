#!/bin/bash

# =========================================================
# CONFIGURAÇÕES GERAIS - ARGOS CP ACR/ACI
# =========================================================

RM="rm564723"

RESOURCE_GROUP="rg-${RM}-argos-cp"
LOCATION="eastus"

# Azure Container Registry
# Nome deve ser globalmente único e usar apenas letras/números
ACR_NAME="${RM}argosacr"

# Storage Account
# Nome deve usar apenas letras minúsculas e números
STORAGE_ACCOUNT="${RM}argosdata"

FILE_SHARE="mysql-argos-volume"

# Imagens
APP_IMAGE="${RM}-argos-api"
DB_IMAGE="${RM}-argos-mysql"
TAG="v1"

# Azure Container Instances
APP_ACI="${RM}-argos-api"
DB_ACI="${RM}-argos-mysql"

# DNS públicos
APP_DNS="${RM}-argos-api"
DB_DNS="${RM}-argos-mysql"