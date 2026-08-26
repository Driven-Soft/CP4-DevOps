#!/bin/bash

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

source "$SCRIPT_DIR/00_config_geral.sh"


echo "=========================================="
echo "ARGOS - REMOÇÃO DA INFRAESTRUTURA"
echo "=========================================="

echo "Resource Group: $RESOURCE_GROUP"

az group delete \
    --name "$RESOURCE_GROUP" \
    --yes \
    --no-wait

echo ""
echo "Remoção do Resource Group iniciada."