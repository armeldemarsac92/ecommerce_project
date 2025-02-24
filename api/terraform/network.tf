resource "aws_vpc" "main" {
  assign_generated_ipv6_cidr_block     = true
  cidr_block                           = "172.30.0.0/16"
  enable_dns_hostnames                 = true
  enable_dns_support                   = true
  enable_network_address_usage_metrics = false
  instance_tenancy                     = "default"
  
  tags                                 = {
    "Name"   = "${var.project_name}_vpc"
    "Project" = var.project_name
  }
  tags_all                             = {
    "Project" = var.project_name
  }
}

resource "aws_internet_gateway" "main" {
  vpc_id = aws_vpc.main.id

  tags = {
    "Name"   = "${var.project_name}_igw"
    "Project" = var.project_name
  }
}

resource "aws_route_table" "public" {
  vpc_id = aws_vpc.main.id

  route {
    cidr_block = "0.0.0.0/0"
    gateway_id = aws_internet_gateway.main.id
  }

  route {
    ipv6_cidr_block = "::/0"
    gateway_id      = aws_internet_gateway.main.id
  }

  tags = {
    "Name"   = "${var.project_name}_rtb_public"
    "Project" = var.project_name
  }
}

resource "aws_subnet" "subnet1a" {
  availability_zone                              = "eu-central-1a"
  cidr_block                                     = "172.30.0.0/24"
  enable_dns64                                   = false
  enable_resource_name_dns_a_record_on_launch    = false
  enable_resource_name_dns_aaaa_record_on_launch = false
  map_public_ip_on_launch                        = true

  ipv6_cidr_block        = cidrsubnet(aws_vpc.main.ipv6_cidr_block, 8, 0)
  assign_ipv6_address_on_creation = true
  
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
  availability_zone                              = "eu-central-1b"
  cidr_block                                     = "172.30.1.0/24"
  enable_dns64                                   = false
  enable_resource_name_dns_a_record_on_launch    = false
  enable_resource_name_dns_aaaa_record_on_launch = false
  map_public_ip_on_launch                        = true
  
  ipv6_cidr_block        = cidrsubnet(aws_vpc.main.ipv6_cidr_block, 8, 1)
  assign_ipv6_address_on_creation = true
  
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
  availability_zone                              = "eu_central_1c"
  cidr_block                                     = "172.30.2.0/24"
  enable_dns64                                   = false
  enable_resource_name_dns_a_record_on_launch    = false
  enable_resource_name_dns_aaaa_record_on_launch = false
  map_public_ip_on_launch                        = true

  ipv6_cidr_block        = cidrsubnet(aws_vpc.main.ipv6_cidr_block, 8, 2)
  assign_ipv6_address_on_creation = true
  
  private_dns_hostname_type_on_launch            = "ip-name"
  tags                                 = {
    "Name"   = "${var.project_name}-subnet-private1-eu-central-1a"
    "Project" = var.project_name
  }
  tags_all                             = {
    "Project" = var.project_name
  }
  vpc_id                                         = aws_vpc.main.id
}

resource "aws_route_table_association" "public_1" {
  subnet_id      = aws_subnet.subnet1a.id
  route_table_id = aws_route_table.public.id

}

resource "aws_route_table_association" "public_2" {
  subnet_id      = aws_subnet.subnet1b.id
  route_table_id = aws_route_table.public.id

}

resource "aws_route_table_association" "public_3" {
  subnet_id      = aws_subnet.subnet1c.id
  route_table_id = aws_route_table.public.id

}

resource "aws_subnet" "private_subnet_1" {
  vpc_id                  = aws_vpc.main.id
  cidr_block              = "172.30.48.0/20"  
  availability_zone       = "eu-central-1a"
  map_public_ip_on_launch = false

  tags = {
    "Name"   = "${var.project_name}_subnet_private1_eu_central_1a"
    "Project" = var.project_name
  }
}

resource "aws_subnet" "private_subnet_2" {
  vpc_id                  = aws_vpc.main.id
  cidr_block              = "172.30.64.0/20"  
  availability_zone       = "eu-central-1b"
  map_public_ip_on_launch = false

  tags = {
    "Name"   = "${var.project_name}_subnet_private2_eu_central_1b"
    "Project" = var.project_name
  }
}

resource "aws_subnet" "private_subnet_3" {
  vpc_id                  = aws_vpc.main.id
  cidr_block              = "172.30.80.0/20" 
  availability_zone       = "eu-central-1c"
  map_public_ip_on_launch = false

  tags = {
    "Name"   = "${var.project_name}_subnet_private3_eu_central_1c"
    "Project" = var.project_name
  }
}

resource "aws_route_table" "private" {
  vpc_id = aws_vpc.main.id

  tags = {
    "Name"   = "${var.project_name}_rtb_private"
    "Project" = var.project_name
  }
}

resource "aws_route_table_association" "private_1" {
  subnet_id      = aws_subnet.private_subnet_1.id
  route_table_id = aws_route_table.private.id
}

resource "aws_route_table_association" "private_2" {
  subnet_id      = aws_subnet.private_subnet_2.id
  route_table_id = aws_route_table.private.id
}

resource "aws_route_table_association" "private_3" {
  subnet_id      = aws_subnet.private_subnet_3.id
  route_table_id = aws_route_table.private.id
}
