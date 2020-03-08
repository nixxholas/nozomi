FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj
COPY . .

# Copy the certificates
# RUN ls -la
# ADD ca-certificate.crt /usr/local/share/ca-certificates/ca.crt
# RUN chmod 644 /usr/local/share/ca-certificates/ca.crt && update-ca-certificates

# restore as distinct layers
RUN dotnet restore Nozomi.Compute2/Nozomi.Compute2.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish Nozomi.Compute2/Nozomi.Compute2.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
# COPY --from=build-env /app/Nozomi.Compute2/ca-certificate.crt .
RUN ls
ENTRYPOINT ["dotnet", "Nozomi.Compute2.dll"]
