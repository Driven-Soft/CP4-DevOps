#!/bin/bash

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"

source "$SCRIPT_DIR/00_config_geral.sh"


echo "=========================================="
echo "ARGOS - DEPLOY DO MYSQL NO ACI"
echo "=========================================="


# ---------------------------------------------------------
# Carrega variáveis sensíveis do .env
# ---------------------------------------------------------

if [ ! -f "$REPO_ROOT/.env" ]; then
    echo "ERRO: arquivo .env não encontrado."
    echo "Crie o .env baseado no .env.example."
    exit 1
fi

set -a
source "$REPO_ROOT/.env"
set +a


if [ -z "${MYSQL_PASSWORD:-}" ]; then
    echo "ERRO: MYSQL_PASSWORD não definida no .env."
    exit 1
fi


# ---------------------------------------------------------
# Recupera Login Server do ACR
# ---------------------------------------------------------

ACR_LOGIN_SERVER=$(az acr show \
    --name "$ACR_NAME" \
    --resource-group "$RESOURCE_GROUP" \
    --query loginServer \
    --output tsv)


# ---------------------------------------------------------
# Credenciais do ACR
# Recuperadas em runtime, nunca armazenadas no GitHub.
# ---------------------------------------------------------

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
# Chave do Storage Account
# Também recuperada em runtime.
# ---------------------------------------------------------

STORAGE_KEY=$(az storage account keys list \
    --resource-group "$RESOURCE_GROUP" \
    --account-name "$STORAGE_ACCOUNT" \
    --query "[0].value" \
    --output tsv)


# ---------------------------------------------------------
# Verifica imagem no ACR
# ---------------------------------------------------------

if ! az acr repository show \
    --name "$ACR_NAME" \
    --repository "$DB_IMAGE" &>/dev/null; then

    echo "ERRO: imagem '$DB_IMAGE' não encontrada no ACR."
    echo "Execute primeiro: azure/02-push-images.sh"
    exit 1
fi


# ---------------------------------------------------------
# Caso o ACI já exista, remove somente o container.
#
# IMPORTANTE:
# Storage Account e File Share NÃO são removidos.
# Assim conseguimos testar persistência posteriormente.
# ---------------------------------------------------------

if az container show \
    --resource-group "$RESOURCE_GROUP" \
    --name "$DB_ACI" &>/dev/null; then

    echo "ACI '$DB_ACI' já existe. Removendo instância anterior..."

    az container delete \
        --resource-group "$RESOURCE_GROUP" \
        --name "$DB_ACI" \
        --yes
fi


# ---------------------------------------------------------
# Cria o ACI do MySQL
# ---------------------------------------------------------

echo "Criando ACI MySQL..."

MSYS_NO_PATHCONV=1 az container create \
    --resource-group "$RESOURCE_GROUP" \
    --name "$DB_ACI" \
    --image "$ACR_LOGIN_SERVER/$DB_IMAGE:$TAG" \
    --cpu 1 \
    --memory 3 \
    --os-type Linux \
    --ip-address Public \
    --dns-name-label "$DB_DNS" \
    --ports 3306 \
    --registry-login-server "$ACR_LOGIN_SERVER" \
    --registry-username "$ACR_USERNAME" \
    --registry-password "$ACR_PASSWORD" \
    --azure-file-volume-account-name "$STORAGE_ACCOUNT" \
    --azure-file-volume-account-key "$STORAGE_KEY" \
    --azure-file-volume-share-name "$FILE_SHARE" \
    --azure-file-volume-mount-path /var/lib/mysql \
    --secure-environment-variables \
        MYSQL_DATABASE="argos" \
        MYSQL_USER="argos" \
        MYSQL_PASSWORD="$MYSQL_PASSWORD" \
        MYSQL_ROOT_PASSWORD="$MYSQL_PASSWORD" \
    --restart-policy Never


echo ""
echo "=========================================="
echo "ACI MYSQL CRIADO"
echo "=========================================="

az container show \
    --resource-group "$RESOURCE_GROUP" \
    --name "$DB_ACI" \
    --query "{Nome:name,Status:instanceView.state,IP:ipAddress.ip,FQDN:ipAddress.fqdn}" \
    --output table


echo ""
echo "Para acompanhar os logs:"
echo "az container logs --resource-group $RESOURCE_GROUP --name $DB_ACI"