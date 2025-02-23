resource "aws_instance" "bastion" {
  ami                         = "ami-0eddb4a4e7d846d6f"
  instance_type               = "t2.nano"
  key_name                    = "tdev700"
  subnet_id                   = aws_subnet.subnet1b.id
  vpc_security_group_ids      = [aws_security_group.vpc.id]
  associate_public_ip_address = true
  iam_instance_profile        = "ec2Tdev702Role"

  ipv6_address_count         = 1

  tags = {
    Name = "bastion tdev700"
    Project = var.project_name
  }

  root_block_device {
    volume_size = 8
    volume_type = "gp3"
    throughput  = 125
    iops        = 3000
  }

  metadata_options {
    http_endpoint               = "enabled"
    http_tokens                 = "required"
    http_put_response_hop_limit = 2
    instance_metadata_tags      = "enabled"
  }

  credit_specification {
    cpu_credits = "standard"
  }
}