#!/bin/bash

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"

source "$SCRIPT_DIR/00_config_geral.sh"

cd "$REPO_ROOT"

echo "=========================================="
echo "ARGOS - BUILD E PUSH DAS IMAGENS NO ACR"
echo "=========================================="


# ---------------------------------------------------------
# Valida ACR
# ---------------------------------------------------------

if ! az acr show \
    --name "$ACR_NAME" \
    --resource-group "$RESOURCE_GROUP" &>/dev/null; then

    echo "ERRO: ACR '$ACR_NAME' não encontrado."
    echo "Execute primeiro: azure/01-create-infra.sh"
    exit 1
fi


# ---------------------------------------------------------
# Login no Azure Container Registry
# ---------------------------------------------------------

echo "Realizando login no ACR..."

az acr login \
    --name "$ACR_NAME"


# ---------------------------------------------------------
# Recupera endereço oficial do ACR
# Exemplo:
# rm564723argosacr.azurecr.io
# ---------------------------------------------------------

ACR_LOGIN_SERVER=$(az acr show \
    --name "$ACR_NAME" \
    --resource-group "$RESOURCE_GROUP" \
    --query loginServer \
    --output tsv)

echo "ACR Login Server: $ACR_LOGIN_SERVER"


# ---------------------------------------------------------
# Build da API
# ---------------------------------------------------------

echo ""
echo "Construindo imagem da API..."

docker build \
    -f docker/app/Dockerfile \
    -t "$APP_IMAGE:$TAG" \
    .


# ---------------------------------------------------------
# Build do Oracle
# ---------------------------------------------------------

echo ""
echo "Construindo imagem do Oracle..."

docker build \
    -f docker/database/Dockerfile \
    -t "$DB_IMAGE:$TAG" \
    .


# ---------------------------------------------------------
# Tags para o ACR
# ---------------------------------------------------------

echo ""
echo "Criando tags para o ACR..."

docker tag \
    "$APP_IMAGE:$TAG" \
    "$ACR_LOGIN_SERVER/$APP_IMAGE:$TAG"

docker tag \
    "$DB_IMAGE:$TAG" \
    "$ACR_LOGIN_SERVER/$DB_IMAGE:$TAG"


# ---------------------------------------------------------
# Push API
# ---------------------------------------------------------

echo ""
echo "Enviando API para o ACR..."

docker push \
    "$ACR_LOGIN_SERVER/$APP_IMAGE:$TAG"


# ---------------------------------------------------------
# Push Oracle
# ---------------------------------------------------------

echo ""
echo "Enviando Oracle para o ACR..."

docker push \
    "$ACR_LOGIN_SERVER/$DB_IMAGE:$TAG"


echo ""
echo "=========================================="
echo "IMAGENS REGISTRADAS NO ACR"
echo "=========================================="

az acr repository list \
    --name "$ACR_NAME" \
    --output table