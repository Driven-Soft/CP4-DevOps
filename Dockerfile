FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build

WORKDIR /src

COPY . .

RUN dotnet publish Argos.Api/Argos.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview

WORKDIR /app

COPY --from=build /app/publish .

USER app

EXPOSE 8080

ENTRYPOINT ["sh", "-c", "sleep 40 && dotnet Argos.Api.dll"]