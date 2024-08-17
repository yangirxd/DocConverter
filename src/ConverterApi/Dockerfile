FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


RUN apt update
RUN apt install -y wget
RUN apt install -y default-jre
RUN apt-get install -y libreoffice-core --no-install-recommends
RUN apt-get install -y libreoffice-writer
RUN apt-get install -y libreoffice-calc
RUN apt-get install -y cabextract
RUN apt-get install -y xfonts-utils
RUN apt-get install -y fonts-crosextra-carlito
RUN apt install -y fonts-liberation
#RUN sed -i'.bak' 's/$/ contrib/' /etc/apt/sources.list
#RUN apt-get update; apt-get install -y ttf-mscorefonts-installer fontconfig
#RUN fc-cache -f -v

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Converter.Api/Converter.Api.csproj", "src/Converter.Api/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
RUN dotnet restore "src/Converter.Api/Converter.Api.csproj"
COPY . .
WORKDIR "/src/src/Converter.Api"
RUN dotnet build "Converter.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Converter.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Converter.Api.dll"]
