FROM microsoft/dotnet:1.1.2-sdk

MAINTAINER Calder Hayes


WORKDIR /src/Data.PostgreSQL
ENTRYPOINT dotnet restore && dotnet ef database update