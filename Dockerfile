FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Mocker.csproj", ""]
RUN dotnet restore "Mocker.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "Mocker.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Mocker.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app

COPY --from=publish /app .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Mocker.dll