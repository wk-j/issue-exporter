## Issue Exporter

[![Build Status](https://dev.azure.com/wk-j/issue-exporter/_apis/build/status/wk-j.issue-exporter?branchName=master)](https://dev.azure.com/wk-j/issue-exporter/_build/latest?definitionId=30&branchName=master)
[![NuGet](https://img.shields.io/nuget/v/wk.IssueExporter.svg)](https://www.nuget.org/packages/wk.IssueExporter)

## Installation

```bash
dotnet tool install -g wk.IssueExporter
```

## Usage

```bash
wk-issue-exporter --repo bcircle/practika-crm --token $GITHUB_TOKEN
wk-issue-exporter --repo bcircle/practika-sale-tracking-frontend  --token $GITHUB_TOKEN
```