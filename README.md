# FCG Comunicador Gateway Pagamento

## 📖 Descrição

Este projeto é um microserviço de comunicação com um gateway de pagamento. Ele funciona como um serviço de background (worker service) que consome mensagens de uma fila do RabbitMQ. Cada mensagem representa uma solicitação de pagamento que é processada e enviada para um gateway de pagamento externo.

O serviço é responsável por:
- Receber solicitações de pagamento de forma assíncrona.
- Processar os dados do pagamento.
- Comunicar-se com o gateway de pagamento.
- Atualizar o status do pagamento no banco de dados.

## 🏛️ Arquitetura

O projeto segue uma arquitetura limpa (Clean Architecture), dividida nas seguintes camadas:

- `FCG.Comunicador.Domain`: Contém as entidades e objetos de valor do domínio.
- `FCG.Comunicador.Business`: Contém a lógica de negócio principal e as interfaces dos repositórios.
- `FCG.Comunicador.Service`: Orquestra as operações, utilizando a lógica de negócio.
- `FCG.Comunicador.Infra`: Implementa a infraestrutura, como acesso ao banco de dados (usando Entity Framework Core) e repositórios.
- `FCG.Comunicador.Getway.Pagamento`: O projeto principal (worker service) que hospeda o consumidor da fila e inicializa a aplicação.

## 🛠️ Tecnologias Utilizadas

- **.NET 8**: Framework principal para o desenvolvimento da aplicação.
- **Worker Service**: Para processamento em background.
- **RabbitMQ**: Sistema de mensageria para comunicação assíncrona.
- **Entity Framework Core**: ORM para persistência de dados.
- **Docker**: Para containerização da aplicação.

## 🚀 Como Executar

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop) (para execução em contêiner)
- Uma instância do RabbitMQ em execução.

### Configuração

1.  **Clone o repositório:**
    ```bash
    git clone <url-do-repositorio>
    cd FCG.Comunicador.Getway.Pagamento
    ```

2.  **Configure a conexão:**
    Atualize o arquivo `appsettings.json` com as informações de conexão para o seu banco de dados e para o RabbitMQ.

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Sua-String-De-Conexao-Aqui"
      },
      "ConfigFila": {
      "ConnectionString": "endereco.servicebus",
        "NomeFila": "payment-requests"
      },
      "NewRelic": {
        "LicenseKey": "Sua licenca"
      }
    }
    ```

### Executando com .NET CLI

1.  Restaure as dependências:
    ```bash
    dotnet restore FCG.Comunicador.Getway.Pagamento.slnx
    ```

2.  Execute o serviço:
    ```bash
    dotnet run --project src/FCG.Comunicador.Getway.Pagamento/FCG.Comunicador.Getway.Pagamento.csproj
    ```

### Executando com Docker

1.  Construa a imagem Docker:
    ```bash
    docker build -t fcg-comunicador-pagamento .
    ```

2.  Execute o contêiner:
    ```bash
    docker run -d \
      -e ConnectionStrings__DefaultConnection="Sua-String-De-Conexao" \
      -e RabbitMqConfig__HostName="host-do-rabbitmq" \
      --name comunicador-pagamento \
      fcg-comunicador-pagamento
    ```
    > **Nota:** Use as variáveis de ambiente para passar as configurações sensíveis para o contêiner.

## autores

- **Gabriel F.C.G.**
