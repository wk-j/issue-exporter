using System.Collections.Generic;
using Octokit;

namespace IssueExporter {
    class MyIssue {
        public int Id { set; get; }
        public string CreatedAt { set; get; }
        public string Title { set; get; }
        public string Body { set; get; }
        public ItemState State { set; get; }
        public string Label { set; get; }
    }
}
