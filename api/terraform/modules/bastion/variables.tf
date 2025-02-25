#bastion vars

variable "vpc_id" {
  description = "The ID of the VPC"
  type        = string
}

variable "public_subnet_ids" {
  description = "List of public subnet IDs"
  type        = list(string)
}

variable "private_subnet_ids" {
  description = "List of private subnet IDs"
  type        = list(string)
}

variable "security_groups" {
  description = "Map of security group IDs"
  type        = map(string)
}

variable "route53_zone_id" {
  description = "The ID of the Route 53 hosted zone"
  type        = string
}

variable "bastion_host_name" {
  description = "The host name of the bastion ec2 instance"
  default = "bastion.defaultproject.fr"
  type        = string
}

variable "project_name"{
  description = "The name of the project, used to identify ressources globally."
  type        = string
  default     = "example-project"
}

