FROM microsoft/dotnet:2.2-sdk
WORKDIR /app
COPY src .

# install System.Drawing native dependencies
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
     && rm -rf /var/lib/apt/lists/*

ENTRYPOINT [ "dotnet" , "Nozomi.Ticker.dll" ]
