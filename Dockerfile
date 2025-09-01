#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

#windows docker file
#####################################

# FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-1809 AS base
# WORKDIR /app
# EXPOSE 8080
# EXPOSE 8081

# FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
# ARG BUILD_CONFIGURATION=Release
# WORKDIR /src
# COPY ["Ultimate POS Api.csproj", "."]
# RUN dotnet restore "./Ultimate POS Api.csproj"
# COPY . .
# WORKDIR "/src/."
# RUN dotnet build "./Ultimate POS Api.csproj" -c %BUILD_CONFIGURATION% -o /app/build

# FROM build AS publish
# ARG BUILD_CONFIGURATION=Release
# RUN dotnet publish "./Ultimate POS Api.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "Ultimate POS Api.dll"]

#linux docker file

# Base image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image for restoring dependencies and building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Ultimate POS Api.csproj", "."]
RUN dotnet restore "./Ultimate POS Api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./Ultimate POS Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the app
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ultimate POS Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image to run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ultimate POS Api.dll"]
