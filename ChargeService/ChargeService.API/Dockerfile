#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ChargeService.API/ChargeService.API.csproj", "ChargeService.API/"]
COPY ["ChargeService.MessageBroker/ChargeService.MessageBroker.Common.csproj", "ChargeService.MessageBroker/"]
COPY ["ChargeService.DAL/ChargeService.DAL.csproj", "ChargeService.DAL/"]
COPY ["ChargeService.Utility/ChargeService.Utility.csproj", "ChargeService.Utility/"]
COPY ["ChargeService.BLL/ChargeService.BLL.csproj", "ChargeService.BLL/"]
RUN dotnet restore "ChargeService.API/ChargeService.API.csproj"
COPY . .
WORKDIR "/src/ChargeService.API"
RUN dotnet build "ChargeService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChargeService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChargeService.API.dll"]