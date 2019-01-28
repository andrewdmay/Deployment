FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Deployment.Api/Deployment.Api.csproj /src/Deployment.Api/
RUN dotnet restore /src/Deployment.Api/Deployment.Api.csproj
COPY . .
WORKDIR /src/Deployment.Api
RUN dotnet build Deployment.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Deployment.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
CMD ["dotnet", "Deployment.Api.dll"]
