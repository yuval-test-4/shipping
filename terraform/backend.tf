terraform {
  backend "s3" {
    bucket = "terraform-state-demonstration"
    key    = "development/shipping"
    region = "us-east-1"
  }
}