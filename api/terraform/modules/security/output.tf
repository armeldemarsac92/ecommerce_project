output "vpc_security_group_id" {
  description = "ID of the VPC security group"
  value       = aws_security_group.vpc.id
}

output "load_balancer_security_group_id" {
  description = "ID of the load balancer security group"
  value       = aws_security_group.load_balancer.id
}

output "api_security_group_id" {
  description = "ID of the API security group"
  value       = aws_security_group.api.id
}

output "auth_security_group_id" {
  description = "ID of the authentication server security group"
  value       = aws_security_group.auth.id
}

output "frontend_security_group_id" {
  description = "ID of the frontend security group"
  value       = aws_security_group.frontend.id
}

output "rds_security_group_id" {
  description = "ID of the RDS database security group"
  value       = aws_security_group.rds.id
}

output "security_group_ids" {
  description = "Map of all security group IDs"
  value = {
    vpc           = aws_security_group.vpc.id
    load_balancer = aws_security_group.load_balancer.id
    api           = aws_security_group.api.id
    auth          = aws_security_group.auth.id
    frontend      = aws_security_group.frontend.id
    rds           = aws_security_group.rds.id
  }
}