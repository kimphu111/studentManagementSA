FROM ://microsoft.com AS build
WORKDIR /src

COPY ["Gateway.csproj", "./"]
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app

FROM ://microsoft.com
WORKDIR /app

COPY --from=build /app .
ENV ASPNETCORE_URLS=http://+:7000;http://+:7001

ENTRYPOINT ["dotnet", "Gateway.dll"]
