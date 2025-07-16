# A+ Educação Challenge

## Decisão da Arquitetura Utilizada

### Backend (.NET 9 + C# + Entity Framework):

A arquitetura segue os princípios da **Clean Architecture**, separando responsabilidades e garantindo modularidade. O projeto está estruturado em 4 camadas principais:

- **Domain Layer** (`StudentManagement.Domain`):
  No núcleo do sistema está a camada de domínio, onde concentro as regras de negócio, entidades e contratos essenciais. Aqui, defini as entidades `Student` e `User`, além das interfaces de repositório e exceções específicas do domínio. Essa separação garante que a lógica de negócio permaneça isolada de detalhes de infraestrutura, facilitando manutenção e evolução do sistema.

- **Application Layer** (`StudentManagement.Application`):
  A camada de aplicação orquestra os casos de uso do sistema. Implementei serviços responsáveis por coordenar operações de cadastro, consulta, atualização e remoção de alunos, além de autenticação. Os DTOs facilitam a comunicação entre camadas, e os helpers centralizam validações e utilidades, como a validação de CPF e geração de JWT. Essa camada garante que as regras de negócio sejam aplicadas corretamente, sem acoplamento com a infraestrutura.

- **Infrastructure Layer** (`StudentManagement.Infrastructure`):
  Na infraestrutura, estão as implementações concretas de acesso a dados, utilizando Entity Framework Core com PostgreSQL. Configurei o contexto do banco, mapeamentos das entidades e repositórios que interagem diretamente com a base. Essa camada também centraliza as configurações de injeção de dependências, mantendo o sistema flexível e aderente ao princípio da inversão de dependência.

- **API Layer** (`StudentManagement.API`):
  A API expõe os endpoints HTTP para interação com o sistema. Implementei controllers para autenticação e gestão de alunos, aplicando validações robustas logo no início dos fluxos. O middleware global de tratamento de exceções garante respostas padronizadas e amigáveis em caso de erro. Além disso, a configuração da aplicação inclui autenticação JWT, documentação Swagger e boas práticas de segurança. O resultado é uma interface clara, segura e fácil de consumir.

- **Test Layer** (`StudentManagement.Tests`):
  Os testes unitários e de integração validam os principais fluxos do sistema, utilizando xUnit, Moq e FluentAssertions. Cobri desde validações de entrada até operações de repositório, garantindo que as regras de negócio e integrações estejam corretas e confiáveis.

### Frontend (Vue 3 + Vite + Vuetify):

O frontend foi desenvolvido priorizando simplicidade, performance e organização. A estrutura é baseada em:

- **Componentes reutilizáveis** (`src/components`): Elementos Vue desacoplados, como tabelas, barras de busca e cabeçalhos, facilitando a montagem de interfaces responsivas e consistentes.
- **Views (páginas)** (`src/views`): Cada view representa uma tela da aplicação, como login, listagem e edição de estudantes, orquestrando a exibição dos componentes e a interação com os serviços.
- **Serviços** (`src/services`): Centralizam a comunicação com a API, autenticação JWT e notificações, mantendo a lógica de acesso a dados isolada dos componentes.
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