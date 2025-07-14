# A+ Educação Challenge

## Decisão da Arquitetura Utilizada

### Backend (.NET 9 + C# + Entity Framework):

A arquitetura segue os princípios da **Clean Architecture**, separando responsabilidades e garantindo modularidade. O projeto está estruturado em 4 camadas principais:

- **Domain Layer** (`StudentManagement.Domain`): Contém as regras centrais do negócio, entidades, interfaces e exceções do domínio.
- **Application Layer** (`StudentManagement.Application`): Orquestra os casos de uso, implementa a lógica de negócio, DTOs e serviços.
- **Infrastructure Layer** (`StudentManagement.Infrastructure`): Implementa acesso a dados, Entity Framework Context, repositórios e configurações.
- **API Layer** (`StudentManagement.API`): Interface HTTP, controllers, validações, middleware e configurações da aplicação.
- **Test Layer** (`StudentManagement.Tests`): Contém testes unitários e de integração, utilizando xUnit, Moq e FluentAssertions.

### Frontend:

O projeto foi desenvolvido com **Nuxt 3 + TypeScript + Vuetify**, utilizando componentização, composables para comunicação com a API e SCSS para estilização.

---

## Lista de Bibliotecas de Terceiros Utilizadas

### Backend:
- `Microsoft.AspNetCore.OpenApi` (Documentação da API)
- `Swashbuckle.AspNetCore` (Geração de documentação Swagger)
- `Microsoft.EntityFrameworkCore` (ORM para acesso ao banco de dados)
- `Npgsql.EntityFrameworkCore.PostgreSQL` (Driver PostgreSQL para Entity Framework)
- `xUnit` (Framework de testes unitários)
- `Moq` (Biblioteca para mocking em testes)
- `FluentAssertions` (Biblioteca para assertions mais expressivas)

### Frontend:
- `nuxt` (framework Vue.js)
- `vuetify` (UI)
- `sass` (estilização)
- `vue-router` (navegação)
- `axios` (requisições HTTP)

---

## O que Você Melhoraria se Tivesse Mais Tempo

Se houvesse mais tempo, implementaria as seguintes melhorias para tornar o sistema enterprise-ready:

- **Observabilidade**: Logs estruturados com Serilog, métricas com Prometheus/Grafana, health checks e distributed tracing.
- **Performance**: Cache distribuído com Redis, compressão de respostas, paginação eficiente e otimização de queries.
- **Segurança**: Autenticação JWT/OAuth2, controle de permissões por role, headers de segurança e criptografia de dados.
- **DevOps**: Docker, CI/CD automatizado, infraestrutura como código e estratégias de deploy avançadas.
- **Funcionalidades**: Notificações em tempo real (SignalR), exportação de dados, busca avançada (Elasticsearch) e integração com BI.

---

## Quais Requisitos Obrigatórios que Não Foram Entregues

Todos os requisitos obrigatórios foram entregues:

✅ **Framework JS**: Vue.js com Nuxt 3  
✅ **Framework de UI**: Vuetify  
✅ **API**: .NET Core 9 com C#  
✅ **Banco de Dados**: PostgreSQL  
✅ **Idioma**: Código em inglês, mensagens em português  
✅ **CRUD Completo**: Create, Read, Update, Delete de alunos  
✅ **Validações**: Campos obrigatórios e validações customizadas  
✅ **Tratamento de Erros**: Middleware global e consistente  
✅ **Testes**: Unitários implementados  
✅ **Documentação**: Swagger com exemplos 