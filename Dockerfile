# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1.100-buster AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY api.reserveerme.nu/*.csproj ./api.reserveerme.nu/
COPY DAL/*.csproj ./DAL/
COPY Logic/*.csproj ./Logic/
COPY Model/*.csproj ./Model/
RUN dotnet restore

# copy everything else and build app
COPY api.reserveerme.nu/. ./api.reserveerme.nu/
COPY DAL/. ./DAL/
COPY Logic/. ./Logic/
COPY Model/. ./Model/
COPY Tests/. ./Tests/
WORKDIR /source/api.reserveerme.nu
RUN dotnet publish -c release -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "api.reserveerme.nu.dll"]
