##See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use Node.js 14, name this stage 'frontend'
FROM node:14 AS frontend
WORKDIR /build
COPY DinnerSpinner.Web/package.json .             
COPY DinnerSpinner.Web/package-lock.json .
RUN npm install
COPY DinnerSpinner.Web/rollup.config.js .
COPY DinnerSpinner.Web/svelte-app ./svelte-app
RUN npm run build


FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS backend
WORKDIR /build
COPY ["DinnerSpinner.Web/DinnerSpinner.Web.csproj", "DinnerSpinner.Web/"]
COPY ["DinnerSpinner.Domain/DinnerSpinner.Domain.csproj", "DinnerSpinner.Domain/"]
COPY ["DinnerSpinner.Infrastructure/DinnerSpinner.Infrastructure.csproj", "DinnerSpinner.Infrastructure/"]
RUN dotnet restore "DinnerSpinner.Web/DinnerSpinner.Web.csproj"
COPY . .
#RUN dotnet build "DinnerSpinner.Web/DinnerSpinner.Web.csproj" -c Release -o /publish
RUN dotnet publish "DinnerSpinner.Web/DinnerSpinner.Web.csproj" -c Release -o /publish
#RUN dotnet build "DinnerSpinner.Web/DinnerSpinner.Web.csproj" -c Release -o /app/build

#FROM backend AS publish
#RUN dotnet publish "DinnerSpinner.Web/DinnerSpinner.Web.csproj" -c Release -o /publish

FROM base AS final
#FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
WORKDIR /app
COPY --from=frontend /build/wwwroot ./wwwroot
COPY --from=backend /publish .
# ENTRYPOINT /app/dotnet-svelte
ENTRYPOINT ["dotnet", "DinnerSpinner.Web.dll"]

# FROM nginx:1.19-alpine
# COPY --from=frontendbuild /app/wwwroot /usr/share/nginx/html