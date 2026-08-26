FROM gvenzl/oracle-xe

# Azure Container Instances exige root para montar Azure File Share.
# O entrypoint customizado posteriormente reduz os privilégios
# e executa o Oracle como usuário oracle.
USER root

COPY docker/database/aci-entrypoint.sh /usr/local/bin/aci-entrypoint.sh

RUN chmod 755 /usr/local/bin/aci-entrypoint.sh

EXPOSE 1521

VOLUME ["/opt/oracle/oradata"]

ENTRYPOINT ["/usr/local/bin/aci-entrypoint.sh"]