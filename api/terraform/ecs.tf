resource "aws_ecs_cluster" "main" {
  name     = "tdev700Cluster"
  tags                                 = {
    "Project" = var.project_name
  }
  tags_all                             = {
    "Project" = var.project_name
  }

  service_connect_defaults {
    namespace = aws_service_discovery_http_namespace.main.arn
  }

  setting {
    name  = "containerInsights"
    value = "disabled"
  }
}

resource "aws_ecs_service" "auth" {
  availability_zone_rebalancing      = "ENABLED"
  cluster                            = aws_ecs_cluster.main.id
  deployment_maximum_percent         = 200
  deployment_minimum_healthy_percent = 100
  desired_count                      = 0
  enable_ecs_managed_tags            = true
  enable_execute_command             = true
  health_check_grace_period_seconds  = 60
  launch_type                        = "FARGATE"
  platform_version                   = "LATEST"
  name                               = "auth"
  propagate_tags                     = "NONE"
  scheduling_strategy                = "REPLICA"
  tags                                 = {
    "Project" = var.project_name
  }
  tags_all                             = {
    "Project" = var.project_name
  }
  task_definition                    = aws_ecs_task_definition.auth.arn
  triggers                           = {}

  deployment_circuit_breaker {
    enable   = false
    rollback = false
  }

  deployment_controller {
    type = "ECS"
  }

  load_balancer {
    container_name   = "tdev_auth"
    container_port   = 8080
    elb_name         = null
    target_group_arn = aws_lb_target_group.auth.arn
  }

  network_configuration {
    assign_public_ip = true
    security_groups  = [
      aws_security_group.auth.id
    ]
    subnets          = [
      aws_subnet.subnet1a.id,
      aws_subnet.subnet1b.id,
      aws_subnet.subnet1c.id
    ]
  }
}

resource "aws_ecs_service" "api" {
  availability_zone_rebalancing      = "ENABLED"
  cluster                            = aws_ecs_cluster.main.id
  deployment_maximum_percent         = 200
  deployment_minimum_healthy_percent = 100
  desired_count                      = 0
  enable_ecs_managed_tags            = true
  enable_execute_command             = true
  health_check_grace_period_seconds  = 60
  launch_type                        = "FARGATE"
  name                               = "api"
  platform_version                   = "LATEST"
  propagate_tags                     = "NONE"
  scheduling_strategy                = "REPLICA"
  tags                                 = {
    "Project" = var.project_name
  }
  tags_all                             = {
    "Project" = var.project_name
  }
  task_definition                    = aws_ecs_task_definition.api.arn
  triggers                           = {}

  deployment_circuit_breaker {
    enable   = false
    rollback = false
  }

  load_balancer {
    container_name   = "tdev_api"
    container_port   = 8080
    elb_name         = null
    target_group_arn = aws_lb_target_group.api.arn
  }

  network_configuration {
    assign_public_ip = true
    security_groups  = [
      aws_security_group.auth.id
    ]
    subnets          = [
      aws_subnet.subnet1a.id,
      aws_subnet.subnet1b.id,
      aws_subnet.subnet1c.id
    ]
  }
}

resource "aws_ecs_service" "frontend" {
  availability_zone_rebalancing      = "ENABLED"
  cluster                            = aws_ecs_cluster.main.id
  deployment_maximum_percent         = 200
  deployment_minimum_healthy_percent = 100
  desired_count                      = 0
  enable_ecs_managed_tags            = true
  enable_execute_command             = true
  health_check_grace_period_seconds  = 60
  launch_type                        = "FARGATE"
  name                               = "frontend"
  platform_version                   = "LATEST"
  propagate_tags                     = "NONE"
  scheduling_strategy                = "REPLICA"
  tags                                 = {
    "Project" = var.project_name
  }
  tags_all                             = {
    "Project" = var.project_name
  }
  task_definition                    = aws_ecs_task_definition.frontend.arn
  triggers                           = {}

  deployment_circuit_breaker {
    enable   = false
    rollback = false
  }

  load_balancer {
    container_name   = "tdev_api"
    container_port   = 3000
    elb_name         = null
    target_group_arn = aws_lb_target_group.frontend.arn
  }

  network_configuration {
    assign_public_ip = true
    security_groups  = [
      aws_security_group.auth.id
    ]
    subnets          = [
      aws_subnet.subnet1a.id,
      aws_subnet.subnet1b.id,
      aws_subnet.subnet1c.id
    ]
  }
}


