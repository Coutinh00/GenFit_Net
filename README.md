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
- **Oracle Database** (Oracle 19c ou superior)
- **Oracle.EntityFrameworkCore** (Provedor EF Core para Oracle)
- **Serilog** (Logging estruturado)
- **Swagger/OpenAPI** (Documentação da API)
- **xUnit** (Testes automatizados)
- **FluentAssertions** (Assertions mais legíveis)
- **Moq** (Mocking para testes)



## 🗄️ Banco de Dados Oracle

Este projeto utiliza o **Oracle Database 19c** fornecido pela FIAP para armazenamento de dados.

### Credenciais de Acesso

**Configuração de Conexão:**
- **Host:** `oracle.fiap.com.br`
- **Porta:** `1521`
- **SID:** `ORCL`
- **Usuário:** `rm558515`
- **Senha:** `Fiap#2025`

**Connection String:**
```
Data Source=oracle.fiap.com.br:1521/ORCL;User Id=rm558515;Password=Fiap#2025;
```

### Estrutura do Banco de Dados

O banco de dados Oracle contém as seguintes tabelas principais:

- **VAGAS** - Armazena informações sobre vagas de emprego
- **CANDIDATOS** - Armazena dados dos candidatos
- **AUDIT_LOGS** - Logs de auditoria para rastreamento de operações

> **Nota:** O banco de dados Oracle da FIAP já está configurado e disponível. As migrations do Entity Framework Core serão aplicadas automaticamente ao executar a aplicação em modo desenvolvimento.

## 🔧 Configuração e Execução

### Pré-requisitos

- .NET 8.0 SDK (versão 8.0.22 ou superior)
- Oracle Database 19c (fornecido pela FIAP)
- Oracle Client (ODAC) instalado (para desenvolvimento local)
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
    "DefaultConnection": "Data Source=oracle.fiap.com.br:1521/ORCL;User Id=rm558515;Password=Fiap#2025;"
  }
}
```

**Credenciais do Banco de Dados Oracle (FIAP):**
- **Host:** `oracle.fiap.com.br`
- **Porta:** `1521`
- **SID:** `ORCL`
- **Usuário:** `rm558515`
- **Senha:** `Fiap#2025`

**Formato da Connection String Oracle:**
- `Data Source`: Host:Port/SID (ex: oracle.fiap.com.br:1521/ORCL)
- `User Id`: Nome do usuário do banco de dados
- `Password`: Senha do usuário

4. **Configuração do Banco de Dados:**
   
   As migrations serão aplicadas automaticamente ao iniciar a aplicação em modo desenvolvimento. Se preferir executar manualmente:
   
   ```bash
   cd GenFitNet.API
   dotnet ef database update
   ```
   
   > **Importante:** Certifique-se de que o banco de dados Oracle da FIAP está acessível e que as credenciais estão corretas no `appsettings.json`.

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

### Comandos Úteis

**Compilar o projeto:**
```bash
dotnet build
```

**Executar o projeto (da pasta raiz):**
```bash
dotnet run --project GenFitNet.API/GenFitNet.API.csproj
```

**Executar o projeto (Git Bash - Windows):**
```bash
"/c/Program Files/dotnet/dotnet.exe" run --project GenFitNet.API/GenFitNet.API.csproj
```

**Executar o projeto (navegando para a pasta):**
```bash
cd GenFitNet.API
dotnet run
```

**Aplicar migrations manualmente:**
```bash
cd GenFitNet.API
dotnet ef database update
```

## 🌐 Deploys da API

### Ambiente de Desenvolvimento Local

**URL Base:** `http://localhost:5000` ou `https://localhost:5001`

**Swagger UI:** 
- HTTP: `http://localhost:5000/swagger`
- HTTPS: `https://localhost:5001/swagger`

**Health Check:**
- Básico: `http://localhost:5000/health`
- Detalhado: `http://localhost:5000/health/detailed`

