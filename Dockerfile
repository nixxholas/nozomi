FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY . .
RUN dotnet restore Nozomi.Analysis/Nozomi.Analysis.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish Nozomi.Analysis.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Nozomi.Analysis.dll"]