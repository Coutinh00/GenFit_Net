# GenFitNet API

API RESTful desenvolvida em .NET 8.0 para gestão de vagas e candidatos - Sistema de Recursos Humanos.

## 📋 Sobre o Projeto

O GenFitNet é uma solução tecnológica voltada ao tema "O Futuro do Trabalho", permitindo que empresas gerenciem vagas de emprego e candidatos de forma eficiente. A API oferece funcionalidades para:

- ✅ Criar, editar e excluir vagas
- ✅ Filtrar candidatos por diversos critérios
- ✅ Pesquisar pessoas para contato através de informações específicas
- ✅ Gerenciar relacionamento entre vagas e candidatos

## 🏗️ Arquitetura

O projeto segue uma arquitetura em camadas:

```
GenFitNet.API/              # Camada de apresentação (Controllers, Program.cs)
GenFitNet.Application/      # Camada de aplicação (DTOs, Services)
GenFitNet.Infrastructure/   # Camada de infraestrutura (Models, DbContext, Migrations)
GenFitNet.Tests/            # Testes automatizados
```

## 🚀 Tecnologias Utilizadas

- **.NET 8.0**
- **Entity Framework Core 8.0**
- **SQL Server** (LocalDB para desenvolvimento)
- **Serilog** (Logging estruturado)
- **Swagger/OpenAPI** (Documentação da API)
- **xUnit** (Testes automatizados)
- **FluentAssertions** (Assertions mais legíveis)
- **Moq** (Mocking para testes)

## 📦 Funcionalidades Implementadas

### 1. Boas Práticas REST (30 pts) ✅

- ✅ **Paginação**: Implementada em todos os endpoints de listagem
  - Parâmetros: `pageNumber` e `pageSize` (máximo 100)
  - Retorna informações sobre total de páginas, página atual, etc.

- ✅ **HATEOAS**: Links de navegação em todas as respostas
  - Links para recursos relacionados
  - Links de paginação (first, prev, next, last, self)
  - Links para ações (self, update, delete)

- ✅ **Status Codes Adequados**:
  - `200 OK`: Operação bem-sucedida
  - `201 Created`: Recurso criado com sucesso
  - `204 No Content`: Recurso deletado com sucesso
  - `400 Bad Request`: Dados inválidos
  - `404 Not Found`: Recurso não encontrado

- ✅ **Verbos HTTP Corretos**:
  - `GET`: Consulta de recursos
  - `POST`: Criação de recursos
  - `PUT`: Atualização completa de recursos
  - `DELETE`: Exclusão de recursos

### 2. Monitoramento e Observabilidade (15 pts) ✅

- ✅ **Health Checks**:
  - Endpoint `/health`: Verificação básica de saúde
  - Endpoint `/health/detailed`: Verificação detalhada com informações do banco de dados
  - Integração com Entity Framework Core para verificar conectividade do banco

- ✅ **Logging**:
  - Configurado com **Serilog**
  - Logs estruturados em console e arquivo
  - Logs rotativos diários em `logs/genfitnet-YYYYMMDD.txt`
  - Níveis de log configuráveis por ambiente

- ✅ **Tracing**:
  - Implementado com `System.Diagnostics.ActivitySource`
  - Rastreamento de operações HTTP
  - Tags para identificação de requisições (método, path, status code)
  - Atividades nomeadas por operação (GetAllVagas, CreateVaga, etc.)

### 3. Versionamento da API (10 pts) ✅

- ✅ **Estrutura de Versionamento**:
  - Versão atual: **v1** (`/api/v1/`)
  - Configurado com `Microsoft.AspNetCore.Mvc.Versioning`
  - Suporte para múltiplas versões simultâneas
  - Versionamento via URL path

- ✅ **Rotas Versionadas**:
  - `/api/v1/vagas` - Gestão de vagas
  - `/api/v1/candidatos` - Gestão de candidatos
  - `/api/v1/candidatos/search` - Pesquisa de candidatos

- ✅ **Controle de Versão**:
  - Versão padrão: v1.0
  - Versão especificada na URL: `v{version:apiVersion}`
  - Swagger configurado para exibir versões disponíveis

### 4. Integração e Persistência (30 pts) ✅

- ✅ **Banco de Dados**:
  - **SQL Server** (LocalDB para desenvolvimento)
  - Configuração via Connection String em `appsettings.json`

- ✅ **Entity Framework Core**:
  - DbContext configurado com relacionamentos
  - Configuração de entidades com constraints e validações
  - Seed data para desenvolvimento

- ✅ **Migrations**:
  - Migration inicial criada: `20240101000000_InitialCreate.cs`
  - Migrations aplicadas automaticamente em desenvolvimento
  - Suporte para evolução do esquema do banco

- ✅ **Modelos**:
  - **Vaga**: Representa vagas de emprego
  - **Candidato**: Representa candidatos
  - Relacionamento: Um candidato pode estar associado a uma vaga (opcional)

### 5. Testes Integrados (15 pts) ✅

- ✅ **Testes com xUnit**:
  - Testes de controllers (`VagasControllerTests`, `CandidatosControllerTests`)
  - Testes de services (`VagaServiceTests`)
  - Testes de integração usando banco em memória

- ✅ **Cobertura de Testes**:
  - Testes de criação, leitura, atualização e exclusão (CRUD)
  - Testes de paginação
  - Testes de filtros e pesquisas
  - Testes de validação e tratamento de erros

## 🔧 Configuração e Execução

### Pré-requisitos

