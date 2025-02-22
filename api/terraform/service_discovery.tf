resource "aws_service_discovery_http_namespace" "main" {
  description = null
  name        = "tdev700Cluster"
  tags        = {
    "Project" = var.project_name
    "AmazonECSManaged" = "true"
  }
  tags_all    = {
    "Project" = var.project_name
    "AmazonECSManaged" = "true"
  }
}
