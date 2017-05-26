# # docker-machine env default
# # docker run -v /foo/bar:/src -p 3000:3000 brikis98/my-rails-app

# #FROM ubuntu:16.04
# FROM microsoft/dotnet:runtime

# RUN sh -c 'echo "deb [arch=amd64] https://apt-mo.trafficmanager.net/repos/dotnet-release/ xenial main" > /etc/apt/sources.list.d/dotnetdev.list'
# RUN apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 417A0893

# # Install DotNet
# RUN apt-get update && apt-get install -y dotnet

# # Provided by host
# #WORKDIR /src/

# CMD ["dotnet", "restore", "/src/Api"]
# CMD ["dotnet", "build", "/src/Api"]
# CMD ["dotnet", "run", "--project", "/src/Api"]

# EXPOSE 5080
# # CMD ["dotnet", "restore", "/src/Auth"]


#https://marketplace.visualstudio.com/items?itemName=PeterJausovec.vscode-docker
#https://hub.docker.com/r/microsoft/dotnet/
#https://hub.docker.com/r/microsoft/aspnetcore/
#https://hub.docker.com/r/microsoft/aspnetcore-build/


# docker build -t calderhayes/dotnet-boilerplate-dev .
# docker run -t -p 5080:5080 calderhayes/dotnet-boilerplate-dev

# https://github.com/DanWahlin/AspNetCorePostgreSQLDockerApp/blob/master/src/AspNetCorePostgreSQLDockerApp/docker-compose.yml


FROM microsoft/dotnet:1.1.2-sdk

MAINTAINER Calder Hayes

WORKDIR /src/Api

ENV DOTNET_USE_POLLING_FILE_WATCHER=1
ENV ASPNETCORE_URLS=http://*:5080

EXPOSE 5080:5080

ENTRYPOINT dotnet restore && dotnet watch run








#RUN dotnet restore /src/Api

#ENTRYPOINT ["dotnet", "run", "--project", "/src/Api"]

#RUN dotnet run

# COPY *.csproj .
# RUN dotnet restore

# COPY ./src .
# RUN dotnet build



#FROM microsoft/aspnetcore
#WORKDIR /src/Api
#COPY . .
