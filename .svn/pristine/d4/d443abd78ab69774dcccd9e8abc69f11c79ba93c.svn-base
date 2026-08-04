using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text.Json.Serialization;
namespace Infrastructure.Provider
{
    public static class ListProvider
    {
        public static List<T> ReadFileToList<T>(IFormFile file, string sheetName) where T : new()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            using var stream = file.OpenReadStream();

            return fileExtension switch
            {
                ".csv" => ReadCsvToList<T>(stream),
                ".xlsx" or ".xls" => ReadExcelToList<T>(stream, sheetName),
                _ => throw new NotSupportedException("Unsupported file format. Please upload CSV or Excel.")
            };
        }

        private static List<T> ReadCsvToList<T>(Stream stream) where T : new()
        {
            List<T> list = new();
            using var reader = new StreamReader(stream);

            string[] headers = reader.ReadLine()?.Split(',') ?? throw new Exception("Invalid CSV format");

            // Map properties by JsonPropertyName or PropertyName
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToDictionary(
                    p => p.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? p.Name,
                    p => p
                );

            while (!reader.EndOfStream)
            {
                string[] values = reader.ReadLine()?.Split(',') ?? Array.Empty<string>();
                T obj = new();

                for (int i = 0; i < values.Length && i < headers.Length; i++)
                {
                    if (properties.TryGetValue(headers[i], out var property))
                    {
                        object convertedValue = ConvertValue(values[i], property.PropertyType);
                        property.SetValue(obj, convertedValue);
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        private static List<T> ReadExcelToList<T>(Stream stream, string? sheetName) where T : new()
        {
            using var reader = ExcelReaderFactory.CreateReader(stream);
            var result = reader.AsDataSet();

            var table = result.Tables[0]; // Read first worksheet

            List<T> list = new();

            // Map properties by JsonPropertyName or PropertyName
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToDictionary(
                    p => p.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? p.Name,
                    p => p
                );

            // Read headers
            Dictionary<int, PropertyInfo> columnMapping = new();
            for (int col = 0; col < table.Columns.Count; col++)
            {
                string columnName = table.Rows[0][col].ToString()?.Trim();
                if (!string.IsNullOrEmpty(columnName) && properties.TryGetValue(columnName, out var property))
                {
                    columnMapping[col] = property;
                }
            }

            // Read data rows
            for (int row = 1; row < table.Rows.Count; row++) // Start from row 1 (skip header)
            {
                T obj = new();
                foreach (var colMap in columnMapping)
                {
                    int colIndex = colMap.Key;
                    var property = colMap.Value;
                    string cellValue = table.Rows[row][colIndex].ToString()?.Trim().Trim('\'');

                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        object convertedValue = ConvertValue(cellValue, property.PropertyType);
                        property.SetValue(obj, convertedValue);
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        private static object ConvertValue(string value, Type type)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            Type targetType = Nullable.GetUnderlyingType(type) ?? type;

            try
            {
                return Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
            }
            catch
            {
                return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
            }
        }
        

    }
}