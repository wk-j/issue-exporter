using System;
using System.Threading.Tasks;
using Octokit;
using System.Linq;
using System.IO;
using CsvHelper;
using System.Collections.Generic;
using TableMaker;

namespace IssueExporter {

    class Program {

        static void WriteToFile(string fileName, IEnumerable<MyIssue> data) {
            using (var writer = new StreamWriter(fileName))
            using (var csv = new CsvWriter(writer)) {
                csv.WriteRecords(data);
            }
        }

        static void WriteToConsole(IEnumerable<MyIssue> data) {
            var array = new string[data.Count() + 1, 3];
            array[0, 0] = "Created At";
            array[0, 1] = "Title";
            array[0, 2] = "Status";

            foreach (var (x, i) in data.Select((x, i) => (x, i))) {
                var next = i + 1;
                array[next, 0] = x.CreatedAt;
                // array[next, 1] = x.Title.Length > 50 ? x.Title.Substring(0, 50) + "..." : x.Title;
                array[next, 1] = x.Title;
                array[next, 2] = x.State.ToString();
            }

            ArrayPrinter.PrintToConsole(array);
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

            WriteToConsole(data.Take(10));
            WriteToFile(fileName, data);
        }
    }
}
