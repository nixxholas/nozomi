FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

ADD ca-certificate.crt /usr/local/share/ca-certificates/ca.crt
RUN chmod 644 /usr/local/share/ca-certificates/ca.crt && update-ca-certificates

# Copy csproj and restore as distinct layers
COPY . .
RUN dotnet restore Nozomi.Analysis/Nozomi.Analysis.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish Nozomi.Analysis/Nozomi.Analysis.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build-env /app/Nozomi.Analysis/out .
RUN ls
ENTRYPOINT ["dotnet", "Nozomi.Analysis.dll"]
