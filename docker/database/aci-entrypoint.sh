#!/bin/bash

set -e

ORACLE_DATA="/opt/oracle/oradata"

echo "ARGOS: preparando Oracle para execução..."

if [ "$(id -u)" -eq 0 ]; then

    echo "ARGOS: container iniciado como root para compatibilidade com Azure Files."

    # Confirma que o volume existe
    if [ ! -d "$ORACLE_DATA" ]; then
        echo "ERRO: diretório $ORACLE_DATA não encontrado."
        exit 1
    fi

    echo "ARGOS: verificando acesso do usuário oracle ao volume..."

    # UID/GID 54321 = usuário 'oracle'. Usamos numérico porque o grupo
    # primário do oracle (GID 54321) não possui entrada correspondente
    # em /etc/group nesta imagem — chroot --userspec com nome falha
    # com "invalid group".

    # Testa efetivamente escrita usando o usuário Oracle
    if ! chroot --userspec=54321:54321 / sh -c \
        "touch '$ORACLE_DATA/.argos-write-test' && rm '$ORACLE_DATA/.argos-write-test'"; then

        echo "ERRO: usuário oracle não consegue escrever em $ORACLE_DATA."
        echo "Permissões atuais:"
        ls -ld "$ORACLE_DATA"

        exit 1
    fi

    echo "ARGOS: volume acessível pelo usuário oracle."
    echo "ARGOS: iniciando Oracle como usuário oracle..."

    exec chroot --userspec=54321:54321 / /opt/oracle/container-entrypoint.sh "$@"
fi

# Ambiente local ou outro ambiente que já esteja executando como oracle
echo "ARGOS: executando diretamente como $(whoami)."

exec /opt/oracle/container-entrypoint.sh "$@"