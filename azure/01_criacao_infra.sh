#!/bin/bash

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

source "$SCRIPT_DIR/00_config_geral.sh"

echo "=========================================="
echo "ARGOS - CRIAÇÃO DA INFRAESTRUTURA AZURE"
echo "=========================================="

# ---------------------------------------------------------
# Verifica autenticação Azure
# ---------------------------------------------------------

if ! az account show &>/dev/null; then
    echo "ERRO: Azure CLI não está autenticado."
    echo "Execute: az login"
    exit 1
fi


# ---------------------------------------------------------
# Providers utilizados pelo projeto
# ---------------------------------------------------------

echo "Registrando providers..."

az provider register \
    --namespace Microsoft.ContainerRegistry \
    --output none

az provider register \
    --namespace Microsoft.Storage \
    --output none

az provider register \
    --namespace Microsoft.ContainerInstance \
    --output none


# ---------------------------------------------------------
# Resource Group
# ---------------------------------------------------------

if ! az group show \
    --name "$RESOURCE_GROUP" &>/dev/null; then

    echo "Criando Resource Group '$RESOURCE_GROUP'..."

    az group create \
        --name "$RESOURCE_GROUP" \
        --location "$LOCATION" \
        --output none

else
    echo "Resource Group '$RESOURCE_GROUP' já existe."
fi


# ---------------------------------------------------------
# Azure Container Registry
# ---------------------------------------------------------

if ! az acr show \
    --name "$ACR_NAME" \
    --resource-group "$RESOURCE_GROUP" &>/dev/null; then

    echo "Criando ACR '$ACR_NAME'..."

    az acr create \
        --resource-group "$RESOURCE_GROUP" \
        --name "$ACR_NAME" \
        --sku Basic \
        --admin-enabled true \
        --output none

else
    echo "ACR '$ACR_NAME' já existe."

    # Garante que as credenciais administrativas estão habilitadas,
    # pois serão utilizadas pelos ACIs para baixar as imagens.
    az acr update \
        --name "$ACR_NAME" \
        --resource-group "$RESOURCE_GROUP" \
        --admin-enabled true \
        --output none
fi


# ---------------------------------------------------------
# Storage Account
# ---------------------------------------------------------

if ! az storage account show \
    --name "$STORAGE_ACCOUNT" \
    --resource-group "$RESOURCE_GROUP" &>/dev/null; then

    echo "Criando Storage Account '$STORAGE_ACCOUNT'..."

    az storage account create \
        --resource-group "$RESOURCE_GROUP" \
        --name "$STORAGE_ACCOUNT" \
        --location "$LOCATION" \
        --sku Standard_LRS \
        --output none

else
    echo "Storage Account '$STORAGE_ACCOUNT' já existe."
fi

connection_string=$(az storage account show-connection-string --name $STORAGE_ACCOUNT --resource-group $RESOURCE_GROUP --query connectionString --output tsv)

# ---------------------------------------------------------
# Azure File Share
# Volume persistente do Oracle
# ---------------------------------------------------------

echo "Verificando File Share '$FILE_SHARE'..."

FILE_SHARE_EXISTS=$(az storage share-rm exists \
    --resource-group "$RESOURCE_GROUP" \
    --storage-account "$STORAGE_ACCOUNT" \
    --name "$FILE_SHARE" \
    --query exists \
    --output tsv) \
    --connection-string "$connection_string"

if [ "$FILE_SHARE_EXISTS" != "true" ]; then

    echo "Criando File Share '$FILE_SHARE'..."

    az storage share-rm create \
        --resource-group "$RESOURCE_GROUP" \
        --storage-account "$STORAGE_ACCOUNT" \
        --name "$FILE_SHARE" \
        --quota 10 \
        --output none

else
    echo "File Share '$FILE_SHARE' já existe."
fi

echo ""
echo "=========================================="
echo "INFRAESTRUTURA BASE CRIADA COM SUCESSO"
echo "=========================================="

az resource list \
    --resource-group "$RESOURCE_GROUP" \
    --output table