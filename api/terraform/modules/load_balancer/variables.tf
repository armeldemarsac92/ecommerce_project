#load balancer vars

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

variable "domains" {
  description = "List of domain names to use"
  type        = list(string)
  default     = [
    "example-project.fr",
    "api.example-project.fr",
    "auth.example-project.fr"
  ]  
}

variable "external_dns_zone_id"{
  description = "The id of the external DNS zone"
  type        = string
}

variable "project_name"{
  description = "The name of the project, used to identify ressources globally."
  type        = string
  default     = "example-project.fr"
}