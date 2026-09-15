namespace UnionExample
{
    public sealed class UserService
    {
        // You do not need to change this path if running locally, CSProj puts it into the bin
        private const string FILEPATH = "Users.txt";
        private Dictionary<string, int> _cache = new();

        public static UserService Instance { get; } = new();

        public void Initialise()
        {
            EnsureFileEndsWithNewLine();
            RefreshUserCache();
        }

        private void EnsureFileEndsWithNewLine()
        {
            string contents = File.ReadAllText(FILEPATH);

            if (contents.Length > 0 && !contents.EndsWith('\n'))
            {
                File.AppendAllText(FILEPATH, Environment.NewLine);
            }
        }

        public void RefreshUserCache()
        {
            _cache = ReadUsersFromFile();
        }

        private Dictionary<string, int> ReadUsersFromFile()
        {
            var lines = File.ReadAllLines(FILEPATH);

            return lines
                .Select(ParseUser)
                .ToDictionary(entry => entry.Name.ToLowerInvariant(), entry => entry.Id);
        }

        public UserResult GetOrAddUser(string name)
        {
            if (NameExistsInCache(name))
            {
                return GetIdFromCache(name);
            }

            try
            {
                RefreshUserCache();

                if (NameExistsInCache(name))
                {
                    return GetIdFromCache(name);
                }

                return AddUser(name);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }


        #region CacheHelpers

        private bool NameExistsInCache(string name)
        {
            return _cache.ContainsKey(name);
        }

        private int GetIdFromCache(string name)
        {
            return _cache[name];
        }

        private (string Name, int Id) ParseUser(string line)
        {
            var parts = line.Split(',');

            return (parts[0], int.Parse(parts[1]));
        }

        #endregion

        #region AddUser

        private string AddUser(string name)
        {
            int newId = GetNextId();

            AddUserToFile(name, newId);
            CheckUserWasAdded(name, newId);

            _cache[name] = newId;

            return $"{name} ({newId}) has been successfully added.";
        }

        private int GetNextId()
        {
            return _cache.Values.Max() + 1;
        }

        private void AddUserToFile(string name, int id)
        {
            File.AppendAllText(FILEPATH, 
                $"{name},{id}{Environment.NewLine}"
            );
        }

        private void CheckUserWasAdded(string name, int id)
        {
            var latestCache = ReadUsersFromFile();

            if (!latestCache.TryGetValue(name, out int storedId) ||
                storedId != id)
            {
                throw new IOException(
                    "The name could not be verified after being added."
                );
            }
        }

        #endregion
    }
}