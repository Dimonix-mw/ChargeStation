#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TelegramBot.Service/TelegramBot.Service.csproj", "TelegramBot.Service/"]
COPY ["TelegramBot.Service.Kafka.Common/TelegramBot.Service.Kafka.Common.csproj", "TelegramBot.Service.Kafka.Common/"]
RUN dotnet restore "TelegramBot.Service/TelegramBot.Service.csproj"
COPY . .
WORKDIR "/src/TelegramBot.Service"
RUN dotnet build "TelegramBot.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TelegramBot.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TelegramBot.Service.dll"]