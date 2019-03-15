using System;
using System.Threading.Tasks;
using Octokit;
using System.Linq;
using System.IO;
using CsvHelper;
using System.Collections.Generic;

namespace IssueExporter {

    class Program {

        static void WriteToFile(string fileName, IEnumerable<MyIssue> data) {
            using (var writer = new StreamWriter(fileName))
            using (var csv = new CsvWriter(writer)) {
                csv.WriteRecords(data);
            }
        }

        static async Task<IEnumerable<MyIssue>> GetIssuesAsync(string org, string repo, string token) {
            var gh = new GitHubClient(new ProductHeaderValue("wk-app")) {
                Credentials = new Credentials(token)
            };

            var issues = await gh.Issue.GetAllForRepository(org, repo, new RepositoryIssueRequest {
                State = ItemStateFilter.All
            });

            return issues.OrderBy(x => x.CreatedAt).Select(x => new MyIssue {
                CreatedAt = x.CreatedAt.DateTime.ToString("dd/MM/yyyy"),
                Title = x.Title,
                State = x.State.Value
            });
        }

        static async Task Main(string repo = null, string token = null) {
            if (repo == null) return;
            if (token == null) return;

            var tokens = repo.Split("/");
            var org = tokens[0];
            var repository = tokens[1];
            var fileName = repo.Replace("/", ".") + ".csv";

            var data = await GetIssuesAsync(org: org, repo: repository, token: token);
            WriteToFile(fileName, data);
        }
    }
}
