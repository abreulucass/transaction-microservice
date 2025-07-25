
# Etapa Base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Etapa Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o .csproj e restaura dependências
COPY ["src/TransactionMicroservice/TransactionMicroservice.csproj", "src/TransactionMicroservice/"]
RUN dotnet restore "src/TransactionMicroservice/TransactionMicroservice.csproj"

# Copia todo o restante do código
COPY . .

# Build do projeto
WORKDIR "/src/src/TransactionMicroservice"
RUN dotnet build "./TransactionMicroservice.csproj" -c Release -o /app/build

# Etapa de publish
FROM build AS publish
RUN dotnet publish "./TransactionMicroservice.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TransactionMicroservice.dll"]
