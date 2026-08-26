# Argos — DevOps CP | Containers em Nuvem com ACR e ACI

Projeto desenvolvido para o Checkpoint de **DevOps Tools & Cloud Computing** da FIAP.

A solução utiliza como base o projeto **Argos**, originalmente desenvolvido para a Global Solution 2026, adaptado neste checkpoint para execução de containers diretamente na Microsoft Azure utilizando:

* Azure Container Registry (ACR);
* Azure Container Instances (ACI);
* Azure Storage Account;
* Azure File Share para persistência do banco;
* Azure CLI para provisionamento da infraestrutura.

A aplicação é composta por uma **API REST em .NET** integrada a um banco de dados **MySQL**, ambos containerizados com Docker e publicados como imagens próprias no Azure Container Registry.

---

## Arquitetura

```text
                         Microsoft Azure
                               │
                       Resource Group
                               │
          ┌────────────────────┼────────────────────┐
          │                    │                    │
          ▼                    ▼                    ▼
         ACR             Storage Account        Azure Files
          │                                         │
    ┌─────┴─────┐                                   │
    │           │                                   │
    ▼           ▼                                   │
Imagem API   Imagem MySQL                           │
    │           │                                   │
    ▼           ▼                                   │
 ACI API ───► ACI MySQL ◄───────────────────────────┘
    │
    ▼
Swagger / API REST
Porta 8080
```

O banco MySQL utiliza um **Azure File Share** montado em `/var/lib/mysql`, garantindo que os dados sobrevivam à exclusão e recriação do ACI do banco.

---

## Tecnologias utilizadas

### Aplicação

* .NET 10
* ASP.NET Core Web API
* Entity Framework Core
* Pomelo Entity Framework Core MySQL
* Swagger / OpenAPI

### Banco de dados

* MySQL 8.0
* Entity Framework Core Migrations

### Containers

* Docker
* Docker Compose
* Dockerfile da aplicação
* Dockerfile do banco de dados

### Microsoft Azure

* Azure CLI
* Azure Container Registry — ACR
* Azure Container Instances — ACI
* Azure Storage Account
* Azure File Share
* Azure Resource Group

---

## Estrutura principal do projeto

```text
.
├── Argos.Api/
├── Argos.Application/
├── Argos.Domain/
├── Argos.Infrastructure/
│
├── azure/
│   ├── 00_config_geral.sh
│   ├── 01_criacao_infra.sh
│   ├── 02_push_imagens.sh
│   ├── 03_deploy_database.sh
│   ├── 04_deploy_api.sh
│   └── 05_remocao.sh
│
├── docker/
│   ├── app/
│   │   └── Dockerfile
│   └── database/
│       └── Dockerfile
│
├── db/
│   └── ddl.sql
│
├── tests/
│   └── json/
│
├── docker-compose.yml
├── .env.example
├── .gitignore
├── .gitattributes
├── Argos.sln
└── README.md
```

---

# Variáveis de ambiente

Credenciais e informações sensíveis **não são versionadas no repositório**.

O arquivo real:

```text
.env
```

está incluído no `.gitignore`.

O repositório possui somente:

```text
.env.example
```

com o modelo das variáveis necessárias.

## Criando o `.env`

Após clonar o repositório, copie:

```bash
cp .env.example .env
```

No PowerShell:

```powershell
Copy-Item .env.example .env
```

Depois edite o `.env`:

```env
MYSQL_PASSWORD=sua_senha
```

Não faça commit do arquivo `.env`.

A variável é utilizada pelo Docker Compose durante os testes locais e pelos scripts Azure responsáveis pelo deploy do MySQL e da API.

---

# Pré-requisitos

Para executar o projeto localmente:

* Git;
* Docker Desktop;
* Docker Compose;
* .NET SDK compatível com o projeto.

Para realizar o deploy na Azure:

* Azure CLI;
* assinatura Microsoft Azure;
* Docker Desktop em execução;
* acesso para criação dos recursos utilizados pelo projeto.

No Windows, os scripts `.sh` podem ser executados utilizando Git Bash.

---

# Execução local com Docker

Antes do deploy na Azure, as imagens devem ser construídas e testadas localmente.

Na raiz do projeto, com o docker rodando na máquina:

```bash
docker compose build
```

Depois:

```bash
docker compose up -d
```

Verifique os containers:

```bash
docker compose ps
```

A aplicação estará disponível em:

```text
http://localhost:8080/swagger
```

O MySQL utiliza localmente:

```text
localhost:3306
```

Para visualizar os logs:

```bash
docker compose logs argos-api
```

```bash
docker compose logs argos-mysql
```

Para encerrar:

```bash
docker compose down
```

---

# Dockerfiles

