FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 as debug

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


FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime

WORKDIR /app
COPY --from=debug /out/ /app
ENTRYPOINT ["dotnet", "WebPicTweak.API.dll"]

## =============================
## Base image (runtime)
## =============================
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#WORKDIR /app
#EXPOSE 8080
##EXPOSE 8081
#
## =============================
## Build image
## =============================
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Debug
#WORKDIR /src
#
## Копируем только csproj (для кэша restore)
#COPY ["WebPicTweak.API/WebPicTweak.API.csproj", "WebPicTweak.API/"]
#COPY ["WebPicTweak.Core/WebPicTweak.Core.csproj", "WebPicTweak.Core/"]
#COPY ["WebPicTweak.Infrastructure/WebPicTweak.Infrastructure.csproj", "WebPicTweak.Infrastructure/"]
#COPY ["WebPicTweak.Application/WebPicTweak.Application.csproj", "WebPicTweak.Application/"]
#
## Восстановление зависимостей
#RUN dotnet restore "WebPicTweak.API/WebPicTweak.API.csproj"
#
## Копируем весь код
#COPY . .
#
#WORKDIR "/src/WebPicTweak.API"
#
## Сборка (Debug)
#RUN dotnet build "WebPicTweak.API.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
## =============================
## Publish (с pdb-шками)
## =============================
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Debug
#RUN dotnet publish "WebPicTweak.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish \
    #/p:UseAppHost=false \
    #/p:DebugType=portable \
    #/p:DebugSymbols=true
#
## =============================
## Debug tools (vsdbg)
## =============================
#FROM base AS debug
#RUN apt-get update && apt-get install -y unzip curl \
    #&& curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l /vsdbg
#
#WORKDIR /app
#COPY --from=publish /app/publish .
#
#ENTRYPOINT ["dotnet", "WebPicTweak.API.dll"]
#
## =============================
## Final (release runtime)
## =============================
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "WebPicTweak.API.dll"]
#