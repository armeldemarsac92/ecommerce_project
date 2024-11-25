FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
#COPY ["Tdev702.Api/Tdev702.Api.csproj", "Tdev702.Api/"]
COPY ["api/Tdev702.Contracts/Tdev702.Contracts.csproj", "Tdev702.Contracts/"]
COPY ["api/Tdev702.Repository/Tdev702.Repository.csproj", "Tdev702.Repository/"]
COPY ["api/Tdev702.Api/Tdev702.Api.csproj", "Tdev702.Api/"]

RUN dotnet restore "Tdev702.Api/Tdev702.Api.csproj"
RUN dotnet publish "Tdev702.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
COPY . .
WORKDIR "/src/Tdev702.Api"
RUN dotnet build "Tdev702.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Tdev702.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tdev702.Api.dll"]
