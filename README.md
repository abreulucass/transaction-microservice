# Microsserviço de Gerenciamento de Transações

Este projeto é um microsserviço desenvolvido em **.NET 8**, seguindo os princípios de **Clean Architecture**, que permite **registrar** e **listar** transações financeiras.  
Ele utiliza **MongoDB Atlas** para persistência e **Azure Service Bus** para comunicação assíncrona entre serviços.

Esse desafio foi desenvolvido como parte do Desafio Talent Lab 2025 - Bemol Digital.

---

## ⚙️ Tecnologias utilizadas

- ASP.NET Core 8
- MongoDB Atlas
- Azure Service Bus
- xUnit + Moq (testes)
- Clean Architecture

---

## 🚀 Como executar o projeto

### ✅ Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- Conta no [MongoDB Atlas](https://www.mongodb.com/cloud/atlas)
- Conta no [Microsoft Azure](https://azure.microsoft.com/)
- Visual Studio, VS Code ou Rider

---

### 🔐 1. Configurar variáveis no `appsettings.json`

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

### ▶️ 2. Executar o projeto

```
dotnet build
dotnet run --project src/TransactionMicroservice
```

Acesse em https://localhost:5001/swagger para testar a API.

### 🧪 3. Executar testes

```
dotnet test tests/TransactionMicroservice.Tests
```
## 📦 Endpoints disponíveis
| Verbo | Rota            | Descrição                 |
| ----- | --------------- | ------------------------- |
| POST  | `/transactions` | Cria uma nova transação   |
| GET   | `/transactions` | Lista todas as transações |

## 🧠 Breve descrição

Este microsserviço permite:

 - Criar transações com remetente, destinatário, valor e tipo (Credit, Debit)

- Persistir as transações no MongoDB Atlas

- Enviar os dados da transação para uma fila no Azure Service Bus (mensageria)

- Recuperar todas as transações salvas

- Executar testes automatizados com xUnit

A arquitetura segue uma separação em camadas (Domain, Application, Infrastructure, API).

## 📁 Estrutura do Projeto

```
TransactionMicroservice/
│
├── src/
│   ├── TransactionMicroservice.Api          # Camada de entrada (controllers)
│   ├── TransactionMicroservice.Application  # Regras de negócio e serviços
│   ├── TransactionMicroservice.Domain       # Entidades, enums e interfaces
│   └── TransactionMicroservice.Infrastructure # Repositórios, mensageria, configs
│
├── tests/
│   └── TransactionMicroservice.Tests        # Testes unitários
│
└── README.md
```

## 🙋 Autor

### ***Desenvolvido por Lucas Matos***