São utilizadas duas imagens próprias.

## Aplicação

```text
docker/app/Dockerfile
```

A imagem realiza o build e publicação da API .NET.

O container da aplicação utiliza:

```dockerfile
USER app
```

para garantir que o processo da API não seja executado como usuário `root`.

## Banco

```text
docker/database/Dockerfile
```

A imagem é baseada em:

```dockerfile
FROM mysql:8.0
```

e utiliza a porta:

```text
3306
```

com persistência em:

```text
/var/lib/mysql
```

---

# Provisionamento da infraestrutura Azure

Toda a infraestrutura de nuvem utilizada pelo projeto é criada através de **Azure CLI**.

Antes de iniciar:

```bash
az login
```

Caso existam várias assinaturas disponíveis, selecione a assinatura desejada antes da execução dos scripts.

As configurações gerais estão centralizadas em:

```text
azure/00_config_geral.sh
```

Entre elas:

* RM;
* Resource Group;
* região;
* nome do ACR;
* Storage Account;
* File Share;
* nomes das imagens;
* nomes dos ACIs;
* DNS labels;
* tag das imagens.

---

# 1. Criar infraestrutura base

Execute:

```bash
bash azure/01_criacao_infra.sh
```

O script cria via Azure CLI:

* Resource Group;
* Azure Container Registry;
* Storage Account;
* Azure File Share.

O File Share é utilizado posteriormente pelo ACI MySQL para persistência dos arquivos do banco.

---

# 2. Build e push das imagens para o ACR

Execute:

```bash
bash azure/02_push_imagens.sh
```

O script realiza:

1. autenticação no ACR;
2. build da imagem da API;
3. build da imagem do MySQL;
4. criação das tags do ACR;
5. push das duas imagens;
6. listagem dos repositórios existentes no ACR.

Os comandos Docker equivalentes utilizados são:

```bash
docker build \
  -f docker/app/Dockerfile \
  -t rm564723-argos-api:v1 \
  .
```

```bash
docker build \
  -f docker/database/Dockerfile \
  -t rm564723-argos-mysql:v1 \
  .
```

Após obter o Login Server do ACR:

```bash
docker tag \
  rm564723-argos-api:v1 \
  <acr-login-server>/rm564723-argos-api:v1
```

```bash
docker tag \
  rm564723-argos-mysql:v1 \
  <acr-login-server>/rm564723-argos-mysql:v1
```

Push:

```bash
docker push \
  <acr-login-server>/rm564723-argos-api:v1
```

```bash
docker push \
  <acr-login-server>/rm564723-argos-mysql:v1
```

As credenciais do ACR são recuperadas pela Azure CLI durante o deploy e não são armazenadas no repositório.

---

# 3. Deploy do banco MySQL no ACI

Execute:

```bash
bash azure/03_deploy_database.sh
```

O script:

* lê `MYSQL_PASSWORD` do `.env`;
* recupera as credenciais do ACR em runtime;
* recupera a chave do Storage Account;
* valida a imagem do banco no ACR;
* cria o ACI do MySQL;
* expõe a porta `3306`;
* monta o Azure File Share em `/var/lib/mysql`;
* envia as credenciais do banco como variáveis de ambiente seguras.

Para verificar o estado:

```bash
az container show \
  --resource-group rg-rm564723-argos-cp \
  --name rm564723-argos-mysql \
  --query "{Nome:name,Status:instanceView.state,IP:ipAddress.ip,FQDN:ipAddress.fqdn}" \
  --output table
```

Para visualizar os logs:

```bash
az container logs \
  --resource-group rg-rm564723-argos-cp \
  --name rm564723-argos-mysql
```

O MySQL estará disponível na porta:

```text
3306
```

---

# 4. Deploy da API .NET no ACI

Após o MySQL estar em execução:

```bash
bash azure/04_deploy_api.sh
```

O script:

* recupera o FQDN do MySQL;
* constrói a connection string;
* recupera as credenciais do ACR;
* valida a imagem da aplicação;
* cria o ACI da API;
* expõe a porta `8080`;
* injeta a connection string de forma segura.

Após o deploy, o script informa a URL do Swagger:

```text
http://<fqdn-api>:8080/swagger
```

---

# Verificação do usuário não-root

O container da aplicação foi configurado para executar com o usuário:

```text
app
```

A verificação pode ser feita através do ACI:

```bash
az container exec \
  --resource-group rg-rm564723-argos-cp \
  --name rm564723-argos-api \
  --exec-command "whoami"
```

Resultado esperado:

```text
app
```

---

# Acesso direto ao MySQL no ACI

Para executar consultas diretamente no banco:

