using Octokit;

namespace IssueExporter {
    class MyIssue {
        public string CreatedAt { set; get; }
        public string Title { set; get; }
        public ItemState State { set; get; }
    }
}
