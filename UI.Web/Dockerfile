FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UI.Web/UI.Web.csproj", "UI.Web/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Framework/Framework.csproj", "Framework/"]
COPY ["ToDo.Data/ToDo.Data.csproj", "ToDo.Data/"]
RUN dotnet restore "./UI.Web/./UI.Web.csproj"
COPY . .
WORKDIR "/src/UI.Web"
RUN dotnet build "./UI.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UI.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UI.Web.dll"]