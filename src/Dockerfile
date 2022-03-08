##See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use Node.js 14, name this stage 'frontend'
FROM node:14 AS frontend

# Our working directory within the image
WORKDIR /src
#WORKDIR "/src/DinnerSpinner.Web"
# Copy package and lock files then install dependencies
COPY package.json .             
COPY package-lock.json .
RUN npm install

# Copy the rest of the files for the frontend
COPY rollup.config.js .
COPY svelte-app ./svelte-app

# Build - this'll output files to /build/wwwroot
RUN npm run build


FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DinnerSpinner.Web/DinnerSpinner.Web.csproj", "DinnerSpinner.Web/"]
COPY ["DinnerSpinner.Domain/DinnerSpinner.Domain.csproj", "DinnerSpinner.Domain/"]
COPY ["DinnerSpinner.Infrastructure/DinnerSpinner.Infrastructure.csproj", "DinnerSpinner.Infrastructure/"]
RUN dotnet restore "DinnerSpinner.Web/DinnerSpinner.Web.csproj"
COPY . .
WORKDIR "/src/DinnerSpinner.Web"
RUN dotnet build "DinnerSpinner.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DinnerSpinner.Web.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=frontend /app/wwwroot .
ENTRYPOINT ["dotnet", "DinnerSpinner.Web.dll"]


# FROM nginx:1.19-alpine
# COPY --from=frontendbuild /app/wwwroot /usr/share/nginx/html