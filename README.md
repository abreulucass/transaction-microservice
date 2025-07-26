# Microsserviço de Gerenciamento de Transações

Este projeto é um microsserviço desenvolvido em **.NET 8**, seguindo os princípios de **Clean Architecture**, que permite **registrar** e **listar** transações financeiras.  
Ele utiliza **MongoDB Atlas** para persistência e **Azure Service Bus** para comunicação assíncrona entre serviços.

Esse desafio foi desenvolvido como parte do Desafio Talent Lab 2025 - Bemol Digital.

---

## ⚙️ Tecnologias utilizadas

- ASP.NET Core 8
- MongoDB Atlas
- Azure Service Bus
- xUnit + Moq + FluentAssertions (testes)
- Clean Architecture
- Docker

---

## 🚀 Como executar o projeto

### ✅ Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- Conta no [MongoDB Atlas](https://www.mongodb.com/cloud/atlas)
- Conta no [Microsoft Azure](https://azure.microsoft.com/)
- Visual Studio, VS Code ou Rider
- Docker (Opcional)

---

### 💻 1. Clone o repositório

```
git clone https://github.com/abreulucass/transaction-microservice.git
cd transaction-microservice
```

### 🔐 2. Configurar variáveis no `appsettings.json`

Crie ou edite o arquivo `appsettings.Development.json` com a seguinte estrutura:

```json
{
  "ConnectionStrings": {
    "MongoDb": "SUASTRING",
    "AzureBusConnection": "SUASTRING"
  },
  "MongoDbSettings": {
    "DatabaseName": "TransactionDb",
    "CollectionName": "Transactions"
  },
  "AzureServiceBus": {
    "QueueName": "NOMEDASUAFILA"
  }
}
```

### ▶️ 3. Executar o projeto

```
dotnet build
dotnet run --project src/TransactionMicroservice
```

Acesse em https://localhost:5001/swagger para testar a API.

### 🧪 4. Executar testes

```
dotnet test tests/TransactionMicroservice.Tests
```

### 🐋 5. Executar com Docker

1. Certifique-se que o Docker e Docker Compose estão instalados.

2. No arquivo ***docker-compose.yml*** na parte de environment escreva suas strings de conexão:

    ```dockerfile
    environment:
      ASPNETCORE_ENVIRONMENT: Development
      ConnectionStrings__MongoDb: SUASTRING
      ConnectionStrings__AzureBusConnection: SUASTRING
      MongoDbSettings__DatabaseName: TransactionDB
      MongoDbSettings__CollectionName: transactions
      AzureServiceBus__QueueName: NOMEDASUAFILA
    ```

3. Na raiz do projeto, rode:

    ```bash
    docker-compose up --build
    ```
4. A API estará disponível em: http://localhost:8080/swagger

## 📦 Endpoints disponíveis
| Verbo | Rota                      | Descrição                                    |
| ----- |---------------------------|----------------------------------------------|
| POST  | `api/transactions`        | Cria uma nova transação                      |
| GET   | `api/transactions`        | Lista todas as transações                    |
| GET   | `api/transactions/{id} `  | Recupera a transação referente ao id fornecido |

## 🧠 Breve descrição

Este microsserviço permite:

- Criar transações com remetente, destinatário, valor e tipo (Credit, Debit)

- Persistir as transações no MongoDB Atlas

- Enviar os dados da transação para uma fila no Azure Service Bus (mensageria)

- Recuperar todas as transações salvas

- Recuperar transação por id

- Executar testes automatizados com xUnit

A arquitetura segue uma separação em camadas (Domain, Application, Infrastructure, API).

## 📁 Estrutura do Projeto

```
TransactionMicroservice/
├── Api/ # Camada de apresentação (Controllers HTTP)
│     └── Controllers/
│        └── TransactionController.cs # Endpoint para criação e listagem de transações
│
├── Application/ # Camada de aplicação (casos de uso e DTOs)
│     ├── DTOs/ # Objetos de transferência de dados
│     │    ├── CreateTransactionDto.cs # DTO para criação de transação
│     │    ├── TransactionDto.cs # DTO para retorno
│     │    └── TransactionMessageDto.cs # DTO usado para mensagens na fila
│     ├── Services/
│     │    └── TransactionService.cs # Serviço de aplicação
│     └── ApplicationModule.cs # Configuração da injeção de dependência
│
├── Domain/ # Camada de domínio (núcleo da aplicação)
│     ├── Entities/
│     │     └── Transaction.cs # Entidade que representa uma transação
│     ├── Enums/
│     │     ├── TransactionStatus.cs # Enum para status da transação
│     │     └── TransactionType.cs # Enum para tipo da transação
│     └── Interfaces/
│           ├── ITransactionQueueService.cs # Interface para serviço de fila
│           └── ITransactionRepository.cs # Interface para repositório de transações
│
├── Helpers/
│     └── JsonHelper.cs # Funções auxiliares para manipulação de JSON
│
└── Infrastructure/ # Camada de infraestrutura (implementações técnicas)
      ├── Configurations/
      │     ├── AzureBusServiceSettings.cs # Configurações do Azure Service Bus
      │     └── MongoDbSettings.cs # Configurações do MongoDB
      ├── Messaging/
      │     └── MessageSenderService.cs # Serviço que envia mensagens para o Azure Bus
      ├── Repositories/
      │     └── DbTransactionRepository.cs # Implementação do repositório no MongoDB
      ├── InfrastructureModule.cs # Injeção de dependência da camada infra
      └── StartupDiagnostics.cs # Verificações iniciais de saúde/configuração
```

## 🙋 Autor

### ***Desenvolvido por Lucas Matos***
