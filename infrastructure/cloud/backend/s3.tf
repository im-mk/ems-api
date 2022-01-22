resource "aws_s3_bucket" "bucket" {
    bucket = var.terraform_bucket_name
    tags = {        
        App = var.app_name
    }
}