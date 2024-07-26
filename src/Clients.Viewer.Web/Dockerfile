FROM  --platform=$BUILDPLATFORM nginx:1.27 AS base
ARG BUILDPLATFORM
EXPOSE 80
EXPOSE 443

FROM  --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILDPLATFORM

WORKDIR /src
COPY ["Directory.Build.props", "/"]
COPY ["Directory.Packages.props", "/"]
COPY ["Nuget.Config", "/"]

COPY ["Clients.Viewer.Web/Web.csproj", "Clients.Viewer.Web/"]
ARG BUILD_CONFIGURATION=Release
RUN dotnet restore "Clients.Viewer.Web/Web.csproj"
COPY . .
WORKDIR "/src/Clients.Viewer.Web"
RUN dotnet build "Web.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
RUN dotnet publish "Web.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
COPY --from=publish /app/publish/wwwroot /usr/share/nginx/html