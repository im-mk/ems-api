resource "aws_instance" "ec2_ems_api" {
    ami = var.amazon_linux_ami
    availability_zone = "eu-west-2a"
    instance_type = "t2.micro"
    key_name = var.key
    vpc_security_group_ids = [aws_security_group.sg_ems_api.id]
    subnet_id = aws_subnet.sub_ecs_api.id
    tags = {
        Name = "ec2-ems-api"
        App = var.app_name
    }
    user_data = <<EOF
#! /bin/sh
yum update -y
amazon-linux-extras install docker
service docker start
usermod -a -G docker ec2-user
chkconfig docker on
EOF
    
}