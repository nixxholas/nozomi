FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj
COPY . .

# restore as distinct layers
RUN dotnet restore Nozomi.Payment/Nozomi.Payment.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish Nozomi.Payment/Nozomi.Payment.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
COPY nozomi.pfx .

ENTRYPOINT ["dotnet", "Nozomi.Payment.dll"]
