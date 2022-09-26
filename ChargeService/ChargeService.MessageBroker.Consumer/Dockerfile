#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 82
EXPOSE 445

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ChargeService.MessageBroker.Consumer/ChargeService.MessageBroker.Consumer.csproj", "ChargeService.MessageBroker.Consumer/"]
COPY ["ChargeService.Utility/ChargeService.Utility.csproj", "ChargeService.Utility/"]
COPY ["ChargeService.MessageBroker/ChargeService.MessageBroker.Common.csproj", "ChargeService.MessageBroker/"]
COPY ["ChargeService.BLL/ChargeService.BLL.csproj", "ChargeService.BLL/"]
COPY ["ChargeService.DAL/ChargeService.DAL.csproj", "ChargeService.DAL/"]
RUN dotnet restore "ChargeService.MessageBroker.Consumer/ChargeService.MessageBroker.Consumer.csproj"
COPY . .
WORKDIR "/src/ChargeService.MessageBroker.Consumer"
RUN dotnet build "ChargeService.MessageBroker.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChargeService.MessageBroker.Consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChargeService.MessageBroker.Consumer.dll"]

#
FROM alpine:latest

# Add hello scripts
ADD sayhello /sayhello
RUN chmod +x /sayhello

# Add docker-compose-wait tool -------------------
ENV WAIT_VERSION 2.7.2
ADD https://github.com/ufoscout/docker-compose-wait/releases/download/$WAIT_VERSION/wait /wait
RUN chmod +x /wait

CMD ["/sayhello"]