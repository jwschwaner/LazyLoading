FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LazyLoading.Api/LazyLoading.Api.csproj", "LazyLoading.Api/"]
COPY ["LazyLoading.Application/LazyLoading.Application.csproj", "LazyLoading.Application/"]
COPY ["LazyLoading.Domain/LazyLoading.Domain.csproj", "LazyLoading.Domain/"]
COPY ["LazyLoading.Infrastructure/LazyLoading.Infrastructure.csproj", "LazyLoading.Infrastructure/"]
RUN dotnet restore "LazyLoading.Api/LazyLoading.Api.csproj"
COPY . .
WORKDIR "/src/LazyLoading.Api"
RUN dotnet build "./LazyLoading.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LazyLoading.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create a script to wait for the database and start the API
RUN echo '#!/bin/sh \n\
echo "Starting LazyLoading API..." \n\
exec dotnet LazyLoading.Api.dll \n\
' > /app/entrypoint.sh && chmod +x /app/entrypoint.sh

ENTRYPOINT ["/app/entrypoint.sh"]
