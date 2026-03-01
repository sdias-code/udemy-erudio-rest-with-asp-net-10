# Projeto com base no curso da Udemy
## ASP.NET 2026 do 0 à Azure e GCP com ASP .NET 10 Docker e K8s

Projeto demonstrando boas práticas como:
- Versionamento de API
- HATEOAS
- Paginação
- Filtragem e ordenação nas consultas
- Upload e download de arquivos
- Importação e exportação de arquivos csv e xlsx
- Envio de e-mail e anexos (MailKit)
- Autenticação usando JWT com atualização, expiração e revoke de token
- Factory Pattern
- Service Layer
- Repository Pattern
- Injeção de Dependência
- Testes unitários 
- Testes de integração com banco real
- Uso do Testcontainers

------------------------------------------------------------------------

## Solution Structure

Solution: `RestWithAspNet10_Scaffold`

-   API: `RestWithAspNet10_Scaffold`
-   Tests: `RestWithAspNet10`

------------------------------------------------------------------------

# Arquitetura

A aplicação segue uma arquitetura em camadas com separação clara de
responsabilidades.

## Camadas

### 1. Controllers

Responsáveis por: - Expor os endpoints HTTP - Receber requisições -
Validar entrada - Retornar respostas HTTP apropriadas

Não contêm regra de negócio.

------------------------------------------------------------------------

### 2. Services

Responsáveis por: - Implementar regras de negócio - Orquestrar chamadas
para repositórios - Aplicar validações de domínio - Trabalhar com DTOs

------------------------------------------------------------------------

### 3. Repositories

Responsáveis por: - Acesso a dados - Comunicação com o banco SQL
Server - Execução de queries e paginação

Implementação baseada em Repository Pattern.

------------------------------------------------------------------------

### 4. DTOs

Responsáveis por: - Separar modelo de domínio da exposição externa -
Controlar dados de entrada (Create/Update) - Controlar dados de saída
(Response)

------------------------------------------------------------------------

### 5. Mappers

Responsáveis por: - Conversão entre Entities e DTOs

------------------------------------------------------------------------

## Fluxo da Requisição

Request HTTP ↓ Controller ↓ Service (Regra de negócio) ↓ Repository
(Acesso a dados) ↓ Banco de Dados ↑ Repository ↑ Service ↑ Controller ↑
Response HTTP

------------------------------------------------------------------------

# Tecnologias

-   .NET 10
-   ASP.NET Core Web API
-   Entity Framework Core
-   SQL Server
-   Swagger / OpenAPI (OAS 3.0) / Scalar
-   xUnit
-   FluentAssertions
-   Testcontainers
-   Docker

------------------------------------------------------------------------

# Endpoints

## Auth

/api/auth

POST /api/auth/signin
POST /api/auth/refresh
POST /api/auth/revoke
POST /api/auth/create

------------------------------------------------------------------------
## Book (v1)

/api/v1/book

GET /api/v1/book\
POST /api/v1/book\
GET /api/v1/book/{id}\
PUT /api/v1/book/{id}\
DELETE /api/v1/book/{id}

------------------------------------------------------------------------

## Person (v1)

/api/v1/person

GET /api/v1/person\
POST /api/v1/person\
GET /api/v1/person/{id}\
PUT /api/v1/person/{id}\
DELETE /api/v1/person/{id}\
PATCH /api/v1/person/{id}/enable\
PATCH /api/v1/person/{id}/disable

------------------------------------------------------------------------

## Person (v2)

/api/v2/person

GET /api/v2/person\
POST /api/v2/person\
GET /api/v2/person/{id}\
PUT /api/v2/person/{id}\
DELETE /api/v2/person/{id}

------------------------------------------------------------------------

## File

GET /api/v1/file/downloadfile/{fileName}\
POST /api/v1/file/uploadfile\
POST /api/v1/file/uploadmultiplefiles

------------------------------------------------------------------------

## Calc

GET /calc/soma/{firstNumber}/{secondNumber}\
GET /calc/subtracao/{firstNumber}/{secondNumber}\
GET /calc/divisao/{firstNumber}/{secondNumber}\
GET /calc/multiplicacao/{firstNumber}/{secondNumber}\
GET /calc/media/{firstNumber}/{secondNumber}\
GET /calc/raiz/{number}

------------------------------------------------------------------------

## Scalar

Url: https://localhost:7286/scalar/

------------------------------------------------------------------------

# Testes

Executar:

dotnet test

------------------------------------------------------------------------

# Como Executar

dotnet restore\
dotnet build\
dotnet run

------------------------------------------------------------------------

# Autor

Silvio Dias Ferreira\
https://github.com/sdias-code

Linkedin:\
https://www.linkedin.com/in/sdias2026

MIT License
