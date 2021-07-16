# # https://hub.docker.com/_/microsoft-dotnet
# FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
# WORKDIR /source
# # copy csproj and restore as distinct layers
# COPY *.csproj .
# RUN dotnet restore

# # copy and publish app and libraries
# COPY . .
# RUN dotnet publish -c release -o /app --no-restore

# # final stage/image
# FROM mcr.microsoft.com/dotnet/runtime:5.0
# WORKDIR /app
# EXPOSE 5004
# COPY --from=build /app .
# ENTRYPOINT ["dotnet", "backend.dll"]

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env 
WORKDIR /app  
COPY *.csproj ./ 
RUN dotnet restore  
COPY . ./ 
RUN dotnet publish -c Release -o out  
FROM mcr.microsoft.com/dotnet/aspnet:5.0 
WORKDIR /app 
# EXPOSE 80 
# EXPOSE 443 
COPY --from=build-env /app/out . 
ENTRYPOINT [ "dotnet","backend.dll" ]