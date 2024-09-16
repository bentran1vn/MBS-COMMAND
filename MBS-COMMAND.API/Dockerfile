FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MBS-COMMAND.API/MBS-COMMAND.API.csproj", "MBS-COMMAND.API/"]
COPY ["MBS_COMMAND.Presentation/MBS_COMMAND.Presentation.csproj", "MBS_COMMAND.Presentation/"]
COPY ["MBS_COMMAND.Application/MBS_COMMAND.Application.csproj", "MBS_COMMAND.Application/"]
COPY ["MBS_COMMAND.Contract/MBS_COMMAND.Contract.csproj", "MBS_COMMAND.Contract/"]
COPY ["MBS_COMMAND.Domain/MBS_COMMAND.Domain.csproj", "MBS_COMMAND.Domain/"]
COPY ["MBS_COMMAND.Persistence/MBS_COMMAND.Persistence.csproj", "MBS_COMMAND.Persistence/"]
COPY ["MBS_COMMAND.Infrastucture/MBS_COMMAND.Infrastucture.csproj", "MBS_COMMAND.Infrastucture/"]
RUN dotnet restore "MBS-COMMAND.API/MBS-COMMAND.API.csproj"
COPY . .
WORKDIR "/src/MBS-COMMAND.API"
RUN dotnet build "MBS-COMMAND.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MBS-COMMAND.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MBS-COMMAND.API.dll"]
