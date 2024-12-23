using System.Linq;
using TestPrj3.Models;

namespace TestPrj3.Services
{
    public class PositionService
    {
        private readonly Asoft2Context _context;

        public PositionService(Asoft2Context context)
        {
            _context = context;
        }

        public ICollection<Position> GetAllPositions()
        {
            return _context.Positions.ToList();
        }

        public ICollection<Position> GetPositionsByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            return _context.Positions.Where(p => p.PositionName.Contains(name)).ToList();
        }

        public Position GetPositionById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            return _context.Positions.FirstOrDefault(p => p.PositionId.Equals(id));
        }

        public void AddPosition(Position position)
        {
            if (string.IsNullOrEmpty(position.PositionId) || string.IsNullOrEmpty(position.PositionName))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            var pkConfig = _context.PrimaryKeyConfigs.FirstOrDefault(pkc => pkc.TableName.Equals("Position"));

            position.PositionId = new PrimaryKeyConfigService(_context).GeneratePrimaryKey("Position");
            _context.Positions.Add(position);
            pkConfig.LastKey += 1;
            _context.PrimaryKeyConfigs.Update(pkConfig);
            _context.SaveChanges();
        }

        public void UpdatePosition(string id, string name, string? description)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            id = id.Replace("%2F", "/");
            var position = _context.Positions.FirstOrDefault(p => p.PositionId.Equals(id));
            if (position == null)
            {
                throw new Exception("NOT_FOUND");
            }
            position.PositionName = name;
            position.Description = description;
            _context.SaveChanges();
        }

        public void DeletePosition(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            id = id.Replace("%2F", "/");
            var position = _context.Positions.FirstOrDefault(p => p.PositionId.Equals(id));
            if (position == null)
            {
                throw new Exception("NOT_FOUND");
            }
            if (position.Employees.Any())
            {
                throw new Exception("RELATED");
            }
            _context.Positions.Remove(position);
            _context.SaveChanges();
        }
    }
}
