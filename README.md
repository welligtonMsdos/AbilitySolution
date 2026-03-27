# AbilitySolution

[![.NET 10](https://img.shields.io/badge/.NET-10.0-purple)](https://dotnet.microsoft.com/)
[![MongoDB](https://img.shields.io/badge/MongoDB-Persistência-green)](https://www.mongodb.com/)
[![Docker](https://img.shields.io/badge/Docker-Enabled-blue)](https://www.docker.com/)

## 📖 Visão Geral da Solução

O **AbilitySolution** é uma solução de arquitetura moderna desenvolvida em **.NET 10** para automação de coleta e gerenciamento de notícias. O sistema utiliza técnicas de RPA para extração de dados do portal MSN Brasil, processando e armazenando as informações em um banco de dados **MongoDB**.

O projeto foi construído com foco em **Clean Architecture** e princípios **SOLID**, garantindo baixo acoplamento e alta testabilidade.
### Componentes principais:
- **Ability.Api**: Interface REST para consumo dos dados.
- **Ability.Worker**: Background Service responsável pelo scraping via **Playwright**.
- **Ability.Domain**: Núcleo da aplicação com entidades e regras de negócio.
- **Ability.Infrastructure**: Implementação de persistência e validações.

---

## 🛠️ Tecnologias e Padrões Utilizados

A solução foi construída utilizando o estado da arte do ecossistema .NET, focando em performance, validação rigorosa e manutenibilidade.

* **Runtime:** **.NET 10** (Explorando as últimas *features* e otimizações do framework).
* **RPA (Robotic Process Automation):** **Playwright** para extração de dados dinâmica e interação com elementos complexos do portal MSN.
* **Validação:** **FluentValidation** para garantir a integridade dos dados de entrada e regras de domínio.
* **Banco de Dados:** **MongoDB**, oferecendo alta escalabilidade e flexibilidade para o armazenamento de documentos de notícias.
* **Containerização:** **Docker** e **Docker Compose** para orquestração simplificada do ambiente de desenvolvimento e produção.

### 📐 Padrões de Projeto e Princípios
* **Repository Pattern:** Abstração completa da camada de persistência para facilitar testes e isolar o domínio.
* **Dependency Injection (DI):** Inversão de controle aplicada em toda a solução para baixo acoplamento.
* **Result Pattern:** Padronização das respostas da API, garantindo fluxos de sucesso e falha previsíveis.
* **Princípios:** Adoção rigorosa de **SOLID**, **DRY** (Don't Repeat Yourself) e **Clean Code**.

---

## 🏗️ Decisões Arquiteturais

Para garantir a escalabilidade e a robustez do sistema, foram tomadas as seguintes decisões de design:

* **Separação de Preocupações (SoC):** O **Worker** é totalmente independente da **API**. Como o processo de extração utiliza o Playwright (Chromium), o consumo de CPU e RAM é significativamente maior. Essa separação permite escalar o Worker horizontalmente em um cluster sem degradar a performance das consultas dos usuários na API.
* **Idempotência na Extração:** O Worker realiza uma verificação prévia no **MongoDB** através da URL da notícia antes de persistir. Isso garante que, mesmo em múltiplas execuções, não existam duplicidades de registros no banco de dados.
* **Resiliência no RPA:** Implementação de estratégias avançadas como `NetworkIdle`, `Timeouts` customizados e blocos `Try-Catch-Finally` granulares para lidar com a natureza volátil de sites externos e garantir que o navegador sempre seja liberado após o uso.

---

### 1. Global Exception Middleware
Localizado na camada de apresentação (`Ability.Api`), este componente envolve toda a requisição em um bloco de segurança.
* **Rede de Segurança:** Captura qualquer exceção não tratada (como falhas de conexão com o MongoDB ou erros de lógica) e as converte em uma resposta JSON padronizada com o `Result Pattern`.
* **Segurança de Dados:** Impede que *stack traces* internos sejam expostos ao cliente em ambiente de produção, retornando um erro `500 Internal Server Error` amigável.

### 2. Custom Validation Action Filter
Um filtro de ação customizado que atua antes da execução dos Controllers.
* **Auto-validação:** Utiliza reflexão para identificar e executar automaticamente o `IValidator<T>` (FluentValidation) correspondente ao DTO da requisição.
* **Desacoplamento:** Remove a necessidade de repetir `if (!ModelState.IsValid)` em todos os métodos do Controller, mantendo o código limpo e focado na regra de negócio.
* **Padronização:** Retorna automaticamente um `400 Bad Request` contendo um dicionário detalhado de erros de validação sempre que um contrato é violado.

## 📑 Funcionalidades da API

Além da automação via Worker, a **Ability.Api** expõe um **CRUD completo** para o gerenciamento das notícias extraídas:

* **POST `/api/Noticia`**: Criação manual de registros.
* **GET `/api/Noticia`**: Listagem de todas as notícias capturadas.
* **GET `/api/Noticia/{id}`**: Consulta detalhada de uma notícia específica.
* **PUT `/api/Noticia/{id}`**: Atualização de dados existentes.
* **DELETE `/api/Noticia/{id}`**: Remoção de registros do banco de dados.

---

## 📦 Como Rodar o Projeto

### Pré-requisitos
* **Docker** e **Docker Compose** instalados.
* **Git** para clonagem do repositório.

### Passo a Passo

1.  **Clone o repositório:**
    ```bash
    git clone https://github.com/welligtonMsdos/AbilitySolution.git
    cd Ability.Api
    ```

2.  **Subir os containers:**
    O projeto está configurado para orquestrar o banco de dados e as aplicações automaticamente:
    ```bash
    docker-compose up -d --build
    ```

3.  **Acessar a API:**
    A API estará disponível no endereço `http://localhost:5015`. Você pode testar o carregamento das notícias capturadas através da rota:
    * **GET:** `http://localhost:5015/scalar/v1`

---

## 📈 Melhorias Futuras

Roadmap de evolução incluiria:

* **Observabilidade:** Integração com **Serilog** para logs estruturados e **OpenTelemetry** para rastreamento distribuído de requisições.
* **Cache:** Implementação de **Redis** no endpoint de listagem para reduzir a carga de leitura no MongoDB.
* **Testes Unitários e Integração:** Cobertura de testes com **xUnit** e **Moq**, focando em regras de domínio e validadores do **FluentValidation**.
* **Interface de Monitoramento:** Desenvolvimento de um dashboard em **Angular** ou **React** para exibir métricas do Worker em tempo real (taxa de sucesso, volume de dados e logs de erro).

---

**Desenvolvido por:** Welligton Silva https://github.com/welligtonMsdos

