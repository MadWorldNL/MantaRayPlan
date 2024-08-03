
FROM  --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ARG BUILDPLATFORM
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM  --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILDPLATFORM
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Server.Controllers.Api.MessageBus/Api.MessageBus.csproj", "Server.Controllers.Api.MessageBus/"]
RUN dotnet restore "Server.Controllers.Api.MessageBus/Api.MessageBus.csproj"
COPY . .
WORKDIR "/src/Server.Controllers.Api.MessageBus"
RUN dotnet build "Api.MessageBus.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Api.MessageBus.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MadWorldNL.MantaRayPlan.Api.MessageBus.dll"]