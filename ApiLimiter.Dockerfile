FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj
COPY . .

# restore as distinct layers
RUN dotnet restore Nozomi.Api.Limiter/Nozomi.Api.Limiter.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish Nozomi.Api.Limiter/Nozomi.Api.Limiter.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Nozomi.Api.Limiter.dll"]
