#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 81
EXPOSE 444

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ChargeService.Kafka.Consumer/ChargeService.Kafka.Consumer.csproj", "ChargeService.Kafka.Consumer/"]
COPY ["ChargeService.Kafka.Common/ChargeService.Kafka.Common.csproj", "ChargeService.Kafka.Common/"]
COPY ["ChargeService.DAL/ChargeService.DAL.csproj", "ChargeService.DAL/"]
COPY ["ChargeService.Utility/ChargeService.Utility.csproj", "ChargeService.Utility/"]
COPY ["ChargeService.BLL/ChargeService.BLL.csproj", "ChargeService.BLL/"]
COPY ["ChargeService.MessageBroker/ChargeService.MessageBroker.Common.csproj", "ChargeService.MessageBroker/"]
RUN dotnet restore "ChargeService.Kafka.Consumer/ChargeService.Kafka.Consumer.csproj"
COPY . .
WORKDIR "/src/ChargeService.Kafka.Consumer"
RUN dotnet build "ChargeService.Kafka.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChargeService.Kafka.Consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChargeService.Kafka.Consumer.dll"]