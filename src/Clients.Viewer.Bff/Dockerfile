FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ARG BUILDPLATFORM
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILDPLATFORM
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY Directory.Build.props .
COPY Directory.Packages.props .
COPY Nuget.Config .

COPY ["Clients.Viewer.Bff/Bff.csproj", "Clients.Viewer.Bff/"]
RUN dotnet restore "Clients.Viewer.Bff/Bff.csproj"
COPY . .
WORKDIR "/src/Clients.Viewer.Bff"
RUN dotnet build "Bff.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Bff.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MadWorldNL.MantaRayPlan.Viewer.Bff.dll"]
