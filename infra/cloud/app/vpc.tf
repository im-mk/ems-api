resource "aws_vpc" "vpc_ems" {
    cidr_block = "10.1.0.0/16"
    tags = {
        Name = "vpc-ems"
        App = var.app_name
    }
}

resource "aws_internet_gateway" "ig_ecs_api" {
    vpc_id = aws_vpc.vpc_ems.id
    tags = {
        Name = "ig-ecs-api"
        App = var.app_name
    }
}

resource "aws_subnet" "sub_ecs_api" {
    vpc_id = aws_vpc.vpc_ems.id
    cidr_block = "10.1.1.0/24"
    availability_zone = var.region
    tags = {
        Name = "sub-ecs-api-a"
        App = var.app_name
    }
}

resource "aws_route_table" "rt_ems_api" {
    vpc_id = aws_vpc.vpc_ems.id
    route {
        cidr_block = "0.0.0.0/0"
        gateway_id = aws_internet_gateway.ig_ecs_api.id
    }
    tags = {
        Name = "rt_ems_api"
        App = var.app_name
    }
}

resource "aws_route_table_association" "route_table_association_ems" {
    subnet_id = aws_subnet.sub_ecs_api.id
    route_table_id = aws_route_table.rt_ems_api.id
}

resource "aws_network_acl" "allowall" {
    vpc_id = aws_vpc.vpc_ems.id

    egress {
        protocol = "-1"
        rule_no = "100"
        action = "allow"
        cidr_block = "0.0.0.0/0"
        from_port = "0"
        to_port = "0"
    }

    ingress {
        protocol = "-1"
        rule_no = "200"
        action = "allow"
        cidr_block = "0.0.0.0/0"
        from_port = "0"
        to_port = "0"
    }

    tags = {
        Name = "allowall"
        App = var.app_name
    }
}

resource "aws_security_group" "sg_ems_api" {
    name        = "sg_ems_api"
    description = "Allow Http and ssh inbound traffic"
    vpc_id = aws_vpc.vpc_ems.id
    
    ingress {
        description = "SSH"
        from_port   = 22
        to_port     = 22
        protocol    = "tcp"
        cidr_blocks = ["0.0.0.0/0"]
    }

    ingress {
        description = "TLS"
        from_port   = 443
        to_port     = 443
        protocol    = "tcp"
        cidr_blocks = ["0.0.0.0/0"]
    }

    egress {
        from_port   = 0
        to_port     = 0
        protocol    = "-1"
        cidr_blocks = ["0.0.0.0/0"]
    }

    tags = {
        Name = "sg_ems_api"
        App = var.app_name
    }
}

resource "aws_eip" "eip_ems" {
    instance = aws_instance.ec2_ems_api.id
    vpc = true
    depends_on = [aws_internet_gateway.ig_ecs_api]
}
