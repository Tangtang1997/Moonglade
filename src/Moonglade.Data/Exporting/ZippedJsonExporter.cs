﻿using System.IO.Compression;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;

namespace Moonglade.Data.Exporting;

public class ZippedJsonExporter<T>(MoongladeRepository<T> repository, string fileNamePrefix, string directory)
    where T : class
{
    public async Task<ExportResult> ExportData<TResult>(Expression<Func<T, TResult>> selector, CancellationToken ct)
    {
        var data = await repository.SelectAsync(selector, ct);
        var result = await ToZippedJsonResult(data, ct);
        return result;
    }

    private async Task<ExportResult> ToZippedJsonResult<TE>(IEnumerable<TE> list, CancellationToken ct)
    {
        var tempId = Guid.NewGuid().ToString();
        string exportDirectory = CreateExportDirectory(directory, tempId);
        foreach (var item in list)
        {
            var json = JsonSerializer.Serialize(item, MoongladeJsonSerializerOptions.Default);
            await SaveJsonToDirectory(json, exportDirectory, $"{Guid.NewGuid()}.json", ct);
        }

        var distPath = Path.Join(directory, "export", $"{fileNamePrefix}-{DateTime.UtcNow:yyyy-MM-dd-HH-mm-ss}.zip");
        ZipFile.CreateFromDirectory(exportDirectory, distPath);

        return new()
        {
            FilePath = distPath
        };
    }

    private static async Task SaveJsonToDirectory(string json, string directory, string filename, CancellationToken ct)
    {
        var path = Path.Join(directory, filename);
        await File.WriteAllTextAsync(path, json, Encoding.UTF8, ct);
    }

    private static string CreateExportDirectory(string directory, string subDirName)
    {
        if (directory is null) return null;

        var path = Path.Join(directory, "export", subDirName);
        if (Directory.Exists(path))
        {
            Directory.Delete(path);
        }

        Directory.CreateDirectory(path);
        return path;
    }
}