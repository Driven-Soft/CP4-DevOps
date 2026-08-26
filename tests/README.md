# Testes JSON — Argos CP DevOps

Estes arquivos documentam as requisições usadas nos testes da API.

- `GET` e `DELETE` normalmente não possuem body; por isso seus arquivos registram método, endpoint, status esperado e `body: null`.
- `POST` e `PUT` incluem o body JSON enviado pelo Swagger.
- Substitua os IDs de exemplo (`1`) pelos IDs retornados pelos POSTs no ambiente usado na gravação.
- Enums são enviados como strings, por exemplo `CIDADAO`, `ALTO`, `EM_ANALISE`.
- A API atual do ZIP analisado ainda expõe PATCH nos updates. Para cumprir literalmente a rubrica com PUT, adicione `[HttpPut("{id:int}")]` às mesmas actions de atualização antes de usar os arquivos `put.json`.
- `COMENTARIOS_OCORRENCIA` não possui endpoint de atualização na API atual.
- `LOGS_ALERTA` é tabela de auditoria interna e não possui controller CRUD; ela é validada por SELECT após operações em alertas.

## Ordem recomendada para testes
1. Usuário
2. Tipo de ocorrência
3. Zona de risco
4. Ocorrência
5. Comentário
6. Alerta

Anote os IDs retornados pelos POSTs e atualize os arquivos dependentes antes da gravação.
