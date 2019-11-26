FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY . .
RUN dotnet restore Nozomi.Web/Nozomi.Web.csproj

# Required libraries in Unix
RUN apt-get update -q && apt-get install -q -y \
        curl apt-transport-https apt-utils dialog

# Node Bash Script for Debian
# https://github.com/nodesource/distributions#deb
RUN curl -sL https://deb.nodesource.com/setup_12.x | bash -

# Propagate Node for Docker
RUN apt-get update && apt-get upgrade -y && \
    apt-get install -y nodejs \
    npm node-gyp libnode-dev  # note this one

# Copy everything else and build
RUN dotnet publish Nozomi.Web/Nozomi.Web.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Nozomi.Web.dll"]
