using System;
using System.Threading.Tasks;
using Octokit;
using System.Linq;
using System.IO;
using CsvHelper;

namespace IssueExporter {
    class MyIssue {
        public string CreatedAt { set; get; }
        public string Title { set; get; }
        public ItemState State { set; get; }
    }

    class Program {
        static async Task Main(
                string repo = "wk-j/issue-exporter",
                string token = ""
            ) {

            var gh = new GitHubClient(new ProductHeaderValue("wk-app"));
            gh.Credentials = new Credentials(token);

            var tokens = repo.Split("/");
            var org = tokens[0];
            var repository = tokens[1];

            var issues = await gh.Issue.GetAllForRepository(org, repository, new RepositoryIssueRequest {
                State = ItemStateFilter.All
            });

            var data = issues.OrderBy(x => x.CreatedAt).Select(x => new MyIssue {
                CreatedAt = x.CreatedAt.DateTime.ToString("dd/MM/yyyy"),
                Title = x.Title,
                State = x.State.Value
            });

            var fileName = repo.Replace("/", ".") + ".csv";

            using (var writer = new StreamWriter(fileName))
            using (var csv = new CsvWriter(writer)) {
                csv.WriteRecords(data);
            }
        }
    }
}
