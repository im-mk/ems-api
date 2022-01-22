resource "aws_dynamodb_table" "terraform_state_lock" {
    name           = var.terraform_lock_table_name
    read_capacity  = 5
    write_capacity = 5
    hash_key       = "LockID"
  
    attribute {
        name = "LockID"
        type = "S"
    }

    tags = {        
        App = var.app_name
    }
}