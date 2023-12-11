FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Avian.csproj", "./"]
COPY ["../Avian.Domain/Avian.Domain.csproj", "../Avian.Domain/"]
COPY ["../Avian.Dal/Avian.Dal.csproj", "../Avian.Dal/"]
COPY ["../Avian.Application/Avian.Application.csproj", "../Avian.Application/"]
RUN dotnet restore "Avian.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "Avian.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Avian.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Avian.dll"]
