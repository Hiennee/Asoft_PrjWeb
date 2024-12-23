using System.Linq;
using TestPrj3.Models;

namespace TestPrj3.Services
{
    public class PrimaryKeyConfigService
    {
        public readonly Asoft2Context _context;

        public PrimaryKeyConfigService(Asoft2Context context)
        {
            _context = context;
        }

        public ICollection<PrimaryKeyConfig> GetAllConfigs()
        {
            return _context.PrimaryKeyConfigs.ToList();
        }

        public PrimaryKeyConfig GetConfigByTableName(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            return _context.PrimaryKeyConfigs.FirstOrDefault(c => c.TableName.Equals(tableName));
        }

        public void AddConfig(PrimaryKeyConfig config)
        {
            if (string.IsNullOrEmpty(config.TableName) || string.IsNullOrEmpty(config.Format) ||
                string.IsNullOrEmpty(config.Symbol1) || string.IsNullOrEmpty(config.Symbol2) ||
                string.IsNullOrEmpty(config.Symbol3) || config.NumberLength <= 0)
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            _context.PrimaryKeyConfigs.Add(config);
            _context.SaveChanges();
        }

        public void UpdateConfig(int id, string tableName, string format, string symbol1, string symbol2, string symbol3, int numberLength, int lastKey)
        {
            if (id <= 0 || string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(format) ||
                string.IsNullOrEmpty(symbol1) || string.IsNullOrEmpty(symbol2) ||
                string.IsNullOrEmpty(symbol3) || numberLength <= 0)
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            var config = _context.PrimaryKeyConfigs.FirstOrDefault(c => c.Id == id);
            if (config == null)
            {
                throw new Exception("NOT_FOUND");
            }
            config.TableName = tableName;
            config.Format = format;
            config.Symbol1 = symbol1;
            config.Symbol2 = symbol2;
            config.Symbol3 = symbol3;
            config.NumberLength = numberLength;
            config.LastKey = lastKey;
            _context.SaveChanges();
        }

        public void DeleteConfig(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            var config = _context.PrimaryKeyConfigs.FirstOrDefault(c => c.Id == id);
            if (config == null)
            {
                throw new Exception("NOT_FOUND");
            }
            _context.PrimaryKeyConfigs.Remove(config);
            _context.SaveChanges();
        }
        public string GeneratePrimaryKey(string table)
        {
            _context.SaveChanges();
            var config = _context.PrimaryKeyConfigs.FirstOrDefault(pkc => pkc.TableName.Equals(table));
            if (config == null)
            {
                throw new Exception("INVALID_TABLE");
            }
            string s1 = config.Symbol1;
            string s2 = config.Symbol2;
            string s3 = config.Symbol3;
            int numberLength = config.NumberLength;
            int lastKey = config.LastKey;
            string pkNumSuffix = lastKey.ToString($"D{numberLength}");
            return $"{config.Format.Replace("S1", s1)
                                   .Replace("S2", s2)
                                   .Replace("S3", s3)
                                   .Replace(new string('N', numberLength), pkNumSuffix)}";
        }
    }
}
