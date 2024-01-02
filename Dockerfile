#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/WebApi.Api/WebApi.Api.csproj", "src/WebApi.Api/"]
COPY ["src/WebApi.DTO/WebApi.DTO.csproj", "src/WebApi.DTO/"]
COPY ["src/WebApi.Core/WebApi.Core.csproj", "src/WebApi.Core/"]
RUN dotnet restore "src/WebApi.Api/WebApi.Api.csproj"
COPY . .
WORKDIR "/src/src/WebApi.Api"
RUN dotnet build "WebApi.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./WebApi.Api.csproj" -c Release -o ./out/

FROM base AS final
WORKDIR /app
COPY --from=publish /app/out/ .
RUN apt-get update && apt-get install -y curl
ENTRYPOINT ["dotnet", "WebApi.Api.dll"]
