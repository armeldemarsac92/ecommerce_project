locals {
  domains = [
    "epitechproject.fr",
    "api.epitechproject.fr",
    "auth.epitechproject.fr"
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

resource "aws_route53_zone" "private" {
  name = "internal.epitechproject.fr"

  vpc {
    vpc_id = aws_vpc.main.id
  }
}

resource "aws_route53_record" "db" {
  zone_id = aws_route53_zone.private.id
  name    = "db.internal.epitechproject.fr"
  type    = "CNAME"
  ttl     = 60
  records = [aws_db_instance.postgresql.address]
}

resource "aws_route53_record" "bastion" {
  zone_id = data.aws_route53_zone.main.zone_id
  name    = "bastion.epitechproject.fr"
  type    = "A"
  ttl     = 60
  records = [aws_instance.bastion.public_ip]
}
