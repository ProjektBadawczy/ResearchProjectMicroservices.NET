FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EdmondsKarpService/EdmondsKarpService.csproj", "EdmondsKarpService/"]
RUN dotnet restore "EdmondsKarpService/EdmondsKarpService.csproj"
COPY . .
WORKDIR "/src/EdmondsKarpService"
RUN dotnet build "EdmondsKarpService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EdmondsKarpService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EdmondsKarpService.dll"]
