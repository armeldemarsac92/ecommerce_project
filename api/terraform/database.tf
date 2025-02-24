resource "aws_db_subnet_group" "database" {
  name        = "tdev700-database-subnet-group"
  description = "Database subnet group for tdev700 RDS instances"

  subnet_ids = [
    aws_subnet.private_subnet_1.id,
    aws_subnet.private_subnet_2.id,
    aws_subnet.private_subnet_3.id
  ]

  tags = {
    Name   = "tdev700-database-subnet-group"
    projet = var.project_name
  }
}

data "aws_db_snapshot" "final_snapshot" {
  most_recent         = true
  snapshot_type       = "manual"
  include_shared      = false
  include_public      = false

  db_snapshot_identifier = "${var.database_name}-final-snapshot"
}

resource "aws_db_instance" "postgresql" {
  identifier                          = var.database_name
  snapshot_identifier    = data.aws_db_snapshot.final_snapshot.id
  allocated_storage                   = 20
  engine                              = "postgres"
  engine_version                      = "16.3"
  instance_class                      = "db.t4g.micro"

  db_subnet_group_name                = aws_db_subnet_group.database.name
  vpc_security_group_ids              = [aws_security_group.vpc.id]

  storage_type                        = "gp3"
  storage_encrypted                   = true
  max_allocated_storage               = 1000
  iops                                = 3000
  storage_throughput                  = 125

  performance_insights_enabled        = true
  performance_insights_retention_period = 7

  backup_retention_period             = 1
  backup_window                       = "21:38-22:08"
  maintenance_window                  = "mon:02:18-mon:02:48"

  skip_final_snapshot   = false
  final_snapshot_identifier = "${var.database_name}-final-snapshot-${formatdate("YYYYMMDDhhmmss", timestamp())}"
  publicly_accessible                 = false

}