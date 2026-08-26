#!/bin/bash

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"

source "$SCRIPT_DIR/00_config_geral.sh"


echo "=========================================="
echo "ARGOS - DEPLOY DA API .NET NO ACI"
echo "=========================================="


# ---------------------------------------------------------
# Carrega .env
# ---------------------------------------------------------

if [ ! -f "$REPO_ROOT/.env" ]; then
    echo "ERRO: arquivo .env não encontrado."
    exit 1
fi

set -a
source "$REPO_ROOT/.env"
set +a


if [ -z "${ORACLE_PASSWORD:-}" ]; then
    echo "ERRO: ORACLE_PASSWORD não definida no .env."
    exit 1
fi


# ---------------------------------------------------------
# Verifica se o ACI Oracle existe
# ---------------------------------------------------------

if ! az container show \
    --resource-group "$RESOURCE_GROUP" \
    --name "$DB_ACI" &>/dev/null; then

    echo "ERRO: ACI Oracle '$DB_ACI' não encontrado."
    echo "Execute primeiro: azure/03-deploy-database.sh"
    exit 1
fi


# ---------------------------------------------------------
# Recupera FQDN do Oracle
# ---------------------------------------------------------

ORACLE_FQDN=$(az container show \
    --resource-group "$RESOURCE_GROUP" \
    --name "$DB_ACI" \
    --query ipAddress.fqdn \
    --output tsv)


if [ -z "$ORACLE_FQDN" ]; then
    echo "ERRO: não foi possível recuperar o FQDN do Oracle."
    exit 1
fi

echo "Oracle disponível em:"
echo "$ORACLE_FQDN:1521"


# ---------------------------------------------------------
# Connection String da aplicação
# O código da API NÃO precisa ser alterado.
# ---------------------------------------------------------

CONNECTION_STRING="Data Source=${ORACLE_FQDN}:1521/XEPDB1;User Id=system;Password=${ORACLE_PASSWORD};"


# ---------------------------------------------------------
# ACR
# ---------------------------------------------------------

ACR_LOGIN_SERVER=$(az acr show \
    --name "$ACR_NAME" \
    --resource-group "$RESOURCE_GROUP" \
    --query loginServer \
    --output tsv)

ACR_USERNAME=$(az acr credential show \
    --name "$ACR_NAME" \
    --resource-group "$RESOURCE_GROUP" \
    --query username \
    --output tsv)

ACR_PASSWORD=$(az acr credential show \
    --name "$ACR_NAME" \
    --resource-group "$RESOURCE_GROUP" \
    --query passwords[0].value \
    --output tsv)


# ---------------------------------------------------------
# Verifica imagem da API no ACR
# ---------------------------------------------------------

if ! az acr repository show \
    --name "$ACR_NAME" \
    --repository "$APP_IMAGE" &>/dev/null; then

    echo "ERRO: imagem '$APP_IMAGE' não encontrada no ACR."
    exit 1
fi


# ---------------------------------------------------------
# Remove ACI anterior da API, caso exista
# ---------------------------------------------------------

if az container show \
    --resource-group "$RESOURCE_GROUP" \
    --name "$APP_ACI" &>/dev/null; then

    echo "ACI '$APP_ACI' já existe. Removendo..."

    az container delete \
        --resource-group "$RESOURCE_GROUP" \
        --name "$APP_ACI" \
        --yes
fi


# ---------------------------------------------------------
# Cria ACI da API
# ---------------------------------------------------------

echo "Criando ACI da API..."

az container create \
    --resource-group "$RESOURCE_GROUP" \
    --name "$APP_ACI" \
    --image "$ACR_LOGIN_SERVER/$APP_IMAGE:$TAG" \
    --cpu 1 \
    --memory 1.5 \
    --os-type Linux \
    --ip-address Public \
    --dns-name-label "$APP_DNS" \
    --ports 8080 \
    --registry-login-server "$ACR_LOGIN_SERVER" \
    --registry-username "$ACR_USERNAME" \
    --registry-password "$ACR_PASSWORD" \
    --environment-variables \
        ASPNETCORE_URLS="http://+:8080" \
    --secure-environment-variables \
        ConnectionStrings__ArgosOracle="$CONNECTION_STRING" \
    --restart-policy Always


echo ""
echo "=========================================="
echo "ACI API CRIADO"
echo "=========================================="

APP_FQDN=$(az container show \
    --resource-group "$RESOURCE_GROUP" \
    --name "$APP_ACI" \
    --query ipAddress.fqdn \
    --output tsv)

echo ""
echo "Swagger:"
echo "http://${APP_FQDN}:8080/swagger"

echo ""
echo "Status:"
az container show \
    --resource-group "$RESOURCE_GROUP" \
    --name "$APP_ACI" \
    --query "{Nome:name,Status:instanceView.state,IP:ipAddress.ip,FQDN:ipAddress.fqdn}" \
    --output table

echo ""
echo "Para verificar o usuário do container:"
echo "az container exec --resource-group $RESOURCE_GROUP --name $APP_ACI --exec-command \"whoami\""