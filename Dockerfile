FROM mcr.microsoft.com/dotnet/aspnet:5.0.9-buster-slim AS base
#RUN apt-get install -y bash 
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build -- start

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src

### Checkout
# Copy source code
COPY ./src .

# Build Api
RUN dotnet build SvcTemplate.Api.sln /p:TargetFramework=net5.0 /p:Configuration=Debug -m:1

# Build Sdk
RUN dotnet build SvcTemplate.Sdk.sln /p:TargetFramework=net5.0 /p:Configuration=Debug -m:1

# Build Tests
RUN dotnet build SvcTemplate.Tests.sln /p:TargetFramework=net5.0 /p:Configuration=Debug -m:1


### NUnit
# RUN TESTS DOTNET
RUN dotnet test LementPro.Server.SvcTemplate.Test --no-build --logger "trx;LogFileName=TestResults-Rev_0-Build_0.trx" --results-directory /app/TestResults

# Publish
RUN dotnet publish LementPro.Server.SvcTemplate.Api -c Release /p:TrimUnusedDependencies=true -o /app

FROM base AS final
WORKDIR /app
ARG ENVIRONMENT
ENV ASPNETCORE_ENVIRONMENT ${ENVIRONMENT:-DockerLocal}

COPY --from=build /app .
COPY entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

CMD ["/entrypoint.sh", "sh", "-c", "dotnet LementPro.Server.SvcTemplate.Api.dll --environment=${ASPNETCORE_ENVIRONMENT}"]


