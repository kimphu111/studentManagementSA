FROM ://microsoft.com AS build
WORKDIR /src

COPY ["AuthGateway.csproj", "./"]
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app

FROM ://microsoft.com
WORKDIR /app

COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:7000

ENTRYPOINT ["dotnet", "AuthGateway.dll"]
