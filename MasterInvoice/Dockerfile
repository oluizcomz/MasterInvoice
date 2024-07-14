# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.


# Stage 3: Build entities project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS entities-build
WORKDIR /src/entities
COPY ["../Entities/Entities.csproj", "./"]
RUN dotnet restore "Entities.csproj"
COPY Entities/ .
RUN dotnet build "Entities.csproj" -c Release -o /app/build/entities



# Stage 1: Build domain project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS domain-build
WORKDIR /src/domain
COPY ["../Domain/Domain.csproj", "./"]
RUN dotnet restore "Domain.csproj"
COPY Domain/ .
RUN dotnet build "Domain.csproj" -c Release -o /app/build/domain

# Stage 2: Build infrastructure project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS infrastructure-build
WORKDIR /src/infrastructure
COPY ["../Infrastructure/Infrastructure.csproj", "./"]
RUN dotnet restore "Infrastructure.csproj"
COPY Infrastructure/ .
RUN dotnet build "Infrastructure.csproj" -c Release -o /app/build/infrastructure

# Stage 4: Build application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src/app
COPY ["MasterInvoice.csproj", "./"]
RUN dotnet restore "MasterInvoice.csproj"
COPY . .
RUN dotnet build "MasterInvoice.csproj" -c Release -o /app/build/app

# Stage 5: Publish application
FROM build AS publish
RUN dotnet publish "MasterInvoice.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 6: Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=entities-build /app/build/entities ./entities
COPY --from=domain-build /app/build/domain ./domain
COPY --from=infrastructure-build /app/build/infrastructure ./infrastructure
ENTRYPOINT ["dotnet", "MasterInvoice.dll"]
