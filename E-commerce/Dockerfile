#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["E-commerce/E-commerce.csproj", "E-commerce/"]
COPY ["E-commerce.BLL/E-commerce.BLL.csproj", "E-commerce.BLL/"]
COPY ["E-commerce.DAL/E-commerce.DAL.csproj", "E-commerce.DAL/"]
COPY ["E-commerce.Models/E-commerce.Models.csproj", "E-commerce.Models/"]
RUN dotnet restore "E-commerce/E-commerce.csproj"
COPY . .
WORKDIR "/src/E-commerce"
RUN dotnet build "E-commerce.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "E-commerce.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "E-commerce.dll"]