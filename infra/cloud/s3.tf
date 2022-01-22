resource "aws_s3_bucket" "ems_data_bucket" {
    bucket = var.bucket_name
    acl = "private"

    cors_rule {        
        allowed_methods = ["GET"]
        allowed_origins = ["*"]
    }

    tags = {        
        App = var.app_name
    }
}