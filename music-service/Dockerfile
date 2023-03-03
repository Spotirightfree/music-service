#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 38534

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["music-service.csproj", "music-service/"]
RUN dotnet restore "music-service/music-service.csproj"
COPY . music-service/music-service
WORKDIR "/src/music-service/music-service"
RUN dotnet build "music-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "music-service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:38534
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "music-service.dll"]