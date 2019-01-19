FROM microsoft/dotnet:2.2-sdk
WORKDIR /app

COPY . ./

RUN dotnet restore

# copy and build everything else
RUN dotnet publish -c Release -o out

ENTRYPOINT [ "dotnet" , "Nozomi.Ticker.dll" ]
