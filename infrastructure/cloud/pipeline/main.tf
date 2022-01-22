provider "aws" {
    region = var.region
    profile = var.profile
}

data "aws_secretsmanager_secret" "github" {
    name = "github"
}

data "aws_secretsmanager_secret_version" "github" {
    secret_id = "${data.aws_secretsmanager_secret.github.id}"    
}

provider "github" {
    version = "2.4.0"
    token        = jsondecode(data.aws_secretsmanager_secret_version.github.secret_string)["token"]
    individual   = false
    organization = "${var.github_organisation}"
}

resource "aws_s3_bucket" "ems_codepipeline_bucket" {
    bucket = "ems-codepipeline-bucket"
    acl    = "private"
    
    tags = {
        App = var.app_name
    }
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

resource "aws_codebuild_project" "ems_codebuild_api" {
  name          = "ems_codebuild_api"
  description   = "build ems api"
  build_timeout = "5"
  service_role  = "${aws_iam_role.ems_codepipeline_role.arn}"

  artifacts {
    type = "CODEPIPELINE"
  }

  cache {
    type     = "S3"
    location = "${aws_s3_bucket.ems_codepipeline_bucket.bucket}"
  }

  environment {
    compute_type = "BUILD_GENERAL1_SMALL"
    image        = "aws/codebuild/standard:4.0"
    type         = "LINUX_CONTAINER"
  }

  source {
    type = "CODEPIPELINE"
  }

  tags = {
    App = var.app_name
  }
}

resource "aws_codepipeline" "ems_codepipeline" {
    name     = "ems_codepipeline"
    role_arn = "${aws_iam_role.ems_codepipeline_role.arn}"

    artifact_store {
        location = "${aws_s3_bucket.ems_codepipeline_bucket.bucket}"
        type     = "S3"
    }

    stage {
        name = "Source"

        action {
            name             = "Source"
            category         = "Source"
            owner            = "ThirdParty"
            provider         = "GitHub"
            version          = "1"
            output_artifacts = ["source_output"]

            configuration = {
                Owner  = var.github_organisation
                Repo   = var.github_repository
                Branch = "master"
                OAuthToken = jsondecode(data.aws_secretsmanager_secret_version.github.secret_string)["token"]
                PollForSourceChanges = false
            }
        }
    }

    stage {
        name = "Build"

        action {
            name             = "Build"
            category         = "Build"
            owner            = "AWS"
            provider         = "CodeBuild"
            input_artifacts  = ["source_output"]
            output_artifacts = ["build_output"]
            version          = "1"

            configuration = {
                ProjectName = "${aws_codebuild_project.ems_codebuild_api.name}"
            }
        }
    }
}

resource "aws_codepipeline_webhook" "ems_webhook" {
  name            = "test-webhook-github-bar"
  authentication  = "GITHUB_HMAC"
  target_action   = "Source"
  target_pipeline = "${aws_codepipeline.ems_codepipeline.name}"

  authentication_configuration {
    secret_token = jsondecode(data.aws_secretsmanager_secret_version.github.secret_string)["webhook_secret"]
  }

  filter {
    json_path    = "$.ref"
    match_equals = "refs/heads/{Branch}"
  }
}

# Wire the CodePipeline webhook into a GitHub repository.
resource "github_repository_webhook" "repo_hook" {
  repository = var.github_repository 

  configuration {
    url          = "${aws_codepipeline_webhook.ems_webhook.url}"
    content_type = "json"
    insecure_ssl = true
    secret       = jsondecode(data.aws_secretsmanager_secret_version.github.secret_string)["webhook_secret"]
  }

  events = ["push"]
}
