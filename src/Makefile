################################################
#       Setup/Configuration

# Initialize secret-store for local dev
.PHONY: setup
setup:
	dotnet dev-certs https --trust
	dotnet user-secrets init -p EMS.Api/
	dotnet user-secrets set "TOKENKEY" "super sectet key" -p EMS.Api/

# Create entity framework migration
.PHONY: migrate
migrate:	
	dotnet ef migrations add ${name} -p EMS.Db/ -s EMS.Api

# Create a release build
.PHONY: publish
publish: 
	dotnet publish --configuration Release


###################################################
#          Run

# Run api on local machine
.PHONY: start
start: 
	cd EMS.Api && dotnet run

.PHONY: watch
watch: 
	cd EMS.Api && dotnet watch run

#####################################################
#  Build and run api in a container

# .PHONY: start-docker
# start-docker:
# 	docker-compose up -d --build --force-recreate
# 	make create-s3

# .PHONY: stop-docker
# stop-docker:
# 	docker-compose down

#####################################################

# Start aws service locally using localstack
.PHONY: start-infra-local
start-infra-local:
	cd ./../infra && make start-infra-local

.PHONY: stop-infra-local
stop-infra-local:
	cd ./../infra && make stop-infra-local