resource "aws_ecs_task_definition" "auth" {
  container_definitions    = jsonencode(
    [
      {
        environment      = [
          {
            name  = "ASPNETCORE_ENVIRONMENT"
            value = "Production"
          },
        ]
        essential        = true
        image            = "502863813996.dkr.ecr.eu-central-1.amazonaws.com/t-dev-702/prod/auth:latest"
        logConfiguration = {
          logDriver     = "awslogs"
          options       = {
            awslogs-create-group  = "true"
            awslogs-group         = "/ecs/tdev_auth"
            awslogs-region        = "eu-central-1"
            awslogs-stream-prefix = "ecs"
            max-buffer-size       = "25m"
            mode                  = "non-blocking"
          }
          secretOptions = []
        }
        mountPoints      = []
        name             = "tdev_auth"
        portMappings     = [
          {
            appProtocol   = "http"
            containerPort = 8080
            hostPort      = 8080
            name          = "auth"
            protocol      = "tcp"
          },
        ]
        systemControls   = []
        volumesFrom      = []
      },
    ]
  )
  cpu                      = "256"
  enable_fault_injection   = false
  execution_role_arn       = "arn:aws:iam::502863813996:role/ecsTaskExecutionRole"
  family                   = "tdev_auth"
  ipc_mode                 = null
  memory                   = "512"
  network_mode             = "awsvpc"
  pid_mode                 = null
  requires_compatibilities = [
    "FARGATE",
  ]
  tags                                 = {
    "Project" = var.project_name
  }
  tags_all                             = {
    "Project" = var.project_name
  }
  task_role_arn            = "arn:aws:iam::502863813996:role/ECSSeeqrRole"
  track_latest             = false

  runtime_platform {
    cpu_architecture        = "X86_64"
    operating_system_family = "LINUX"
  }
}

resource "aws_ecs_task_definition" "api" {
  container_definitions    = jsonencode(
    [
      {
        environment      = [
          {
            name  = "ASPNETCORE_ENVIRONMENT"
            value = "Production"
          },
        ]
        essential        = true
        image            = "502863813996.dkr.ecr.eu-central-1.amazonaws.com/t-dev-702/prod/api:latest"
        logConfiguration = {
          logDriver     = "awslogs"
          options       = {
            awslogs-create-group  = "true"
            awslogs-group         = "/ecs/tdev_api"
            awslogs-region        = "eu-central-1"
            awslogs-stream-prefix = "ecs"
            max-buffer-size       = "25m"
            mode                  = "non-blocking"
          }
          secretOptions = []
        }
        mountPoints      = []
        name             = "tdev_api"
        portMappings     = [
          {
            appProtocol   = "http"
            containerPort = 8080
            hostPort      = 8080
            name          = "api"
            protocol      = "tcp"
          },
        ]
        systemControls   = []
        volumesFrom      = []
      },
    ]
  )
  cpu                      = "256"
  enable_fault_injection   = false
  execution_role_arn       = "arn:aws:iam::502863813996:role/ecsTaskExecutionRole"
  family                   = "tdev_api"
  ipc_mode                 = null
  memory                   = "512"
  network_mode             = "awsvpc"
  pid_mode                 = null
  requires_compatibilities = [
    "FARGATE",
  ]
  tags                                 = {
    "Project" = var.project_name
  }
  tags_all                             = {
    "Project" = var.project_name
  }
  task_role_arn            = "arn:aws:iam::502863813996:role/ECSSeeqrRole"
  track_latest             = false

  runtime_platform {
    cpu_architecture        = "X86_64"
    operating_system_family = "LINUX"
  }
}

resource "aws_ecs_task_definition" "frontend" {
  container_definitions    = jsonencode(
    [
      {
        environment      = [
          {
            name  = "ASPNETCORE_ENVIRONMENT"
            value = "Production"
          },
        ]
        essential        = true
        image            = "502863813996.dkr.ecr.eu-central-1.amazonaws.com/t-dev-702/prod/frontend:latest"
        logConfiguration = {
          logDriver     = "awslogs"
          options       = {
            awslogs-create-group  = "true"
            awslogs-group         = "/ecs/tdev_frontend"
            awslogs-region        = "eu-central-1"
            awslogs-stream-prefix = "ecs"
            max-buffer-size       = "25m"
            mode                  = "non-blocking"
          }
          secretOptions = []
        }
        mountPoints      = []
        name             = "tdev_api"
        portMappings     = [
          {
            appProtocol   = "http"
            containerPort = 3000
            hostPort      = 3000
            name          = "frontend"
            protocol      = "tcp"
          },
        ]
        systemControls   = []
        volumesFrom      = []
      },
    ]
  )
  cpu                      = "256"
  enable_fault_injection   = false
  execution_role_arn       = "arn:aws:iam::502863813996:role/ecsTaskExecutionRole"
  family                   = "tdev_frontend"
  ipc_mode                 = null
  memory                   = "512"
  network_mode             = "awsvpc"
  pid_mode                 = null
  requires_compatibilities = [
    "FARGATE",
  ]
  tags                                 = {
    "Project" = var.project_name
  }
  tags_all                             = {
    "Project" = var.project_name
  }
  task_role_arn            = "arn:aws:iam::502863813996:role/ECSSeeqrRole"
  track_latest             = false

  runtime_platform {
    cpu_architecture        = "X86_64"
    operating_system_family = "LINUX"
  }
}