**Banco de Dados:**
- **Tipo:** Oracle Database (FIAP)
- **Host:** `oracle.fiap.com.br`
- **Porta:** `1521`
- **SID:** `ORCL`
- **Usuário:** `rm558515`
- **Senha:** `Fiap#2025`
- **Connection String:** `Data Source=oracle.fiap.com.br:1521/ORCL;User Id=rm558515;Password=Fiap#2025;`

**Instruções de Acesso:**
1. Execute o projeto localmente usando:
   ```bash
   dotnet run --project GenFitNet.API
   ```
   Ou no Git Bash:
   ```bash
   "/c/Program Files/dotnet/dotnet.exe" run --project GenFitNet.API/GenFitNet.API.csproj
   ```
2. Acesse o Swagger UI para visualizar e testar os endpoints
3. As migrations são aplicadas automaticamente em modo desenvolvimento
4. Verifique a conexão com o banco Oracle através do endpoint `/health/detailed`

**Testes:**
- Não é necessário autenticação para acessar a API
- Use o Swagger UI para testar os endpoints interativamente
- Todos os endpoints estão documentados no Swagger
- O banco de dados Oracle da FIAP está configurado e pronto para uso

### Ambiente de Produção

> **⚠️ Status:** Atualmente, o projeto **não possui deploy de produção configurado**. Esta seção é um template para ser preenchido quando o deploy for realizado.

**Configurações Atuais do Projeto:**
- ✅ Apenas ambiente de desenvolvimento local configurado
- ❌ Não há `appsettings.Production.json`
- ❌ Não há Dockerfile ou configurações de containerização
- ❌ Não há pipelines de CI/CD configurados
- ❌ Não há variáveis de ambiente de produção definidas

**Quando o deploy for configurado, adicione aqui:**
- **URL Base:** `[URL_DO_DEPLOY]`
- **Swagger UI:** `[URL_DO_DEPLOY]/swagger`
- **Health Check:**
  - Básico: `[URL_DO_DEPLOY]/health`
  - Detalhado: `[URL_DO_DEPLOY]/health/detailed`
- **Banco de Dados:**
- **Tipo:** Oracle Database
- **Connection String:** Configurada via variáveis de ambiente
- **Formato:** `Data Source=[HOST]:[PORT]/[SERVICE_NAME];User Id=[USER];Password=[PASSWORD];`
- **Schema/User:** `[NOME_DO_SCHEMA]`
- **Instruções de Acesso e Testes**

### Exemplo de Teste Rápido

```bash
# Verificar se a API está online
curl http://localhost:5000/health

# Listar vagas
curl http://localhost:5000/api/v1/vagas

# Listar candidatos
curl http://localhost:5000/api/v1/candidatos
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

## 🔗 Integração com Banco de Dados Oracle

### Connection String Format

A connection string do Oracle segue o formato:
```
Data Source=HOST:PORT/SID;User Id=USERNAME;Password=PASSWORD;
```

### Exemplo de Conexão

```csharp
// Configuração automática via appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(connectionString));
```

### Troubleshooting de Conexão

Se encontrar problemas de conexão:

1. **Verifique as credenciais** no `appsettings.json`
2. **Teste a conectividade** com o banco Oracle:
   ```bash
   # Verificar se o host está acessível
   ping oracle.fiap.com.br
   ```
3. **Verifique o Health Check** da API:
   ```bash
   curl http://localhost:5000/health/detailed
   ```
4. **Confirme que o Oracle Client está instalado** no ambiente de desenvolvimento

## 📄 Licença

Este projeto foi desenvolvido para fins acadêmicos como parte do curso "Advanced Business Development with .NET" da FIAP.

## 👥 Autores

**Projeto:** GenFitNet - Sistema de Recrutamento Inteligente com IA

**Disciplina:** Advanced Business Development with .NET - FIAP

**Integrantes:**
- **Vinicius Murtinho Vicente** - RM551151
- **Lucas Barreto Consentino** - RM557107
- **Gustavo Bispo Cordeiro** - RM558515

---

**Versão da API:** v1.0  
**Última atualização:** Novembro 2024  
**Banco de Dados:** Oracle Database 19c (FIAP)  
**Framework:** .NET 8.0

