# API de Gestão Financeira Inteligente 💰

Web API RESTful desenvolvida em **.NET / ASP.NET Core** para gerenciamento financeiro básico, com foco em backend, segurança e boas práticas de desenvolvimento.

> ⚠️ Projeto somente backend. Não possui frontend — a interação é feita via Swagger ou ferramentas como Postman.

---

## 📌 Funcionalidades

### 👤 Usuários

- Cadastro de usuários  
- Autenticação e login  
- Proteção de rotas autenticadas  

### 🏦 Contas Bancárias

- Criação de contas vinculadas ao usuário  
- Consulta de contas  
- Atualização e remoção de contas  
- Controle de saldo inicial  

### 🔐 Segurança

- Autenticação e autorização com JWT (JSON Web Token)  
- Proteção de endpoints sensíveis  
- Validação de dados de entrada  
- Tratamento padronizado de erros  

### 🧪 Qualidade

- Testes unitários das regras de negócio  
- Arquitetura organizada em camadas  
- Boas práticas de API REST  

---

## 🛠️ Tecnologias Utilizadas

- .NET / ASP.NET Core Web API  
- C#  
- SQL Server ou PostgreSQL  
- Entity Framework Core  
- JWT (JSON Web Token)  
- Swagger (OpenAPI)  
- xUnit / NUnit (testes)  

---

## 🚀 Como Executar o Projeto

### Pré-requisitos

- .NET SDK instalado  
- SQL Server ou PostgreSQL  
- Visual Studio ou VS Code  

### Passos

```bash
git clone https://github.com/seu-usuario/seu-repositorio.git
cd nome-do-projeto
dotnet restore
dotnet ef database update
dotnet run

## 📖 Documentação da API

Após executar o projeto, acesse:

`https://localhost:xxxx/swagger`

O Swagger permite visualizar e testar todos os endpoints diretamente pelo navegador.

---

## 🧠 Objetivo do Projeto

Projeto desenvolvido para fins de estudo e portfólio, com foco em:

- Construção de APIs RESTful com .NET  
- Implementação de autenticação segura  
- Integração com banco de dados relacional  
- Estruturação de backend escalável  
- Aplicação de boas práticas de desenvolvimento  
