resource "aws_lb" "main" {
  name        = "lb_${var.project_name}"
  description = "Application load balancer of ${var.project_name}."
  client_keep_alive                                            = 3600
  desync_mitigation_mode                                       = "defensive"
  drop_invalid_header_fields                                   = false
  enable_cross_zone_load_balancing                             = true
  enable_deletion_protection                                   = false
  enable_http2                                                 = true
  enable_tls_version_and_cipher_suite_headers                  = false
  enable_waf_fail_open                                         = false
  enable_xff_client_port                                       = false
  enable_zonal_shift                                           = false
  idle_timeout                                                 = 60
  internal                                                     = false
  ip_address_type                                              = "dualstack"
  load_balancer_type                                           = "application"
  preserve_host_header                                         = false
  security_groups                                              = [
    aws_security_group.load_balancer.id
  ]
  subnets                                                      = [
    aws_subnet.subnet1a.id,
    aws_subnet.subnet1b.id,
    aws_subnet.subnet1c.id
  ]
  tags                                 = {
    "Project" = var.project_name
  }
  tags_all                             = {
    "Project" = var.project_name
  }
}

resource "aws_lb_listener" "main" {
  name        = "lb_listener_main_${var.project_name}"
  description = "Listener of ${aws_lb_listener.main.name} from ${var.project_name}."
  certificate_arn                                                       = "arn:aws:acm:eu-central-1:502863813996:certificate/1cd8114a-0789-4ac7-b3f9-dc01576cf8ef"
  load_balancer_arn                                                     = aws_lb.main.arn
  port                                                                  = 443
  protocol                                                              = "HTTPS"
  routing_http_response_server_enabled                                  = true
  ssl_policy                                                            = "ELBSecurityPolicy-TLS13-1-2-2021-06"
  tags                                 = {
    "Project" = var.project_name
  }
  tags_all                             = {
    "Project" = var.project_name
  }

  default_action {
    order            = 1
    target_group_arn = aws_lb_target_group.frontend.arn
    type             = "forward"

    forward {
      stickiness {
        duration = 3600
        enabled  = false
      }
      target_group {
        arn    = aws_lb_target_group.frontend.arn
        weight = 1
      }
    }
  }
}

resource "aws_lb_listener_rule" "auth" {
  name        = "lb_listener_rule_auth_${var.project_name}"
  description = "Listener rule for the auth server of ${var.project_name}."
  listener_arn = aws_lb_listener.main.arn
  priority     = 2
  tags                                 = {
    "Project" = var.project_name
  }
  tags_all                             = {
    "Project" = var.project_name
  }

  action {
    order            = 1
    target_group_arn = aws_lb_target_group.auth.arn
    type             = "forward"

    forward {
      stickiness {
        duration = 3600
        enabled  = false
      }
      target_group {
        arn    = aws_lb_target_group.auth.arn
        weight = 1
      }
    }
  }

  condition {
    host_header {
      values = [
        "auth.*",
      ]
    }
  }
}

resource "aws_lb_listener_rule" "api" {
  name        = "lb_listener_rule_api_${var.project_name}"
  description = "Listener rule for the api server of ${var.project_name}."
  listener_arn = aws_lb_listener.main.arn
  priority     = 3
  tags                                 = {
    "Project" = var.project_name
  }
  tags_all                             = {
    "Project" = var.project_name
  }

  action {
    order            = 1
    target_group_arn = aws_lb_target_group.api.arn
    type             = "forward"

    forward {
      stickiness {
        duration = 3600
        enabled  = false
      }
      target_group {
        arn    = aws_lb_target_group.api.arn
        weight = 1
      }
    }
  }

  condition {
    host_header {
      values = [
        "api.*",
      ]
    }
  }
}



resource "aws_lb_target_group" "auth" {
  name        = "lb_tg_auth_${var.project_name}"
  description = "Target group for the auth server of ${var.project_name}."
  vpc_id = aws_vpc.main.id
  deregistration_delay              = "300"
  ip_address_type                   = "ipv4"
  load_balancing_algorithm_type     = "round_robin"
  load_balancing_anomaly_mitigation = "off"
  load_balancing_cross_zone_enabled = "use_load_balancer_configuration"
  port                              = 8080
  protocol                          = "HTTP"
  protocol_version                  = "HTTP1"
  slow_start                        = 0
  tags                              = {}
  tags_all                          = {}
  target_type                       = "ip"

  health_check {
    enabled             = true
    healthy_threshold   = 5
    interval            = 30
    matcher             = "200"
    path                = "/api/auth/healthcheck"
    port                = "traffic-port"
    protocol            = "HTTP"
    timeout             = 5
    unhealthy_threshold = 2
  }

  stickiness {
    cookie_duration = 86400
    cookie_name     = null
    enabled         = false
    type            = "lb_cookie"
  }
}

resource "aws_lb_target_group" "api" {
  name        = "lb_tg_api_${var.project_name}"
  description = "Target group for the api server of ${var.project_name}."
  vpc_id = aws_vpc.main.id
  deregistration_delay              = "300"
  ip_address_type                   = "ipv4"
  load_balancing_algorithm_type     = "round_robin"
  load_balancing_anomaly_mitigation = "off"
  load_balancing_cross_zone_enabled = "use_load_balancer_configuration"
  port                              = 8080
  protocol                          = "HTTP"
  protocol_version                  = "HTTP1"
  slow_start                        = 0
  tags                              = {}
  tags_all                          = {}
  target_type                       = "ip"

  health_check {
    enabled             = true
    healthy_threshold   = 5
    interval            = 30
    matcher             = "200"
    path                = "/api/v1/healthcheck"
    port                = "traffic-port"
    protocol            = "HTTP"
    timeout             = 5
    unhealthy_threshold = 2
  }

  stickiness {
    cookie_duration = 86400
    cookie_name     = null
    enabled         = false
    type            = "lb_cookie"
  }
}

resource "aws_lb_target_group" "frontend" {
  name        = "lb_tg_frontend_${var.project_name}"
  description = "Target group for the frontend server of ${var.project_name}."
  vpc_id = aws_vpc.main.id
  deregistration_delay              = "300"
  ip_address_type                   = "ipv4"
  load_balancing_algorithm_type     = "round_robin"
  load_balancing_anomaly_mitigation = "off"
  load_balancing_cross_zone_enabled = "use_load_balancer_configuration"
  port                              = 3000
  protocol                          = "HTTP"
  protocol_version                  = "HTTP1"
  slow_start                        = 0
  tags                              = {}
  tags_all                          = {}
  target_type                       = "ip"

  health_check {
    enabled             = true
    healthy_threshold   = 5
    interval            = 30
    matcher             = "200"
    path                = "/sign-in"
    port                = "traffic-port"
    protocol            = "HTTP"
    timeout             = 5
    unhealthy_threshold = 2
  }

  stickiness {
    cookie_duration = 86400
    cookie_name     = null
    enabled         = false
    type            = "lb_cookie"
  }
}

