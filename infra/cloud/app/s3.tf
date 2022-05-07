resource "aws_s3_bucket" "ems_data_bucket" {
    bucket = var.bucket_name
    
    tags = {        
        App = var.app_name
    }
}

resource "aws_s3_bucket_cors_configuration" "ems_data_bucket_cors" {
  bucket = aws_s3_bucket.ems_data_bucket.bucket

  cors_rule {
    allowed_methods = ["GET"]
    allowed_origins = ["*"]
  }
}

resource "aws_s3_bucket_acl" "ems_data_bucket_acl" {
  bucket = aws_s3_bucket.ems_data_bucket.id
  acl    = "private"
}