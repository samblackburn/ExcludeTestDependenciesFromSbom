using System.Globalization;
using CsvHelper;

using var reader = new StreamReader(args.FirstOrDefault() ?? @"../../../input/input.csv");
using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
var records = csv.GetRecords<CsvRow>();
var filteredRecords = new List<CsvRow>();
foreach (var record in records)
{
    var filteredProjects = record.projects
        .Split(", ")
        .Where(p => !p.Contains("\\test", StringComparison.OrdinalIgnoreCase))
        .ToArray();

    if (!filteredProjects.Any()) continue;
    
    filteredRecords.Add(record with { projects = string.Join(", ", filteredProjects) });
}

Directory.CreateDirectory("../../../output");
using var writer = new StreamWriter(File.Open("../../../output/output.csv", FileMode.Create));
using var filteredCsv = new CsvWriter(writer, CultureInfo.CurrentCulture);
filteredCsv.WriteRecords(filteredRecords);

// ReSharper disable InconsistentNaming
// ReSharper disable NotAccessedPositionalProperty.Global
public record CsvRow(
    string id,
    string name,
    Version version,
    PackageManager type,
    int issuesCritical,
    int issuesHigh,
    int issuesMedium,
    int issuesLow,
    string dependenciesWithIssues,
    string licenses,
    string projects,
    Uri licenseUrls,
    Version latestVersion,
    DateTime? latestVersionPublishedDate,
    DateTime? firstPublishedDate,
    bool? isDeprecated);

public enum PackageManager { npm, nuget }