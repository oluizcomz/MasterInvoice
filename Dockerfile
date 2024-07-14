# Stage 1: Build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["Entities/Entities.csproj", "Entities/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["MasterInvoice/MasterInvoiceWeb.csproj", "MasterInvoice/"]

# Restore dependencies
RUN dotnet restore "MasterInvoice/MasterInvoiceWeb.csproj"

# Copy all files and build the application
COPY . .
WORKDIR /src/MasterInvoice
RUN dotnet build "MasterInvoiceWeb.csproj" -c Release -o /app/build

# Stage 2: Build a runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "MasterInvoiceWeb.dll"]
