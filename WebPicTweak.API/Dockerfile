FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 as debug

#install debugger for NET Core
RUN apt-get update
RUN apt-get install -y unzip
RUN curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l ~/vsdbg

WORKDIR /src

COPY ["WebPicTweak.API/WebPicTweak.API.csproj", "WebPicTweak.API/"]
COPY ["WebPicTweak.Core/WebPicTweak.Core.csproj", "WebPicTweak.Core/"]
COPY ["WebPicTweak.Infrastructure/WebPicTweak.Infrastructure.csproj", "WebPicTweak.Infrastructure/"]
COPY ["WebPicTweak.Application/WebPicTweak.Application.csproj", "WebPicTweak.Application/"]

RUN dotnet restore "WebPicTweak.API/WebPicTweak.API.csproj"

COPY . .

RUN mkdir /out/ 

RUN dotnet build WebPicTweak.API/WebPicTweak.API.csproj --configuration Debug --no-restore
RUN dotnet publish WebPicTweak.API/WebPicTweak.API.csproj --output /out --configuration Release --no-restore

ENTRYPOINT ["dotnet", "WebPicTweak.API.dll"]


FROM mcr.microsoft.com/dotnet/aspnet:9.0 as runtime

WORKDIR /app
COPY --from=debug /out/ /app
ENTRYPOINT ["dotnet", "WebPicTweak.API.dll"]


