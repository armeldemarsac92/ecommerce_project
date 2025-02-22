resource "aws_vpc" "main" {
  assign_generated_ipv6_cidr_block     = true
  cidr_block                           = "172.30.0.0/16"
  enable_dns_hostnames                 = true
  enable_dns_support                   = true
  enable_network_address_usage_metrics = false
  instance_tenancy                     = "default"
  tags                                 = {
    "Project" = var.project_name
  }
  tags_all                             = {
    "Project" = var.project_name
  }
}

resource "aws_subnet" "subnet1a" {
  assign_ipv6_address_on_creation                = false
  availability_zone                              = "eu-central-1a"
  cidr_block                                     = "172.30.0.0/24"
  enable_dns64                                   = false
  enable_resource_name_dns_a_record_on_launch    = false
  enable_resource_name_dns_aaaa_record_on_launch = false
  ipv6_cidr_block                                = "2a05:d014:1f66:b701::/64"
  ipv6_native                                    = false
  map_public_ip_on_launch                        = true
  private_dns_hostname_type_on_launch            = "ip-name"
  tags                                 = {
    "Project" = var.project_name
  }
  tags_all                             = {
    "Project" = var.project_name
  }
  vpc_id                                         = aws_vpc.main.id
}

resource "aws_subnet" "subnet1b" {
  assign_ipv6_address_on_creation                = false
  availability_zone                              = "eu-central-1b"
  cidr_block                                     = "172.30.1.0/24"
  enable_dns64                                   = false
  enable_resource_name_dns_a_record_on_launch    = false
  enable_resource_name_dns_aaaa_record_on_launch = false
  ipv6_cidr_block                                = "2a05:d014:1f66:b700::/64"
  ipv6_native                                    = false
  map_public_ip_on_launch                        = true
  private_dns_hostname_type_on_launch            = "ip-name"
  tags                                 = {
    "Project" = var.project_name
  }
  tags_all                             = {
    "Project" = var.project_name
  }
  vpc_id                                         = aws_vpc.main.id
}


resource "aws_subnet" "subnet1c" {
  assign_ipv6_address_on_creation                = false
  availability_zone                              = "eu-central-1c"
  cidr_block                                     = "172.30.2.0/24"
  enable_dns64                                   = false
  enable_resource_name_dns_a_record_on_launch    = false
  enable_resource_name_dns_aaaa_record_on_launch = false
  ipv6_cidr_block                                = "2a05:d014:1f66:b702::/64"
  ipv6_native                                    = false
  map_public_ip_on_launch                        = true
  private_dns_hostname_type_on_launch            = "ip-name"
  tags                                 = {
    "Project" = var.project_name
  }
  tags_all                             = {
    "Project" = var.project_name
  }
  vpc_id                                         = aws_vpc.main.id
}

