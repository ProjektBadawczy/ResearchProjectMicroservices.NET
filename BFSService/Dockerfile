FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BFSService/BFSService.csproj", "BFSService/"]
RUN dotnet restore "BFSService/BFSService.csproj"
COPY . .
WORKDIR "/src/BFSService"
RUN dotnet build "BFSService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BFSService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BFSService.dll"]
