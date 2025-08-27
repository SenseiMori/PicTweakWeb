# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 as debug

#install debugger for NET Core
RUN apt-get update
RUN apt-get install -y unzip
RUN curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l ~/vsdbg

WORKDIR /src

#COPY ./*/*.csproj ./
#RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done


COPY ["WebPicTweak.API/WebPicTweak.API.csproj", "WebPicTweak.API/"]
COPY ["WebPicTweak.Core/WebPicTweak.Core.csproj", "WebPicTweak.Core/"]
COPY ["WebPicTweak.Infrastructure/WebPicTweak.Infrastructure.csproj", "WebPicTweak.Infrastructure/"]
COPY ["WebPicTweak.Application/WebPicTweak.Application.csproj", "WebPicTweak.Application/"]

RUN dotnet restore "WebPicTweak.API/WebPicTweak.API.csproj"

COPY . .
RUN dotnet build WebPicTweak.API/WebPicTweak.API.csproj --configuration Debug --no-restore

RUN mkdir /out/
RUN dotnet publish WebPicTweak.API/WebPicTweak.API.csproj --output /out --configuration Release --no-restore

ENTRYPOINT ["dotnet", "WebPicTweak.API.dll"]

FROM base AS final
WORKDIR /app
COPY --from=debug /out/ /app	
ENTRYPOINT ["dotnet", "WebPicTweak.API.dll"]









#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS debug
#RUN apt-get update
#RUN apt-get install -y unzip
#RUN curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l ~/vsdbg
#
#RUN mkdir /src/
#WORKDIR /src/
#
#COPY ./*.sln ./
#
#COPY ./*/*.csproj ./
#RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done
##RUN dotnet restore
#RUN dotnet restore "./WebPicTweak.API/WebPicTweak.API.csproj"
#
#COPY . /src/
#RUN mkdir /out/
#RUN dotnet build --no-restore --output /out/ --configuration Debug
#
#ENTRYPOINT ["dotnet", "WebPicTweak.API.dll"]
#
## Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
#WORKDIR /app
#COPY --from=debug /out/ /app	
#ENTRYPOINT ["dotnet", "WebPicTweak.API.dll"]














#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#WORKDIR /WebPicTweak
#
#COPY ./*.sln ./
#
#COPY ./*/*.csproj ./
#RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done
#
#RUN dotnet restore ./WebPicTweak.API/WebPicTweak.API.csproj
#
#COPY . .
#
#RUN dotnet build ./WebPicTweak.API/WebPicTweak.API.csproj --configuration Debug --no-restore
#RUN dotnet publish ./WebPicTweak.API/WebPicTweak.API.csproj --output /out --configuration Debug --no-restore
#
## Финальный образ
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
#WORKDIR /app
#COPY --from=build /out/ /app
#ENTRYPOINT ["dotnet", "WebPicTweak.API.dll"]