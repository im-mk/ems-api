# EMS Api

This is a dotnet core api...

## Getting Started

### Software/Tools required

Dotnet 5 <https://dotnet.microsoft.com/download>
Docker <https://docs.docker.com/docker-for-windows/install/>
Vscode

Install Dotnet Core entity framekwork tool using the following command to create entity framework migrations:

dotnet tool install --global dotnet-ef

### Run the api

Check the Makefile for a list of all the commands available to run this api.

`make setup` only need to run this once.

When you hit the https endpoint using a browser or through javascript call you will get invalid certificate error due to the certificate issued by the localhost. above command will fix this issue.
We need to store the secret key in dotnet user-secrets store when running the api locally. check dotnet user-secrets utility for more details.

`make start-infra-local` will start postgres db and other dependencies in docker.

`make start` will run the api

### Stop the api

To stop it press control + c to terminate the api.

To remove the infra run `make stop-infra-local`

### Run the api in a docker container

Start/Stop the api using the following commands.

`make start-docker`

`make stop-docker`
