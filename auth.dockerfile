FROM microsoft/dotnet:1.1.2-sdk

MAINTAINER Calder Hayes

WORKDIR /src/Auth

ENV ASPNETCORE_URLS=http://*:5050

EXPOSE 5050:5050

ENTRYPOINT dotnet restore && dotnet run