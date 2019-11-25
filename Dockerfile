FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build-env
WORKDIR /app

# Copy csproj
COPY . .

# Copy the certificates
# RUN ls -la
# ADD ca-certificate.crt /usr/local/share/ca-certificates/ca.crt
# RUN chmod 644 /usr/local/share/ca-certificates/ca.crt && update-ca-certificates

# restore as distinct layers
RUN dotnet restore Nozomi.Compute/Nozomi.Compute.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish Nozomi.Compute/Nozomi.Compute.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
WORKDIR /app
COPY --from=build-env /app/Nozomi.Compute/out .
# COPY --from=build-env /app/Nozomi.Compute/ca-certificate.crt .
RUN ls
ENTRYPOINT ["dotnet", "Nozomi.Compute.dll"]
