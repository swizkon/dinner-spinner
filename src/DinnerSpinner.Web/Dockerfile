##See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
#

# Use Node.js 14, name this stage 'frontend'
FROM node:14 AS frontend
WORKDIR /build
COPY package.json .             
COPY package-lock.json .
RUN npm install
COPY rollup.config.js .
COPY svelte-app ./svelte-app
RUN npm run build

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS backend
WORKDIR /build
COPY DinnerSpinner.Web.csproj .
COPY ["../DinnerSpinner.Web/DinnerSpinner.Web.csproj", "DinnerSpinner.Web/"]
COPY ["../DinnerSpinner.Domain/DinnerSpinner.Domain.csproj", "DinnerSpinner.Domain/"]
COPY ["../DinnerSpinner.Infrastructure/DinnerSpinner.Infrastructure.csproj", "DinnerSpinner.Infrastructure/"]
RUN dotnet restore DinnerSpinner.Web.csproj
COPY . .
RUN dotnet publish -c Release -o /publish


#
#FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
#WORKDIR /src
#COPY ["../DinnerSpinner.Web/DinnerSpinner.Web.csproj", "DinnerSpinner.Web/"]
#COPY ["../DinnerSpinner.Domain/DinnerSpinner.Domain.csproj", "DinnerSpinner.Domain/"]
#COPY ["../DinnerSpinner.Infrastructure/DinnerSpinner.Infrastructure.csproj", "DinnerSpinner.Infrastructure/"]
#RUN dotnet restore "DinnerSpinner.Web.csproj"
#COPY . .
#WORKDIR "/src/DinnerSpinner.Web"
#RUN dotnet build "DinnerSpinner.Web.csproj" -c Release -o /app/build
#
FROM build AS publish
RUN dotnet publish "DinnerSpinner.Web.csproj" -c Release -o /app/publish

#
#FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
#WORKDIR /app
#COPY --from=frontend /build/wwwroot ./wwwroot
#COPY --from=backend /publish .
#ENTRYPOINT /app/dotnet-svelte

FROM base AS final
WORKDIR /app
COPY --from=frontend /build/wwwroot ./wwwroot
COPY --from=backend /publish .
ENTRYPOINT ["dotnet", "DinnerSpinner.Web.dll"]


# FROM nginx:1.19-alpine
# COPY --from=frontendbuild /app/wwwroot /usr/share/nginx/html