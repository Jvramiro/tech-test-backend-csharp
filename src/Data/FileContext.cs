using SharpCompress.Common;
using System.Text.Json;

namespace TestJrAPI.Data {
    public class FileContext {

        private readonly string filePath;

        public FileContext(IConfiguration configuration) {
            this.filePath = configuration["Database:FilePath"];
            Console.WriteLine(filePath);
        }

        public async Task CreateAsync<T>(T item) {
            var items = await ReadAsync<T>();

            var itemType = typeof(T);
            var idProperty = itemType.GetProperty("Id");
            if (idProperty != null && idProperty.PropertyType == typeof(Guid)) {
                idProperty.SetValue(item, Guid.NewGuid());
            }

            items.Add(item);
            await WriteAsync(items);
        }
        public async Task<List<T>> GetAllAsync<T>() {
            var items = await ReadAsync<T>();
            return items;
        }

        public async Task<T> GetByIdAsync<T>(Predicate<T> predicate) {
            var items = await ReadAsync<T>();
            return items.Find(predicate);
        }

        public async Task UpdateAsync<T>(Predicate<T> predicate, T updatedItem) {
            var items = await ReadAsync<T>();
            var existingItem = items.Find(predicate);
            if (existingItem != null) {
                var index = items.IndexOf(existingItem);
                items[index] = updatedItem;
                await WriteAsync(items);
            }
        }

        public async Task DeleteAsync<T>(Predicate<T> predicate) {
            var items = await ReadAsync<T>();
            var itemToRemove = items.Find(predicate);
            if (itemToRemove != null) {
                items.Remove(itemToRemove);
                await WriteAsync(items);
            }
        }

        private async Task<List<T>> ReadAsync<T>() {
            if (File.Exists(filePath)) {
                var json = await File.ReadAllTextAsync(filePath);
                return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            return new List<T>();
        }

        private async Task WriteAsync<T>(List<T> items) {
            var json = JsonSerializer.Serialize(items);

            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }

            await File.WriteAllTextAsync(filePath, json);
        }
    }
}
