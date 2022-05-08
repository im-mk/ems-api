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
                Branch = var.github_branch
                OAuthToken = jsondecode(data.aws_secretsmanager_secret_version.github.secret_string)["webhook_secret"]
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
