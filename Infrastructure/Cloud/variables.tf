variable "profile" {
    default = "personal"
	description = "e.g. personal"
}

variable "region" {
    default = "eu-west-2"
}

variable "key" {
    default = "aws"
	description = "Key name"
}

variable "amazon_linux_ami" {
    default = "ami-01a6e31ac994bbc09"
}

variable "app_name" {
    default = "ems"
}