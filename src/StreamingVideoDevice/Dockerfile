#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
RUN apt update
RUN apt install -y ffmpeg
WORKDIR /app
EXPOSE 5558

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["StreamingVideoDevice/StreamingVideoDevice.csproj", "StreamingVideoDevice/"]
RUN dotnet restore "StreamingVideoDevice/StreamingVideoDevice.csproj"
COPY . .
WORKDIR "/src/StreamingVideoDevice"
RUN dotnet build "StreamingVideoDevice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StreamingVideoDevice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StreamingVideoDevice.dll"]