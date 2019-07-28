FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY . .
RUN dotnet restore Nozomi.Web/Nozomi.Web.csproj

# Propagate Docker for node
RUN apt-get update -yq && apt-get upgrade -yq && apt-get install -yq python g++ make curl git nano

# Propagate Node for Docker
RUN apt-get install -yq nodejs build-essential

# Copy everything else and build
RUN dotnet publish Nozomi.Web/Nozomi.Web.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build-env /app/Nozomi.Web/out .
ENTRYPOINT ["dotnet", "Nozomi.Web.dll"]
