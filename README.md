# Sistema de Gestão - API

Sistema de gestão de solicitações desenvolvido em **.NET 10** com arquitetura em camadas, autenticação JWT e ASP.NET Core Identity.

---

## 📋 Índice

- [Tecnologias Utilizadas](#-tecnologias-utilizadas)
- [Pré-requisitos](#-pré-requisitos)
- [Configuração do Ambiente](#-configuração-do-ambiente)
- [Como Executar](#-como-executar)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [Arquitetura](#-arquitetura)
- [Endpoints da API](#-endpoints-da-api)
- [Modelos de Dados](#-modelos-de-dados)
- [Fluxo de Dados](#-fluxo-de-dados)
- [Usuários de Teste](#-usuários-de-teste)

---

## 🚀 Tecnologias Utilizadas

| Tecnologia | Versão | Descrição |
|------------|--------|-----------|
| .NET | 10.0 | Framework principal |
| ASP.NET Core | 10.0 | Framework web |
| Entity Framework Core | 10.0.3 | ORM para acesso a dados |
| SQL Server | - | Banco de dados relacional |
| ASP.NET Core Identity | 10.0.3 | Autenticação e autorização |
| JWT Bearer | 10.0.3 | Tokens de autenticação |
| Swashbuckle | 10.1.2 | Documentação Swagger/OpenAPI |

---

## 📦 Pré-requisitos

Antes de executar o projeto, certifique-se de ter instalado:

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/sql-server) (local ou Docker)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)

### SQL Server via Docker (opcional)

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=SqlServer@2026!" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

---

## ⚙️ Configuração do Ambiente

### 1. Clone o repositório

```bash
git clone https://github.com/Hasselmann0/SistemaDeGest-o.git
cd SistemaDeGest-o
```

### 2. Configure a Connection String

Edite o arquivo `SistemaDeGestao/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=SistemaDeGestao;User Id=sa;Password=SuaSenha;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Secret": "ChaveSecretaSuperSeguraParaJWTComMaisDe32Caracteres!@#2024",
    "Issuer": "SistemaDeGestao",
    "Audience": "SistemaDeGestao.Client",
    "ExpirationInMinutes": 60
  }
}
```

### 3. Execute as Migrations

```bash
cd SistemaDeGestao
dotnet ef database update --project ../SistemaDeGestao.Infra
```

---

## ▶️ Como Executar

### Via CLI

```bash
cd SistemaDeGestao
dotnet run
```

### Via Visual Studio

1. Abra a solução `SistemaDeGestao.sln`
2. Defina `SistemaDeGestao.API` como projeto de inicialização
3. Pressione `F5` ou clique em "Iniciar"

### Acessar a API

- **Swagger UI**: https://localhost:{porta}/swagger
- **API Base URL**: https://localhost:{porta}/api

---

## 📁 Estrutura do Projeto

```
SistemaDeGestao/
├── SistemaDeGestao.API/              # Camada de Apresentação (API)
│   ├── Controllers/
│   │   ├── AuthController.cs         # Endpoints de autenticação
│   │   └── RequestController.cs      # Endpoints de solicitações
│   ├── Program.cs                    # Configuração da aplicação
│   ├── appsettings.json              # Configurações
│   └── SistemaDeGestao.API.csproj
│
├── SistemaDeGestao.APP/              # Camada de Aplicação (Serviços)
│   ├── DTOs/
│   │   ├── Auth/
│   │   │   ├── LoginRequestDto.cs
│   │   │   ├── LoginResponseDto.cs
│   │   │   └── UserInfoDto.cs
│   │   └── Requests/
│   │       ├── CreateRequestDto.cs
│   │       ├── RequestDto.cs
│   │       ├── RequestDetailDto.cs
│   │       ├── RequestFilterDto.cs
│   │       ├── ApproveRequestDto.cs
│   │       └── RejectRequestDto.cs
│   ├── Interfaces/
│   │   ├── IAuthService.cs
│   │   └── IRequestService.cs
│   ├── Mapper/
│   │   └── RequestMapper.cs
│   ├── Services/
│   │   ├── AuthService.cs
│   │   └── RequestService.cs
│   └── SistemaDeGestao.APP.csproj
│
├── SistemaDeGestao.Domain/           # Camada de Domínio (Entidades)
│   ├── Entities/
│   │   ├── BaseEntity.cs
│   │   ├── UserEntity.cs
│   │   ├── RequestEntity.cs
│   │   └── RequestStatusHistory.cs
│   ├── Enums/
│   │   ├── UserRole.cs
│   │   ├── RequestStatus.cs
│   │   ├── RequestCategory.cs
│   │   └── RequestPriority.cs
│   ├── Interfaces/
│   │   ├── ILoginRepository.cs
│   │   └── IRequestRepository.cs
│   └── SistemaDeGestao.Domain.csproj
│
├── SistemaDeGestao.Infra/            # Camada de Infraestrutura (Dados)
│   ├── Data/
│   │   ├── ApplicationDbContext.cs
│   │   └── Configurations/
│   │       ├── UserConfiguration.cs
│   │       ├── RequestConfiguration.cs
│   │       └── RequestStatusHistoryConfiguration.cs
│   ├── Migrations/
│   ├── Repositories/
│   │   ├── LoginRepository.cs
│   │   └── RequestRepository.cs
│   ├── Seed/
│   │   └── DatabaseSeeder.cs
│   └── SistemaDeGestao.Infra.csproj
│
└── SistemaDeGestao.sln
```

---

## 🏗️ Arquitetura

O projeto segue a **Arquitetura em Camadas (Layered Architecture)** com separação clara de responsabilidades:

```
┌─────────────────────────────────────────────────────────────┐
│                    SistemaDeGestao.API                      │
│                  (Controllers, Program.cs)                  │
│                    Camada de Apresentação                   │
└─────────────────────────┬───────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────────┐
│                    SistemaDeGestao.APP                      │
│              (Services, DTOs, Mappers, Interfaces)          │
│                    Camada de Aplicação                      │
└─────────────────────────┬───────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────────┐
│                   SistemaDeGestao.Domain                    │
│              (Entities, Enums, Interfaces)                  │
│                     Camada de Domínio                       │
└─────────────────────────┬───────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────────┐
│                   SistemaDeGestao.Infra                     │
│         (DbContext, Repositories, Configurations)           │
│                  Camada de Infraestrutura                   │
└─────────────────────────────────────────────────────────────┘
```

---

## 📡 Endpoints da API

### Autenticação

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| `POST` | `/api/auth/login` | Realizar login | ❌ Não |

#### Request Body - Login
```json
{
  "email": "user@sistema.com",
  "password": "User@123"
}
```

#### Response - Login
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2024-01-01T12:00:00Z",
  "user": {
    "id": "guid",
    "email": "user@sistema.com",
    "role": "User"
  }
}
```

---

### Solicitações (Requests)

| Método | Endpoint | Descrição | Autenticação | Roles |
|--------|----------|-----------|--------------|-------|
| `GET` | `/api/requests` | Listar solicitações | ✅ Sim | Todos |
| `GET` | `/api/requests/{id}` | Obter solicitação por ID | ✅ Sim | Todos |
| `POST` | `/api/requests` | Criar nova solicitação | ✅ Sim | Todos |
| `POST` | `/api/requests/{id}/approve` | Aprovar solicitação | ✅ Sim | Manager |
| `POST` | `/api/requests/{id}/reject` | Rejeitar solicitação | ✅ Sim | Manager |
| `GET` | `/api/requests/{id}/history` | Histórico de status | ✅ Sim | Todos |

#### Query Parameters - Listar Solicitações

| Parâmetro | Tipo | Descrição |
|-----------|------|-----------|
| `status` | `int?` | Filtrar por status (0=Pending, 1=Approved, 2=Rejected) |
| `category` | `int?` | Filtrar por categoria (0=Compras, 1=TI, 2=Reembolso) |
| `priority` | `int?` | Filtrar por prioridade (0=Baixa, 1=Media, 2=Alta) |
| `searchText` | `string?` | Busca por texto no título/descrição |

#### Request Body - Criar Solicitação
```json
{
  "title": "Compra de equipamento",
  "description": "Necessário comprar novo monitor para o setor de TI",
  "category": 1,
  "priority": 2
}
```

#### Request Body - Aprovar
```json
{
  "comment": "Aprovado conforme orçamento disponível"
}
```

#### Request Body - Rejeitar
```json
{
  "comment": "Rejeitado por falta de orçamento no período atual"
}
```

---

## 📊 Modelos de Dados

### UserEntity

| Campo | Tipo | Descrição |
|-------|------|-----------|
| Id | `string` | Identificador único (Identity) |
| UserName | `string` | Nome de usuário |
| Email | `string` | Email do usuário |
| Role | `UserRole` | Papel do usuário |
| IsActive | `bool` | Status ativo/inativo |
| CreatedAt | `DateTime` | Data de criação |
| UpdatedAt | `DateTime?` | Data de atualização |

### RequestEntity

| Campo | Tipo | Descrição |
|-------|------|-----------|
| Id | `Guid` | Identificador único |
| Title | `string` | Título da solicitação |
| Description | `string` | Descrição detalhada |
| Category | `RequestCategory` | Categoria |
| Priority | `RequestPriority` | Prioridade |
| Status | `RequestStatus` | Status atual |
| CreatedByUserId | `string` | ID do criador |
| CreatedAt | `DateTime` | Data de criação |

### RequestStatusHistory

| Campo | Tipo | Descrição |
|-------|------|-----------|
| Id | `Guid` | Identificador único |
| RequestId | `Guid` | ID da solicitação |
| FromStatus | `RequestStatus` | Status anterior |
| ToStatus | `RequestStatus` | Novo status |
| ChangedByUserId | `string` | ID de quem alterou |
| Comment | `string?` | Comentário da alteração |
| CreatedAt | `DateTime` | Data da alteração |

### Enumerações

#### RequestStatus
| Valor | Nome | Descrição |
|-------|------|-----------|
| 0 | Pending | Pendente |
| 1 | Approved | Aprovado |
| 2 | Rejected | Rejeitado |

#### RequestCategory
| Valor | Nome | Descrição |
|-------|------|-----------|
| 0 | Compras | Solicitações de compras |
| 1 | TI | Solicitações de TI |
| 2 | Reembolso | Solicitações de reembolso |

#### RequestPriority
| Valor | Nome | Descrição |
|-------|------|-----------|
| 0 | Baixa | Prioridade baixa |
| 1 | Media | Prioridade média |
| 2 | Alta | Prioridade alta |

#### UserRole
| Valor | Nome | Descrição |
|-------|------|-----------|
| 0 | User | Usuário comum |
| 1 | Manager | Gerente/Aprovador |

---

## 🔄 Fluxo de Dados

### Fluxo de Autenticação

```
┌──────────┐     POST /api/auth/login      ┌────────────────┐
│  Cliente │ ──────────────────────────────▶│ AuthController │
└──────────┘                                └───────┬────────┘
                                                    │
                                                    ▼
                                            ┌───────────────┐
                                            │  AuthService  │
                                            └───────┬───────┘
                                                    │
                                                    ▼
                                          ┌─────────────────┐
                                          │ LoginRepository │
                                          └───────┬─────────┘
                                                  │
                                                  ▼
                                            ┌───────────┐
                                            │ Identity  │
                                            │   (DB)    │
                                            └─────┬─────┘
                                                  │
                              JWT Token           │
┌──────────┐◀─────────────────────────────────────┘
│  Cliente │
└──────────┘
```

### Fluxo de Criação de Solicitação

```
┌──────────┐    POST /api/requests     ┌───────────────────┐
│  Cliente │ ─────────────────────────▶│ RequestController │
│ (c/ JWT) │                           └─────────┬─────────┘
└──────────┘                                     │
                                                 ▼
                                         ┌───────────────┐
                                         │RequestService │
                                         └───────┬───────┘
                                                 │
                                                 ▼
                                       ┌───────────────────┐
                                       │RequestRepository  │
                                       └─────────┬─────────┘
                                                 │
                                                 ▼
                                           ┌───────────┐
                                           │  SQL Server│
                                           │   (DB)    │
                                           └─────┬─────┘
                                                 │
              RequestDto                         │
┌──────────┐◀────────────────────────────────────┘
│  Cliente │
└──────────┘
```

### Fluxo de Aprovação/Rejeição

```
┌──────────┐   POST /api/requests/{id}/approve   ┌───────────────────┐
│ Manager  │ ───────────────────────────────────▶│ RequestController │
│ (c/ JWT) │                                     └─────────┬─────────┘
└──────────┘                                               │
                                                           ▼
                                                   ┌───────────────┐
                                                   │RequestService │
                                                   └───────┬───────┘
                                                           │
                    ┌──────────────────────────────────────┼──────────────────┐
                    │                                      │                  │
                    ▼                                      ▼                  ▼
          ┌─────────────────┐                    ┌─────────────────┐  ┌───────────────┐
          │ Valida Request  │                    │ Atualiza Status │  │Cria Histórico │
          │  (Status=Pending)│                    │                 │  │               │
          └─────────────────┘                    └─────────────────┘  └───────────────┘
                                                           │
                                                           ▼
                                                     ┌───────────┐
                                                     │  SQL Server│
                                                     └─────┬─────┘
                                                           │
                            RequestDto                     │
┌──────────┐◀──────────────────────────────────────────────┘
│ Manager  │
└──────────┘
```

---

## 👥 Usuários de Teste

O sistema cria automaticamente os seguintes usuários ao iniciar (Seed):

| Email | Senha | Role | Descrição |
|-------|-------|------|-----------|
| `admin@sistema.com` | `Admin@123` | Manager | Administrador/Gerente |
| `manager@sistema.com` | `Manager@123` | Manager | Gerente |
| `user@sistema.com` | `User@123` | User | Usuário comum |

---

## 🔐 Segurança

- **Autenticação**: JWT Bearer Token
- **Autorização**: Role-based (User, Manager)
- **Identity**: ASP.NET Core Identity com Entity Framework
- **CORS**: Configurado para `http://localhost:4200` (Angular)

### Headers de Autenticação

```http
Authorization: Bearer {seu_token_jwt}
```

---

## 📝 Licença

Este projeto está sob a licença MIT.

---

## 👨‍💻 Autor

Desenvolvido por [Hasselmann0](https://github.com/Hasselmann0)
