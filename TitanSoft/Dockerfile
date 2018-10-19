FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY TitanSoft/TitanSoft.Api.csproj TitanSoft/
COPY TitanSoft.Models/TitanSoft.Models.csproj TitanSoft.Models/
COPY TitanSoft.DataAccess/TitanSoft.DataAccess.csproj TitanSoft.DataAccess/
RUN dotnet restore TitanSoft/TitanSoft.Api.csproj
COPY . .
WORKDIR /src/TitanSoft
RUN dotnet build TitanSoft.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish TitanSoft.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TitanSoft.Api.dll"]
