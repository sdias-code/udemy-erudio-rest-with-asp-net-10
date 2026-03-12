# =========================
# Runtime base
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base

WORKDIR /app

# Instalar dependências necessárias
RUN apk add --no-cache \
    curl \
    icu-libs

# Habilitar suporte completo de globalization
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Expor portas da aplicação
EXPOSE 8080
EXPOSE 8443

# Healthcheck da aplicação
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
CMD curl -f http://localhost:8080/health || exit 1


# =========================
# Build da aplicação
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build

WORKDIR /src

# Copia apenas o csproj primeiro (melhora cache do Docker)
COPY src/RestWithAspNet10-Scaffold/*.csproj ./RestWithAspNet10-Scaffold/

RUN dotnet restore ./RestWithAspNet10-Scaffold/RestWithAspNet10-Scaffold.csproj

# Copia o restante do código
COPY src/ ./

WORKDIR /src/RestWithAspNet10-Scaffold

# Publicação da aplicação
RUN dotnet publish RestWithAspNet10-Scaffold.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false


# =========================
# Imagem final
# =========================
FROM base AS final

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "RestWithAspNet10-Scaffold.dll"]