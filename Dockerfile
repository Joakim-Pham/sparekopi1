FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY SparekopiAdmin/SparekopiAdmin.csproj SparekopiAdmin/
RUN dotnet restore SparekopiAdmin/SparekopiAdmin.csproj
COPY SparekopiAdmin/ SparekopiAdmin/
RUN dotnet publish SparekopiAdmin/SparekopiAdmin.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
RUN mkdir -p /data
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "SparekopiAdmin.dll"]
