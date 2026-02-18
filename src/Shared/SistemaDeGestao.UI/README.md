# SistemaDeGestão — Frontend (Angular)

Sistema de gestão de solicitações internas com autenticação JWT, controle de permissões por role (User/Manager) e fluxo completo de criação, aprovação e rejeição de solicitações.

---

## Índice

- [Pré-requisitos](#pré-requisitos)
- [Instalação](#instalação)
- [Comandos Disponíveis](#comandos-disponíveis)
- [Configuração da API](#configuração-da-api)
- [Sobre o Projeto](#sobre-o-projeto)
- [Fluxo de Dados](#fluxo-de-dados)
- [Endpoints da API](#endpoints-da-api)
- [Estrutura de Pastas](#estrutura-de-pastas)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)

---

## Pré-requisitos

Antes de começar, certifique-se de ter instalado:

| Ferramenta | Versão mínima | Como verificar        |
| ---------- | ------------- | --------------------- |
| **Node.js** | 18.x+        | `node --version`      |
| **npm**     | 9.x+         | `npm --version`       |
| **Angular CLI** | 21.x     | `ng version`          |

> Se não tiver o Angular CLI instalado globalmente:
> ```bash
> npm install -g @angular/cli
> ```

Também é necessário que o **Backend (.NET)** esteja rodando em `https://localhost:7041` para que as chamadas à API funcionem.

---

## Instalação

1. **Clone o repositório** (caso ainda não tenha):
   ```bash
   git clone https://github.com/Hasselmann0/SistemaDeGest-o.git
   cd SistemaDeGest-o/src/Shared/SistemaDeGestao.UI
   ```

2. **Instale as dependências**:
   ```bash
   npm install
   ```

3. **Inicie o servidor de desenvolvimento**:
   ```bash
   ng serve
   ```

4. **Acesse no navegador**:
   ```
   http://localhost:4200
   ```

---

## Comandos Disponíveis

| Comando            | Descrição                                                    |
| ------------------ | ------------------------------------------------------------ |
| `npm start`        | Inicia o servidor de desenvolvimento (`ng serve`)            |
| `npm run build`    | Compila o projeto para produção na pasta `dist/`             |
| `npm run watch`    | Compila em modo watch (recompila a cada alteração)           |
| `npm test`         | Executa os testes unitários com Vitest                       |
| `ng generate component nome` | Gera um novo componente via Angular CLI            |

---

## Configuração da API

A URL base da API está configurada nos services:

- **Auth:** `https://localhost:7041/api/Auth` — em `src/app/services/auth.service.ts`
- **Requests:** `https://localhost:7041/api/requests` — em `src/app/services/request.service.ts`

> Para apontar para outra URL, altere a propriedade `apiUrl` diretamente nos arquivos de service.

O CORS do backend já está configurado para aceitar requisições de `http://localhost:4200`.

---

## Sobre o Projeto

O **SistemaDeGestão** é uma aplicação de gestão de solicitações internas de uma empresa. Ele possui dois perfis de usuário:

### Perfis de Usuário

| Role        | Permissões                                                                       |
| ----------- | -------------------------------------------------------------------------------- |
| **User**    | Criar solicitações, visualizar suas próprias solicitações, ver histórico          |
| **Manager** | Tudo do User + visualizar todas as solicitações, aprovar e rejeitar solicitações  |

### Funcionalidades

- **Login** com autenticação JWT (com opção "Lembrar-me")
- **Listagem** de solicitações em tabela com colunas: Título, Categoria, Prioridade, Status, Solicitante, Data de Criação
- **Criação** de nova solicitação via dialog (Título, Descrição, Categoria, Prioridade)
- **Aprovação** de solicitações pendentes (Manager only)
- **Rejeição** de solicitações pendentes com justificativa obrigatória (Manager only)
- **Histórico** de mudanças de status de cada solicitação (timeline visual)
- **Logout** com limpeza de tokens

### Enums do Sistema

| Enum             | Valores                          |
| ---------------- | -------------------------------- |
| **Status**       | Pending (Pendente), Approved (Aprovado), Rejected (Rejeitado) |
| **Categoria**    | Compras, TI, Reembolso           |
| **Prioridade**   | Baixa, Média, Alta               |

---

## Fluxo de Dados

```
┌──────────────┐     POST /api/Auth/login      ┌──────────────┐
│              │ ──────────────────────────────► │              │
│   Frontend   │ ◄────────────────────────────── │   Backend    │
│  (Angular)   │     { token, user }             │  (.NET API)  │
│              │                                 │              │
│  localStorage│     GET/POST /api/requests      │  SQL Server  │
│  sessionStore│ ──────────────────────────────► │              │
│              │ ◄────────────────────────────── │              │
└──────────────┘     Authorization: Bearer JWT   └──────────────┘
```

### Fluxo de Autenticação

1. Usuário preenche email e senha na tela de login
2. `AuthService.login()` faz `POST /api/Auth/login`
3. Backend valida credenciais e retorna `{ token, expiresAt, user }`
4. Token JWT é salvo no `localStorage` (se "Lembrar-me") ou `sessionStorage`
5. `authInterceptor` injeta o header `Authorization: Bearer <token>` em todas as requisições subsequentes
6. Se a API retornar 401/403, o interceptor faz logout automático

### Fluxo de Solicitações

1. Usuário acessa `/requests` (protegido pelo `authGuard`)
2. `RequestPageComponent` chama `RequestService.getAll()` → `GET /api/requests`
3. Backend filtra: Manager vê todas, User vê apenas as suas
4. Tabela exibe os dados com badges coloridos de status e prioridade
5. **Criar:** Botão FAB (+) → abre `CreateRequestDialog` → `POST /api/requests`
6. **Aprovar:** Botão ✓ na tabela → `POST /api/requests/{id}/approve` (Manager only)
7. **Rejeitar:** Botão ✗ na tabela → pede justificativa → `POST /api/requests/{id}/reject` (Manager only)
8. **Histórico:** Botão 🕑 na tabela → abre `RequestHistoryDialog` → `GET /api/requests/{id}/history`

### Proteção de Rotas

```
/login          → Pública (qualquer um acessa)
/requests       → Protegida pelo authGuard (requer login)
/               → Redireciona para /requests
```

---

## Endpoints da API

### Auth

| Método | Endpoint             | Descrição          | Auth    |
| ------ | -------------------- | ------------------ | ------- |
| POST   | `/api/Auth/login`    | Login do usuário   | Não     |

**Request:**
```json
{ "email": "user@email.com", "password": "senha123" }
```

**Response (200):**
```json
{
  "token": "eyJhbGciOi...",
  "expiresAt": "2026-02-18T00:00:00Z",
  "user": {
    "id": "guid-aqui",
    "name": "Nome do Usuário",
    "email": "user@email.com",
    "role": "User"
  }
}
```

### Requests

| Método | Endpoint                        | Descrição                 | Auth         |
| ------ | ------------------------------- | ------------------------- | ------------ |
| GET    | `/api/requests`                 | Listar solicitações       | Bearer Token |
| POST   | `/api/requests`                 | Criar solicitação         | Bearer Token |
| GET    | `/api/requests/{id}`            | Detalhes da solicitação   | Bearer Token |
| POST   | `/api/requests/{id}/approve`    | Aprovar solicitação       | **Manager**  |
| POST   | `/api/requests/{id}/reject`     | Rejeitar solicitação      | **Manager**  |
| GET    | `/api/requests/{id}/history`    | Histórico de status       | Bearer Token |

**GET /api/requests** — Query params opcionais: `status`, `category`, `priority`, `searchText`

**POST /api/requests — Request:**
```json
{
  "title": "Compra de equipamento",
  "description": "Necessário novo monitor para o setor de TI",
  "category": 0,
  "priority": 2
}
```

**POST /api/requests/{id}/approve — Request:**
```json
{ "comment": "Aprovado conforme orçamento" }
```

**POST /api/requests/{id}/reject — Request:**
```json
{ "comment": "Orçamento insuficiente para este período" }
```

---

## Estrutura de Pastas

```
SistemaDeGestao.UI/
├── angular.json                 # Configuração do Angular CLI
├── package.json                 # Dependências e scripts
├── tsconfig.json                # Configuração TypeScript base
├── tsconfig.app.json            # Configuração TS para a aplicação
├── tsconfig.spec.json           # Configuração TS para testes
│
├── public/                      # Arquivos estáticos (favicon, imagens)
│
└── src/
    ├── index.html               # HTML principal (entry point)
    ├── main.ts                  # Bootstrap da aplicação Angular
    ├── styles.css               # Estilos globais
    ├── material-theme.scss      # Tema customizado do Angular Material
    │
    └── app/
        ├── app.ts               # Componente raiz (App)
        ├── app.html             # Template do App (navbar + router-outlet)
        ├── app.css              # Estilos do App
        ├── app.config.ts        # Configuração (providers, interceptors)
        ├── app.routes.ts        # Definição de rotas
        │
        ├── models/              # Interfaces e enums TypeScript
        │   ├── auth.model.ts    #   LoginRequest, LoginResponse, LoginUser
        │   └── request.model.ts #   RequestDto, CreateRequestDto, enums, etc.
        │
        ├── services/            # Serviços (comunicação com a API)
        │   ├── auth.service.ts  #   Login, logout, gerenciamento de token
        │   └── request.service.ts #  CRUD de solicitações + approve/reject/history
        │
        ├── guards/              # Guards de rota
        │   └── auth.guard.ts    #   Protege rotas que requerem autenticação
        │
        ├── interceptors/        # Interceptors HTTP
        │   └── auth.interceptor.ts # Injeta token JWT nos headers
        │
        ├── components/          # Componentes reutilizáveis
        │   ├── navbar/          #   Barra de navegação superior
        │   │   ├── navbar.ts
        │   │   ├── navbar.html
        │   │   └── navbar.css
        │   │
        │   ├── request-table/   #   Tabela de solicitações (Material Table)
        │   │   ├── request-table.ts
        │   │   ├── request-table.html
        │   │   └── request-table.css
        │   │
        │   ├── request-history-dialog/  # Dialog com timeline de histórico
        │   │   ├── request-history-dialog.ts
        │   │   ├── request-history-dialog.html
        │   │   └── request-history-dialog.css
        │   │
        │   └── create-request-dialog/   # Dialog para criar nova solicitação
        │       ├── create-request-dialog.ts
        │       ├── create-request-dialog.html
        │       └── create-request-dialog.css
        │
        └── pages/               # Páginas (rotas)
            ├── login-page/      #   Tela de login
            │   ├── login-page.ts
            │   ├── login-page.html
            │   └── login-page.css
            │
            └── request-page/    #   Tela principal de solicitações
                ├── request-page.ts
                ├── request-page.html
                └── request-page.css
```

### Convenções de Arquitetura

- **Standalone Components** — Todos os componentes são standalone (sem NgModules)
- **Signals** — Estado reativo via `signal()` do Angular (não usa RxJS para estado local)
- **Functional Guards/Interceptors** — Usa `CanActivateFn` e `HttpInterceptorFn`
- **Lazy Loading** — Páginas carregadas sob demanda via `loadComponent()`
- **Angular Material** — UI baseada em Material Design (toolbar, table, dialog, form fields, etc.)

---

## Tecnologias Utilizadas

| Tecnologia        | Versão  | Uso                                  |
| ----------------- | ------- | ------------------------------------ |
| Angular           | 21.x    | Framework principal                  |
| Angular Material  | 21.x    | Componentes de UI (Material Design)  |
| TypeScript        | 5.9     | Linguagem de programação             |
| RxJS              | 7.8     | Programação reativa (HTTP, streams)  |
| Vitest            | 4.x     | Framework de testes unitários        |
| Node.js           | 18+     | Runtime para ferramentas de dev      |
