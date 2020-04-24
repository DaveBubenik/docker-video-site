# docker-video-site

## Introduction
This project contains a Docker-base .NET core website that streams live and recorded data. It uses docker compose to define the system architecture. All systems are arranged behind an NGINX reverse proxy. The system has the following architecture: ![Architecture](/design/out/SystemArchitecture/SystemArchitecture.png)

## Environment Setup
The following software is required to run the system:
- VS2019 with the following workloads: ASP.NET and web development, .NET desktop development, and .NET Core cross-platform development.
- Docker Desktop running Linux containers.

## Running the project
- Open the solution using VS2019
- Click the Debug using Docker Compose

