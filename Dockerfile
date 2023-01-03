# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:7.0 as build-env
WORKDIR /src
COPY src .
RUN dotnet restore Starfish.Web/Starfish.Web.csproj
RUN dotnet publish Starfish.Web/Starfish.Web.csproj -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /publish
COPY --from=build-env /publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "Starfish.Web.dll"]