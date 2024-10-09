FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/FinanceApp.WebApi/FinanceApp.WebApi.csproj", "src/FinanceApp.WebApi/"]
COPY ["src/FinanceApp.Application/FinanceApp.Application.csproj", "src/FinanceApp.Application/"]
COPY ["src/FinanceApp.Domain/FinanceApp.Domain.csproj", "src/FinanceApp.Domain/"]
COPY ["src/FinanceApp.Infrastructure/FinanceApp.Infrastructure.csproj", "src/FinanceApp.Infrastructure/"]
RUN dotnet restore "src/FinanceApp.WebApi/FinanceApp.WebApi.csproj"
COPY . .
WORKDIR "/src/src/FinanceApp.WebApi"
RUN dotnet build "FinanceApp.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build
    
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FinanceApp.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinanceApp.WebApi.dll"]
