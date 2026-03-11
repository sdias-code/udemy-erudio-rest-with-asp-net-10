# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base

# instala curl para healthcheck
RUN apk add --no-cache curl

WORKDIR /app
EXPOSE 8080
EXPOSE 8443

# instalar dependências necessárias
RUN apk add --no-cache \
    icu-libs \
    curl

# habilitar globalization completo
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Build
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

COPY src/RestWithAspNet10-Scaffold/*.csproj ./RestWithAspNet10-Scaffold/
RUN dotnet restore ./RestWithAspNet10-Scaffold/RestWithAspNet10-Scaffold.csproj

COPY src/ ./

WORKDIR /src/RestWithAspNet10-Scaffold
RUN dotnet publish RestWithAspNet10-Scaffold.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# Final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "RestWithAspNet10-Scaffold.dll"]