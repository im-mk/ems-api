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
