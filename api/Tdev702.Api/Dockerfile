FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

USER root
RUN apt-get update && apt-get install -y \
  curl \
  telnet \
  dnsutils \
  netcat-openbsd \
  iputils-ping \
  net-tools \
  unzip \
  && curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip" \
  && unzip awscliv2.zip \
  && ./aws/install \
  && rm -rf /var/lib/apt/lists/* aws awscliv2.zip

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . .

RUN echo "Contents of /src after COPY:" && \
    ls -la && \
    echo "Contents of /src/Tdev702.Api (if it exists):" && \
    ls -la Tdev702.Api || echo "Tdev702.Api directory not found"

RUN dotnet restore "Tdev702.Api/Tdev702.Api.csproj"

RUN dotnet build "Tdev702.Api/Tdev702.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Tdev702.Api/Tdev702.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src/Tdev702.Api/appsettings*.json ./
ENTRYPOINT ["dotnet", "Tdev702.Api.dll"]