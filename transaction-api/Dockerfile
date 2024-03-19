FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5333

ENV ASPNETCORE_URLS=http://+:5333 \ ISDOCKER=true

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["transaction-api/transaction-api.csproj", "transaction-api/"]
RUN dotnet restore "transaction-api/transaction-api.csproj"
COPY . .
WORKDIR "/src/transaction-api"
RUN dotnet build "transaction-api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "transaction-api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "transaction-api.dll"]
