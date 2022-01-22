terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 3.27"
    }
  }

  required_version = ">= 0.14.9"
  
  backend "s3" {
    bucket = "ems-tf-backend-1" # Caution, name from backend/s3
    key = "terraform"
    region = "eu-west-2"
    dynamodb_table = "ems-tf-lock" # Caution, name from backend/dynamodb
  }
}

provider "aws" {
    region = var.region
    profile = var.profile
}