- .NET 8.0 SDK
- SQL Server ou SQL Server LocalDB
- Visual Studio 2022 ou VS Code

### Instalação

1. Clone o repositório:
```bash
git clone <url-do-repositorio>
cd GenFit_Net
```

2. Restaure as dependências:
```bash
dotnet restore
```

3. Configure a connection string em `GenFitNet.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GenFitNetDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

4. Execute as migrations:
```bash
cd GenFitNet.API
dotnet ef database update
```

Ou as migrations serão aplicadas automaticamente ao iniciar a aplicação em modo desenvolvimento.

5. Execute a aplicação:
```bash
dotnet run --project GenFitNet.API
```

6. Acesse a documentação Swagger:
```
https://localhost:5001/swagger
```

### Executar Testes

```bash
dotnet test
```

## 📚 Endpoints da API

### Vagas

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/v1/vagas` | Lista todas as vagas (com paginação) |
| GET | `/api/v1/vagas/{id}` | Obtém uma vaga específica |
| POST | `/api/v1/vagas` | Cria uma nova vaga |
| PUT | `/api/v1/vagas/{id}` | Atualiza uma vaga existente |
| DELETE | `/api/v1/vagas/{id}` | Deleta uma vaga |

**Query Parameters para GET /api/v1/vagas:**
- `pageNumber` (int, padrão: 1)
- `pageSize` (int, padrão: 10, máximo: 100)
- `ativa` (bool?, opcional)

### Candidatos

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/v1/candidatos` | Lista todos os candidatos (com paginação) |
| POST | `/api/v1/candidatos/search` | Pesquisa candidatos com filtros |
| GET | `/api/v1/candidatos/{id}` | Obtém um candidato específico |
| POST | `/api/v1/candidatos` | Cria um novo candidato |
| PUT | `/api/v1/candidatos/{id}` | Atualiza um candidato existente |
| DELETE | `/api/v1/candidatos/{id}` | Deleta um candidato |

**Query Parameters para GET /api/v1/candidatos:**
- `pageNumber` (int, padrão: 1)
- `pageSize` (int, padrão: 10, máximo: 100)

**Filtros para POST /api/v1/candidatos/search:**
- `Nome` (string, opcional)
- `Email` (string, opcional)
- `Cidade` (string, opcional)
- `Estado` (string, opcional)
- `AreaAtuacao` (string, opcional)
- `AnosExperienciaMinimo` (int?, opcional)
- `VagaId` (int?, opcional)
- `Formacao` (string, opcional)

### Health Checks

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/health` | Verificação básica de saúde |
| GET | `/health/detailed` | Verificação detalhada com informações do banco |

## 📝 Exemplos de Uso

### Criar uma Vaga

```bash
POST /api/v1/vagas
Content-Type: application/json

{
  "titulo": "Desenvolvedor .NET Senior",
  "descricao": "Vaga para desenvolvedor .NET com experiência em APIs RESTful",
  "requisitos": "Experiência mínima de 5 anos em .NET",
  "localizacao": "São Paulo - SP",
  "salarioMinimo": 8000,
  "salarioMaximo": 12000,
  "tipoContrato": "CLT"
}
```

### Pesquisar Candidatos

```bash
POST /api/v1/candidatos/search
Content-Type: application/json

{
  "nome": "João",
  "areaAtuacao": ".NET",
  "anosExperienciaMinimo": 5,
  "cidade": "São Paulo"
}
```

## 🗂️ Estrutura do Projeto

```
GenFit_Net/
├── GenFitNet.API/
│   ├── Controllers/
│   │   └── V1/
│   │       ├── VagasController.cs
│   │       └── CandidatosController.cs
│   ├── Helpers/
│   │   └── HateoasHelper.cs
│   ├── Program.cs
│   └── appsettings.json
├── GenFitNet.Application/
│   ├── DTOs/
│   │   ├── VagaDTO.cs
│   │   ├── CandidatoDTO.cs
│   │   └── PagedResultDTO.cs
│   └── Services/
│       ├── IVagaService.cs
│       ├── VagaService.cs
│       ├── ICandidatoService.cs
│       └── CandidatoService.cs
├── GenFitNet.Infrastructure/
│   ├── Models/
│   │   ├── Vaga.cs
│   │   └── Candidato.cs
│   ├── Data/
│   │   └── ApplicationDbContext.cs
│   └── Migrations/
│       └── 20240101000000_InitialCreate.cs
└── GenFitNet.Tests/
    ├── VagasControllerTests.cs
    ├── CandidatosControllerTests.cs
    └── VagaServiceTests.cs
```

## 🔍 Observabilidade

### Logs

Os logs são gerados em:
- **Console**: Durante a execução
- **Arquivo**: `logs/genfitnet-YYYYMMDD.txt` (rotativo diário)

### Health Checks

Monitore a saúde da aplicação através dos endpoints:
- `/health`: Status básico
- `/health/detailed`: Status detalhado com informações do banco

### Tracing

O tracing está habilitado automaticamente e captura:
- Método HTTP
- Path da requisição
- Status code da resposta
- Duração da operação

## 📄 Licença

Este projeto foi desenvolvido para fins acadêmicos.

## 👥 Autores

Desenvolvido como parte do projeto "Advanced Business Development with .NET" - FIAP.

**Integrantes:**
- Vinicius Murtinho Vicente - RM551151
- Lucas Barreto Consentino - RM557107
- Gustavo Bispo Cordeiro - RM558515

---

**Versão da API**: v1.0  
**Última atualização**: 2024

