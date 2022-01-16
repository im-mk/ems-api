####################################################
#      Local docker deployment

.PHONY: start-infra-local
start-infra-local:
	cd Local && docker-compose up -d --build

.PHONY: stop-infra-local
stop-infra-local:
	cd Local && docker-compose down -v

# Create s3 bucket and assign a policy
.PHONY: create-s3
create-s3:
	aws --endpoint-url=http://localhost:4572 s3 mb s3://ems-data-bucket
	aws --endpoint-url=http://localhost:4572 s3api put-bucket-acl --bucket ems-data-bucket --acl public-read 

# List bucket contents
.PHONY: list-s3
list-s3:
	aws --endpoint-url=http://localhost:4572 s3 ls s3://ems-data-bucket/


####################################################
#       Cloud

#Deploy api - s3 data bucket
.PHONY: init-api
init-api:
	cd Cloud/s3 && terraform init

.PHONY: destory-api
destroy-api:
	cd Cloud/s3 && terraform destroy

.PHONY: plan-api
plan-api:
	cd Cloud/s3 && terraform plan

.PHONY: deploy-api
deploy-api:
	cd Cloud/s3 && terraform apply


# Deploy api - ec2

.PHONY: init-api
init-api:
	cd Cloud && terraform init

.PHONY: destory-api
destroy-api:
	cd Cloud && terraform destroy

.PHONY: plan-api
plan-api:
	cd Cloud && terraform plan

.PHONY: deploy-api
deploy-api:
	cd Cloud && terraform apply

.PHONY: connect
connect:
	ssh ec2-user@${ip} -i ./aws.pem