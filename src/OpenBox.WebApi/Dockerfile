FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ["src/OpenBox.WebApi/", "OpenBox.WebApi/"]
COPY ["src/OpenBox.Domain/", "OpenBox.Domain/"]
COPY ["src/OpenBox.Application/", "OpenBox.Application/"]
COPY ["src/OpenBox.Persistence/", "OpenBox.Persistence/"]
RUN dotnet restore "OpenBox.WebApi/OpenBox.WebApi.csproj"
WORKDIR "/src/OpenBox.WebApi"
RUN dotnet build "OpenBox.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenBox.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenBox.WebApi.dll"]
