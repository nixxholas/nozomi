FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT Production

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY . .
RUN ls
RUN dotnet restore Nozomi.Analysis/Nozomi.Analysis.csproj
WORKDIR "/src/Nozomi.Analysis"
RUN dotnet build "Nozomi.Analysis.csproj" -c Release -o /app

FROM build AS publish
WORKDIR "/src/Nozomi.Analysis"
RUN dotnet publish "Nozomi.Analysis.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Nozomi.Analysis.dll", "--server.urls", "http://0.0.0.0:80"]