```bash
MSYS_NO_PATHCONV=1 az container exec \
  --resource-group rg-rm564723-argos-cp \
  --name rm564723-argos-mysql \
  --exec-command "/bin/bash"
```

Dentro do container:

```bash
mysql -u argos -p argos
```

A senha será solicitada sem ser exibida no terminal.

Exemplos:

```sql
SHOW TABLES;
```

```sql
SELECT * FROM USUARIOS;
```

```sql
SELECT * FROM OCORRENCIAS;
```

---

# Persistência dos dados

Os dados do MySQL são armazenados no Azure File Share montado em:

```text
/var/lib/mysql
```

Para validar a persistência:

1. crie um registro pela API;
2. confirme o registro com `SELECT` diretamente no MySQL;
3. exclua somente o ACI do MySQL;
4. mantenha o Storage Account e o File Share;
5. execute novamente `03_deploy_database.sh`;
6. acesse o MySQL novamente;
7. execute o mesmo `SELECT`.

O registro deve continuar disponível após a recriação do ACI.

Exemplo de exclusão somente do ACI:

```bash
az container delete \
  --resource-group rg-rm564723-argos-cp \
  --name rm564723-argos-mysql \
  --yes
```

Depois:

```bash
bash azure/03_deploy_database.sh
```

---

# Testes CRUD

A API pode ser testada através do Swagger do ACI.

Para cada operação CRUD, a persistência deve ser validada diretamente no banco através de consultas `SELECT`.

Fluxo recomendado:

```text
POST pelo Swagger
        ↓
SELECT no MySQL
        ↓
registro criado

PUT/PATCH pelo Swagger
        ↓
SELECT no MySQL
        ↓
registro atualizado

DELETE pelo Swagger
        ↓
SELECT no MySQL
        ↓
registro removido ou desativado

GET pelo Swagger
        ↓
retorno dos registros armazenados
```

Os arquivos JSON utilizados durante os testes estão armazenados em:

```text
tests/json/
```

---

# DDL

O script de definição da estrutura do banco está disponível em:

```text
db/ddl.sql
```

O arquivo contém a estrutura das tabelas utilizadas pelo projeto, incluindo:

* tabelas;
* colunas;
* tipos;
* chaves primárias;
* chaves estrangeiras;
* índices e demais constraints aplicáveis.

---

# Remoção da infraestrutura

Para remover toda a infraestrutura criada na Azure:

```bash
bash azure/05_remocao.sh
```

O script remove o Resource Group do projeto e, consequentemente, os recursos contidos nele.

> Atenção: a remoção do Resource Group também exclui o Storage Account e o File Share. Portanto, os dados persistidos serão apagados.

---

# Segurança

O projeto não versiona credenciais reais.

As seguintes informações são obtidas dinamicamente ou armazenadas apenas localmente:

* senha do MySQL;
* senha administrativa do MySQL;
* credenciais do ACR;
* chave do Storage Account;
* connection string da aplicação.

O `.env` não deve ser enviado para o GitHub.

O repositório disponibiliza apenas:

```text
.env.example
```

como referência de configuração.

---

# Principais recursos Azure

A nomenclatura atual utiliza o RM do representante do grupo como prefixo.

```text
Resource Group:
rg-rm564723-argos-cp

ACR:
rm564723argosacr

Imagem API:
rm564723-argos-api

Imagem Banco:
rm564723-argos-mysql

ACI API:
rm564723-argos-api

ACI Banco:
rm564723-argos-mysql

Storage Account:
rm564723argosdata

File Share:
mysql-argos-volume
```

---

# Evidências da entrega

Para a demonstração do projeto devem ser apresentados:

* recursos criados na Microsoft Azure;
* imagens da API e banco armazenadas no ACR;
* ACI da API em execução;
* ACI MySQL em execução;
* Storage Account e File Share;
* Swagger acessado através do ACI;
* operações CRUD;
* consultas `SELECT` confirmando as operações diretamente no banco;
* persistência dos dados após a recriação do ACI MySQL;
* execução do container da API com usuário não-root.

---

# Projeto base

Este checkpoint utiliza como base o **Argos**, projeto desenvolvido anteriormente para a Global Solution 2026.

Na entrega anterior, os containers eram executados com Docker em uma máquina virtual Azure.

Nesta entrega, a infraestrutura foi adaptada para utilizar serviços gerenciados de containers da Azure:

```text
Antes:
Azure VM → Docker → Containers

Atual:
Docker → ACR → ACI
             ↓
       Azure Storage
```

---

# Integrantes

* **Representante — RM 564723:** [NOME]
* [NOME — RM]
* [NOME — RM]

## Links da entrega

* **Repositório GitHub:** [INSERIR LINK]
* **Vídeo:** [INSERIR LINK]
