locals {
  domains = [
    "epitechproject.fr",
    "api.epitechproject.fr",
    "authentication.epitechproject.fr"
  ]
}

data "aws_route53_zone" "main" {
  name = "epitechproject.fr."  
}

resource "aws_route53_record" "a_records" {
  for_each = toset(local.domains)

  zone_id = data.aws_route53_zone.main.zone_id
  name    = each.value
  type    = "A"

  alias {
    name                   = aws_lb.main.dns_name
    zone_id                = aws_lb.main.zone_id
    evaluate_target_health = true
  }
}

resource "aws_route53_record" "aaaa_records" {
  for_each = toset(local.domains)

  zone_id = data.aws_route53_zone.main.zone_id
  name    = each.value
  type    = "AAAA"

  alias {
    name                   = aws_lb.main.dns_name
    zone_id                = aws_lb.main.zone_id
    evaluate_target_health = true
  }
}