# EMS Api

This is a dotnet core api...

## Getting Started

### Software/Tools required

Dotnet <https://dotnet.microsoft.com/download>

Docker <https://docs.docker.com/docker-for-windows/install/>

Docker-Compose

Vscode

Postman

Install Dotnet Core entity framekwork tool using the following command to create entity framework migrations:

dotnet tool install --global dotnet-ef

### Deploy infra to cloud

- Create an aws account.
- Update aws config with newly created account details.  
- Create a keypair under EC2 and save it locally.
- Update pem file permission to secure it. e.g chmod 400 ~/Documents/abc.pem
- Update infrastructure/cloud/main.tf with key name.  
- Update Makefile with location. e.g ~/Downloads/abc.pem
- Deploy terraform backend using make init-back and make init-deploy. See infrastructure/Makefile
- Deploy infra using make init-api && make deploy-api. See infrastructure/Makefile  
- To connect to the newly created instance check infrastructure/Makefile connect.  

### Destroy infra

- To delete all the infra destroy api resources and then backend respectively using make destroy-api && make destroy-back. See infrastructure/cloud

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

### Calling the api using postman

First call login endpoint with username and password from seed class which will return bearer token.

Example:

{
    "email": "test@test.com",
    "password": "Pa$$w0rd"
}

Copy the bearer token value to Authorization header

Authorization : Bearer {{token}}
