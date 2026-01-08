# Estágio de Build: Utiliza o SDK 9 para suportar o formato .slnx
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

# Copia o arquivo da solução .slnx
COPY ["FCG.Comunicador.Getway.Pagamento.slnx", "./"]

# Copia os arquivos de projeto (.csproj) mantendo a estrutura de pastas
COPY ["src/FCG.Comunicador.Business/FCG.Comunicador.Business.csproj", "src/FCG.Comunicador.Business/"]
COPY ["src/FCG.Comunicador.Domain/FCG.Comunicador.Domain.csproj", "src/FCG.Comunicador.Domain/"]
COPY ["src/FCG.Comunicador.Getway.Pagamento/FCG.Comunicador.Getway.Pagamento.csproj", "src/FCG.Comunicador.Getway.Pagamento/"]
COPY ["src/FCG.Comunicador.Infra/FCG.Comunicador.Infra.csproj", "src/FCG.Comunicador.Infra/"]
COPY ["src/FCG.Comunicador.Service/FCG.Comunicador.Service.csproj", "src/FCG.Comunicador.Service/"]
COPY ["tests/FCG.Comunicador.Getway.Pagamento.Tests/FCG.Comunicador.Getway.Pagamento.Tests.csproj", "tests/FCG.Comunicador.Getway.Pagamento.Tests/"]

# Agora o restore via .slnx irá funcionar, pois o SDK 9 reconhece o formato
RUN dotnet restore "FCG.Comunicador.Getway.Pagamento.slnx"

# Copia todo o código-fonte
COPY . .

# Publica o projeto (o SDK 9 compila perfeitamente o código escrito em .NET 8)
WORKDIR "/src/src/FCG.Comunicador.Getway.Pagamento"
RUN dotnet publish "FCG.Comunicador.Getway.Pagamento.csproj" -c Release -o /app/publish --no-restore

# Estágio Final: Utiliza o Runtime do .NET 8 para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app


# 1. Configurações de Sistema (Alpine)
RUN apk add --no-cache \
    icu-libs \
    tzdata
	
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    TZ=America/Sao_Paulo \
    LC_ALL=pt_BR.UTF-8 \
    LANG=pt_BR.UTF-8

# Copia a publicação do estágio anterior
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "FCG.Comunicador.Getway.Pagamento.dll"]