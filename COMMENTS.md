# A+ Educação Challenge

## Decisão da Arquitetura Utilizada

### Backend (.NET 9 + C# + Entity Framework):

A arquitetura segue os princípios da **Clean Architecture**, garantindo modularidade, testabilidade e facilidade de evolução. O projeto está estruturado em 5 camadas principais, cada uma com responsabilidades bem definidas:

- **Domain Layer** (`StudentManagement.Domain`):  
  No núcleo do sistema, concentro as regras de negócio, entidades e contratos essenciais. As entidades `Student` e `User` foram modeladas para refletir restrições reais (como unicidade de RA e CPF), e as interfaces e exceções específicas do domínio garantem isolamento e clareza das regras.

- **Application Layer** (`StudentManagement.Application`):  
  Orquestra os casos de uso do sistema, centralizando serviços para cadastro, consulta, atualização e remoção de alunos, além de autenticação. Helpers especializados (como validação de CPF) e DTOs garantem comunicação eficiente e validações robustas, sempre desacopladas da infraestrutura.

- **Infrastructure Layer** (`StudentManagement.Infrastructure`):  
  Implementa o acesso a dados com Entity Framework Core e PostgreSQL, incluindo versionamento de migrações e criação de índices (como o índice composto em RA e CPF para alta performance em grandes volumes). O contexto do banco, mapeamentos e repositórios seguem boas práticas de escalabilidade e manutenção. A injeção de dependências é centralizada, facilitando extensões futuras.

- **API Layer** (`StudentManagement.API`):  
  Expõe endpoints HTTP com segurança, com controllers para autenticação e gestão de alunos. As validações são aplicadas logo no início dos fluxos (no corpo da requisição), evitando processamento desnecessário. O middleware global de tratamento de exceções garante respostas padronizadas. A configuração inclui autenticação JWT edocumentação Swagger.

- **Test Layer** (`StudentManagement.Tests`):  
  Testes unitários e de integração cobrem os principais fluxos, utilizando xUnit, Moq e FluentAssertions. Os testes validam desde regras de negócio até integrações reais com o banco, garantindo confiabilidade e facilitando refatorações. O versionamento das migrações também é testado, assegurando que o sistema evolua de forma segura.

**Destaques:**  
- Estrutura preparada para crescimento do volume de dados (índices, versionamento de migrações).
- Validações e regras de negócio centralizadas e testadas.
- Separação clara de responsabilidades, facilitando manutenção e evolução.
- Preocupação com segurança, performance e experiência do desenvolvedor.

### Frontend (Vue 3 + Vite + Vuetify):

O frontend foi desenvolvido priorizando simplicidade, performance e organização. A estrutura é baseada em:

- **Componentes reutilizáveis** (`src/components`): Elementos Vue desacoplados, como tabelas, barras de busca e cabeçalhos, facilitando a montagem de interfaces responsivas e consistentes.
- **Views (páginas)** (`src/views`): Cada view representa uma tela da aplicação, como login, listagem e edição de estudantes, orquestrando a exibição dos componentes e a interação com os serviços.
- **Serviços** (`src/services`): Centralizam a comunicação com a API, autenticação JWT e notificações, mantendo a lógica de acesso a dados isolada dos componentes.
---

## Lista de Bibliotecas de Terceiros Utilizadas

### Backend

- Microsoft.AspNetCore.Authentication.JwtBearer (autenticação JWT)
- Microsoft.AspNetCore.OpenApi (OpenAPI/Swagger)
- Swashbuckle.AspNetCore (Swagger)
- Serilog.AspNetCore, Serilog.Sinks.Console (logging)
- Microsoft.EntityFrameworkCore, Npgsql.EntityFrameworkCore.PostgreSQL (ORM e provider PostgreSQL)
- BCrypt.Net-Next (hash de senha)
- Moq, xUnit, FluentAssertions (testes)

### Frontend

- Vue 3 (framework principal)
- Vue Router (roteamento)
- Vuetify (UI components)
- Axios (requisições HTTP)
- @mdi/font (ícones Material Design)
- Sass, sass-loader (pré-processador CSS)
- Vite, @vitejs/plugin-vue (build e dev server)

---

## O que você melhoraria se tivesse mais tempo

Com mais tempo, investiria em aprimorar a observabilidade e a robustez operacional do sistema. Embora o projeto já conte com logging estruturado via Serilog, seria interessante adicionar métricas de aplicação (como Prometheus) e health checks para monitoramento ativo, facilitando a detecção proativa de problemas em produção.

No aspecto de deploy, exploraria estratégias modernas de entrega contínua e escalabilidade, como a containerização com Docker e orquestração via ECS (AWS) ou Kubernetes, além de considerar opções serverless (como AWS Lambda) para cenários de menor carga ou APIs event-driven. Isso permitiria adaptar o sistema a diferentes demandas de uso, otimizando custos e performance.

Também priorizaria a implementação de testes automatizados mais abrangentes (incluindo testes end-to-end) e a configuração de pipelines CI/CD para garantir entregas seguras e ágeis. Por fim, investiria em documentação técnica detalhada e exemplos de uso para facilitar a manutenção e a evolução do projeto por outros desenvolvedores.

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