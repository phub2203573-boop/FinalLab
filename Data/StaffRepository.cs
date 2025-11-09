using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using HRDepartment.Models;

namespace HRDepartment.Data
{
    public class StaffRepository
    {
        private readonly string _filePath;
        private readonly object _lock = new object();
        private List<Staff> _items;

        public StaffRepository(string contentRoot)
        {
            var dataDir = Path.Combine(contentRoot, "App_Data");
            if (!Directory.Exists(dataDir)) Directory.CreateDirectory(dataDir);
            _filePath = Path.Combine(dataDir, "staffs.json");

            if (File.Exists(_filePath))
            {
                try
                {
                    var json = File.ReadAllText(_filePath);
                    _items = JsonSerializer.Deserialize<List<Staff>>(json) ?? new List<Staff>();
                }
                catch
                {
                    _items = new List<Staff>();
                }
            }
            else
            {
                _items = new List<Staff>();
                Save();
            }
        }

        private void Save()
        {
            lock (_lock)
            {
                File.WriteAllText(_filePath, JsonSerializer.Serialize(_items, new JsonSerializerOptions { WriteIndented = true }));
            }
        }

        public IEnumerable<Staff> GetAll()
        {
            lock (_lock)
            {
                return _items.OrderBy(s => s.Id).ToList();
            }
        }

        public Staff? GetById(int id)
        {
            lock (_lock)
            {
                return _items.FirstOrDefault(s => s.Id == id);
            }
        }

        public bool ExistsByStaffId(string staffId)
        {
            if (string.IsNullOrWhiteSpace(staffId)) return false;
            lock (_lock)
            {
                return _items.Any(s => string.Equals(s.StaffId?.Trim(), staffId.Trim(), StringComparison.OrdinalIgnoreCase));
            }
        }

        public void Add(Staff s)
        {
            lock (_lock)
            {
                s.Id = (_items.Any() ? _items.Max(x => x.Id) : 0) + 1;
                _items.Add(s);
                Save();
            }
        }
    }
}
