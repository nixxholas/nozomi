FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY . .
RUN dotnet restore Nozomi.Web2/Nozomi.Web2.csproj

# Required libraries in Unix
RUN apt-get update -q && apt-get install -q -y \
        curl apt-transport-https apt-utils dialog \
        make g++ build-essential

# Node Bash Script for Debian
# https://github.com/nodesource/distributions#deb
RUN curl -sL https://deb.nodesource.com/setup_12.x | bash -

# Propagate Node for Docker
RUN apt-get update \
    && apt-get install -y nodejs

# Copy everything else and build
RUN dotnet publish Nozomi.Web2/Nozomi.Web2.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Nozomi.Web2.dll"]
