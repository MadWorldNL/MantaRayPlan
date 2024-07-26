[![SonarCloud](https://github.com/MadWorldNL/MantaRayPlan/actions/workflows/sonarqube.yaml/badge.svg)](https://github.com/MadWorldNL/MantaRayPlan/actions/workflows/sonarqube.yaml)
[![CodeQL](https://github.com/MadWorldNL/MantaRayPlan/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/MadWorldNL/MantaRayPlan/actions/workflows/github-code-scanning/codeql)

# MantaRayPlan 
*TODO*

## Key Features
*TODO*

## Getting Started
### Prerequisites
Download the following software
* Docker 
* .NET 8 SDK

### Installation
Installation guide for your development environment
#### Option 1: From Docker
* Clone this repository
* Run this command in the src folder:
```shell
docker compose --profile all up
```

#### Option 2: From your editor
* Clone this repository
* Run this command in the src folder:
```shell
sudo dotnet workload restore
docker compose up
```
* Set this projects as startup:
    * Admin.Bff
    * Admin.Web
    * Viewer.Bff
    * Viewer.Mobile
    * Viewer.Web
    * Api.Grpc *
* Press run

*This project is required to set as startup project

### Running the tests
*TODO*

## Documentation
*TODO*