FROM --platform=$BUILDPLATFORM nginx:1.27 AS base
ARG BUILDPLATFORM
EXPOSE 80
EXPOSE 443

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILDPLATFORM

WORKDIR /src
COPY ["Directory.Build.props", "/"]
COPY ["Directory.Packages.props", "/"]
COPY ["Nuget.Config", "/"]

COPY ["Clients.Admin.Web/Admin.Web.csproj", "Clients.Admin.Web/"]
ARG BUILD_CONFIGURATION=Release
RUN dotnet restore "Clients.Admin.Web/Admin.Web.csproj"
COPY . .
WORKDIR "/src/Clients.Admin.Web"
RUN dotnet build "Admin.Web.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
RUN dotnet publish "Admin.Web.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
COPY --from=publish /app/publish/wwwroot /usr/share/nginx/html