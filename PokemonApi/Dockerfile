FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY *.csproj ./
COPY . ./

RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 3000

ENV ASPNETCORE_URLS=http://+:3000
ENV FLASK_API_URL=http://flask-container:5000

ENTRYPOINT ["dotnet", "PokemonApi.dll"]