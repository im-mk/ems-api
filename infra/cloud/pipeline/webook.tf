data "aws_secretsmanager_secret" "github" {
    name = "github"
}

data "aws_secretsmanager_secret_version" "github" {
    secret_id = "${data.aws_secretsmanager_secret.github.id}"    
}

resource "aws_codepipeline_webhook" "ems_webhook" {
  name            = "ems-codepipeline-webhook"
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

resource "github_repository" "repo" {
  name         = var.github_repository
  description  = "ems"
  homepage_url = "http://example.com/"
}

# Wire the CodePipeline webhook into a GitHub repository.
resource "github_repository_webhook" "repo_hook" {
  repository = github_repository.repo.name

  configuration {
    url          = "${aws_codepipeline_webhook.ems_webhook.url}"
    content_type = "json"
    insecure_ssl = true
    secret       = jsondecode(data.aws_secretsmanager_secret_version.github.secret_string)["webhook_secret"]
  }

  events = ["push"]
}
