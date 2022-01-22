####################################################
#      Local docker deployment

.PHONY: start-infra-local
start-infra-local:
	cd Local && docker-compose up -d --build

.PHONY: stop-infra-local
stop-infra-local:
	cd Local && docker-compose down -v


####################################################
#       Cloud

# Deploy api - ec2

.PHONY: init-api
init-api:
	cd cloud && terraform init

.PHONY: destory-api
destroy-api:
	cd cloud && terraform destroy

.PHONY: plan-api
plan-api:
	cd cloud && terraform plan

.PHONY: deploy-api
deploy-api:
	cd cloud && terraform apply

.PHONY: connect
connect:
	ssh ec2-user@${ip} -i ~/Documents/ems.pem
#	e.g make connect ip=0.0.0.0