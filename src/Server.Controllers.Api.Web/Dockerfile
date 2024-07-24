FROM --platform=$BUILDPLATFORM  mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ARG BUILDPLATFORM
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM --platform=$BUILDPLATFORM  mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILDPLATFORM
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY Directory.Build.props .
COPY Directory.Packages.props .
COPY Nuget.Config .
COPY ["Server.Controllers.Api.Web/Api.Web.csproj", "Server.Controllers.Api.Web/"]
RUN dotnet restore "Server.Controllers.Api.Web/Api.Web.csproj"
COPY . .
WORKDIR "/src/Server.Controllers.Api.Web"
RUN dotnet build "Api.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Api.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MadWorldNL.MantaRayPlan.Api.Web.dll"]
