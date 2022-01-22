variable "profile" {
    default = "default"
	description = "e.g. personal"
}

variable "region" {
    default = "eu-west-2"
}

variable "key" {
    default = "ems"
	description = "Key name"
}

variable "amazon_linux_ami" {
    default = "ami-0fdbd8587b1cf431e"
}

variable "bucket_name" {
    default =   "ems-data-bucket"
	description = "e.g. your-bucket-name"
}

variable "app_name" {
    default = "ems"
}