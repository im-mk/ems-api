provider "aws" {
    region = var.region
    profile = var.profile
}

provider "github" {
  owner = "im-mk"
    # version = "2.4.0"
    # token        = jsondecode(data.aws_secretsmanager_secret_version.github.secret_string)["token"]
    # individual   = false
    # organization = "${var.github_organisation}"
}

resource "aws_s3_bucket" "ems_codepipeline_bucket" {
    bucket = "ems-codepipeline-bucket"
    
    tags = {
        App = var.app_name
    }
}

resource "aws_s3_bucket_acl" "ems_codepipeline_bucket_acl" {
  bucket = aws_s3_bucket.ems_codepipeline_bucket.id
  acl    = "private"
}

resource "aws_iam_role" "ems_codepipeline_role" {
    name = "ems_codepipeline_role"

    assume_role_policy = <<EOF
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Principal": {
        "Service": [
          "codepipeline.amazonaws.com",
          "codebuild.amazonaws.com"
        ]
      },
      "Action": "sts:AssumeRole"
    }
  ]
}
EOF
}

resource "aws_iam_role_policy" "ems_codepipeline_policy" {
    name = "ems_codepipeline_policy"
    role = "${aws_iam_role.ems_codepipeline_role.id}"

    policy = <<EOF
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Resource": [
        "*"
      ],
      "Action": [
        "logs:CreateLogGroup",
        "logs:CreateLogStream",
        "logs:PutLogEvents"
      ]
    },
    {
      "Effect":"Allow",
      "Action": [
        "s3:GetObject",
        "s3:GetObjectVersion",
        "s3:GetBucketVersioning",
        "s3:PutObject"
      ],
      "Resource": [
        "${aws_s3_bucket.ems_codepipeline_bucket.arn}",
        "${aws_s3_bucket.ems_codepipeline_bucket.arn}/*"
      ]
    },
    {
      "Effect": "Allow",
      "Action": [
        "codebuild:BatchGetBuilds",
        "codebuild:StartBuild"
      ],
      "Resource": "*"
    }
  ]
}
EOF
}