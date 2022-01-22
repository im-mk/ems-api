output "ec2_ecs_arn" {
    description = "EC2 arn. It will be the format arn:aws:ec2:::ec2name"
    value = aws_instance.ec2_ems_api.arn
}

output "ec2_ecs_ip" {
    value = aws_eip.eip_ems.public_ip
}