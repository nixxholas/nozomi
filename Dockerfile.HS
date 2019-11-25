FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build-env
WORKDIR /app

# Copy csproj
COPY . .

# restore as distinct layers
RUN dotnet restore Nozomi.HttpSyncing/Nozomi.HttpSyncing.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish Nozomi.HttpSyncing/Nozomi.HttpSyncing.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
WORKDIR /app
COPY --from=build-env /app/Nozomi.HttpSyncing/out .
RUN ls
ENTRYPOINT ["dotnet", "Nozomi.HttpSyncing.dll"]
